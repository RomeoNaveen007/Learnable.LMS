using Learnable.Application.Interfaces.Services;
using Learnable.Domain.Common.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Learnable.Infrastructure.Implementations.Services.Internal
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSetting _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpSetting> options, ILogger<EmailService> logger)
        {
            _settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<bool> SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default)
        {
            return SendAsync(new[] { toEmail }, subject, htmlBody, cancellationToken);
        }

        public async Task<bool> SendAsync(IEnumerable<string> toEmails, string subject, string htmlBody, CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();

            // From address (taken from config)
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));

            // To addresses
            foreach (var to in toEmails)
            {
                if (!string.IsNullOrWhiteSpace(to))
                    message.To.Add(MailboxAddress.Parse(to));
            }

            message.Subject = subject ?? string.Empty;

            var builder = new BodyBuilder { HtmlBody = htmlBody ?? string.Empty };
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();

            try
            {
                // Choose SecureSocketOptions depending on your port and server
                var secure = _settings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;

                try
                {
                    // ❗ CONNECTION EXCEPTION HANDLED HERE
                    await client.ConnectAsync(_settings.Host, _settings.Port, secure, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SMTP connection failed. Host: {Host}", _settings.Host);

                    // Do NOT throw → prevents application crash
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(_settings.Username))
                {
                    await client.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
                }

                await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);

                _logger.LogInformation("Email successfully sent to {Recipients}", string.Join(", ", toEmails));

                return true;

            }

            catch (SmtpCommandException ex)
            {
                // Gmail sending limit exceeded
                if (ex.Message.Contains("5.4.5"))
                {
                    _logger.LogError("Gmail sending limit exceeded. Try again after 24 hours.");
                    return false; // do not crash the app
                }

                _logger.LogError(ex, "SMTP command error");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected email error");
                return false;
            }

        }
    }
}

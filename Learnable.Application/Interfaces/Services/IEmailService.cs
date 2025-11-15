using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default);
        Task SendAsync(IEnumerable<string> toEmails, string subject, string htmlBody, CancellationToken cancellationToken = default);

    }
}

using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Application.Interfaces.Services;
using Learnable.Domain.Common.OTP;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Application.Features.Verifications.Commands.SendOtp
{
    public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, bool>
    {
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public SendOtpCommandHandler(IUserRepository userRepo, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<bool> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            // Generate 4-digit OTP
            var otpCode = new Random().Next(1000, 9999).ToString();

            // Remove old OTP (if exists)
            var oldOtp = await _userRepo.GetOtpByEmailAsync(request.Email, cancellationToken);
            if (oldOtp != null)
            {
                await _userRepo.DeleteOtpAsync(oldOtp, cancellationToken);
            }

            // Create new OTP entry
            var otp = new UserOtp
            {
                Email = request.Email,
                OtpCode = otpCode,
                Attempts = 0,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };

            await _userRepo.AddOtpAsync(otp, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Send OTP email
            string subject = "Your OTP Code";
            string body = $"<h2>Your OTP Code is: <b>{otpCode}</b></h2>";
            await _emailService.SendAsync(request.Email, subject, body, cancellationToken);

            return true;
        }
    }
}

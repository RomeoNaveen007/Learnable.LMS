using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Application.Interfaces.Services;
using Learnable.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Account.Commands.RegisterTeacher
{
    public class RegisterTeacherCommandHandler : IRequestHandler<RegisterTeacherCommand, TeacherUserDto>
    {
        private readonly IUserRepository _userRepo;
        private readonly ITeacherRepository _teacherRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public RegisterTeacherCommandHandler(
            IUserRepository userRepo,
            ITeacherRepository teacherRepo,
            IUnitOfWork unitOfWork,
            IPasswordService passwordService,
            ITokenService tokenService,
            IEmailService emailService)
        {
            _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
            _teacherRepo = teacherRepo ?? throw new ArgumentNullException(nameof(teacherRepo));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<TeacherUserDto> Handle(RegisterTeacherCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // 🔹 OTP validation
            var otpRecord = await _userRepo.GetOtpByEmailAsync(dto.Email, cancellationToken);
            if (otpRecord == null)
                throw new Exception("OTP not generated. Please request OTP first.");

            if (otpRecord.ExpiresAt < DateTime.UtcNow)
                throw new Exception("OTP expired. Please request a new OTP.");

            if (otpRecord.OtpCode != request.OtpCode)
            {
                otpRecord.Attempts++;
                if (otpRecord.Attempts >= 3)
                {
                    var newOtp = new Random().Next(1000, 9999).ToString();
                    otpRecord.OtpCode = newOtp;
                    otpRecord.Attempts = 0;
                    otpRecord.ExpiresAt = DateTime.UtcNow.AddMinutes(5);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await _emailService.SendAsync(dto.Email,
                        "New OTP Code (Retry)",
                        $"<h2>Your new OTP code is: <b>{newOtp}</b></h2>",
                        cancellationToken);

                    throw new Exception("OTP incorrect 3 times. New OTP has been sent.");
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                throw new Exception($"Incorrect OTP. Attempt {otpRecord.Attempts} of 3.");
            }

            // OTP valid → delete OTP record
            await _userRepo.DeleteOtpAsync(otpRecord, cancellationToken);

            // 🔹 Validate email & username
            if (await _userRepo.ExistsByEmailAsync(dto.Email, cancellationToken))
                throw new Exception("Email already registered.");

            if (await _userRepo.ExistsByUsernameAsync(dto.Username, cancellationToken))
                throw new Exception("Username already taken.");

            // 🔹 Create password hash
            _passwordService.CreatePasswordHash(dto.Password, out string hash, out string salt);

            // 🔹 Create User entity with Teacher role
            var user = new Learnable.Domain.Entities.User
            {
                UserId = Guid.NewGuid(),
                Email = dto.Email,
                Username = dto.Username,
                DisplayName = dto.DisplayName,
                FullName = dto.FullName,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = "Teacher",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepo.CreateAsync(user);

            // 🔹 Create linked Teacher entity
            var teacher = new Learnable.Domain.Entities.Teacher
            {
                ProfileId = Guid.NewGuid(),
                UserId = user.UserId,
                DateOfBirth = dto.DateOfBirth,
                ContactPhone = dto.ContactPhone,
                Bio = dto.Bio,
                AvatarUrl = dto.AvatarUrl,
                LastUpdatedAt = DateTime.UtcNow
            };

            await _teacherRepo.CreateAsync(teacher);

            // 🔹 Save both User + Teacher
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔹 Send welcome mail
            await _emailService.SendAsync(user.Email,
                "Teacher Registration Successful",
                $"<h2>Welcome {user.Username}!</h2><p>Your teacher account has been created successfully.</p>",
                cancellationToken);

            return new TeacherUserDto
            {
                User = user.ToDto(_tokenService),
                Teacher = teacher.ToDto()
            };
        }
    }
}

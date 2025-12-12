using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Exceptions;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Application.Interfaces.Services;
using Learnable.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Account.Commands.RegisterUser
{
    public class RegisterUserCommandHandler(
        IUserRepository userRepo,
        IClassStudentRepository classStudentRepo,
        IClassRepository classRepo,
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        ITokenService tokenService,
        IEmailService emailService)
        : IRequestHandler<RegisterUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IClassStudentRepository _classStudentRepo = classStudentRepo;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IEmailService _emailService = emailService;
        private readonly IClassRepository _classRepo = classRepo;

        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // ---------------- OTP VALIDATION ----------------
            var otpRecord = await _userRepo.GetOtpByEmailAsync(dto.Email, cancellationToken)
                ?? throw new BadRequestException("OTP not generated. Please request OTP first.");

            if (otpRecord.ExpiresAt < DateTime.UtcNow)
                throw new BadRequestException("OTP expired. Please request a new OTP.");

            if (otpRecord.OtpCode != request.OtpCode)
            {
                otpRecord.Attempts++;

                // Regenerate OTP after 3 wrong attempts
                if (otpRecord.Attempts >= 3)
                {
                    var newOtp = new Random().Next(1000, 9999).ToString();
                    otpRecord.OtpCode = newOtp;
                    otpRecord.Attempts = 0;
                    otpRecord.ExpiresAt = DateTime.UtcNow.AddMinutes(5);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await _emailService.SendAsync(dto.Email,
                        "New OTP Code Sent",
                        $"<h2>Your new OTP is <b>{newOtp}</b></h2>",
                        cancellationToken);

                    throw new BadRequestException("OTP incorrect 3 times. New OTP sent.");
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                throw new BadRequestException($"Incorrect OTP. Attempt {otpRecord.Attempts} of 3.");
            }

            await _userRepo.DeleteOtpAsync(otpRecord, cancellationToken);

            // ---------------- EMAIL + USERNAME VALIDATION ----------------
            if (await _userRepo.ExistsByEmailAsync(dto.Email, cancellationToken))
                throw new BadRequestException("Email already registered.");

            if (await _userRepo.ExistsByUsernameAsync(dto.Username, cancellationToken))
                throw new BadRequestException("Username already taken.");

            // ---------------- CREATE USER ----------------
            _passwordService.CreatePasswordHash(dto.Password, out string hash, out string salt);

            var user = new Learnable.Domain.Entities.User
            {
                UserId = Guid.NewGuid(),
                Email = dto.Email,
                Username = dto.Username,
                DisplayName = dto.DisplayName,
                FullName = dto.FullName,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = "Student",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepo.CreateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // --- Auto-join General Class ---
            var defaultClass = (await _classRepo.GetAllAsync()).FirstOrDefault(c => c.ClassName == "General Class");

            if (defaultClass == null)
            {
                defaultClass = new Learnable.Domain.Entities.Class
                {
                    ClassId = Guid.NewGuid(),
                    ClassName = "General Class",
                    ClassJoinName = "GENERAL-001",
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active",
                    Description = "Default class for all new students."
                };

                await _classRepo.CreateAsync(defaultClass);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            // Ensure classJoinName always exists
            if (string.IsNullOrWhiteSpace(defaultClass.ClassJoinName))
            {
                defaultClass.ClassJoinName = "GENERAL-001";
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            // ---------------- ADD STUDENT TO GENERAL CLASS ----------------
            if (!await _classStudentRepo.IsStudentInClassAsync(defaultClass.ClassId, user.UserId, cancellationToken))
            {
                var joinRecord = new ClassStudent
                {
                    ClassId = defaultClass.ClassId,
                    UserId = user.UserId,
                    JoinDate = DateTime.UtcNow,
                    StudentStatus = "Active"
                };

                await _classStudentRepo.CreateAsync(joinRecord);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            // ---------------- FETCH CLASSES ----------------
            var classes = await _classStudentRepo.GetClassesForStudentAsync(user.UserId, cancellationToken);

            // ---------------- SEND WELCOME EMAIL ----------------
            await _emailService.SendAsync(user.Email,
                "Registration Successful",
                $"<h2>Welcome {user.Username}!</h2><p>Your account is now active.</p>",
                cancellationToken);

            // ---------------- RETURN CLEAN USER DTO ----------------
            return user.ToUserWithClassesDto(classes, _tokenService);
        }
    }
}

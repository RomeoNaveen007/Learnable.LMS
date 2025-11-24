using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Features.Users.Commands.RegisterUser;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Application.Interfaces.Services;
using Learnable.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUserRepository _userRepo;
    private readonly IClassStudentRepository _classStudentRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IGenericRepository<Class> _classRepo;

    public RegisterUserCommandHandler(
        IUserRepository userRepo,
        IClassStudentRepository classStudentRepo,
        IGenericRepository<Class> classRepo,
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        ITokenService tokenService,
        IEmailService emailService)
    {
        _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        _classStudentRepo = classStudentRepo ?? throw new ArgumentNullException(nameof(classStudentRepo));
        _classRepo = classRepo ?? throw new ArgumentNullException(nameof(classRepo));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // --- OTP validation ---
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

        await _userRepo.DeleteOtpAsync(otpRecord, cancellationToken);

        // --- Validate email & username ---
        if (await _userRepo.ExistsByEmailAsync(dto.Email, cancellationToken))
            throw new Exception("Email already registered.");
        if (await _userRepo.ExistsByUsernameAsync(dto.Username, cancellationToken))
            throw new Exception("Username already taken.");

        // --- Create user ---
        _passwordService.CreatePasswordHash(dto.Password, out string hash, out string salt);
        var user = new User
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
        var defaultClass = (await _classRepo.GetAllAsync())
            .FirstOrDefault(c => c.ClassName == "General Class");

        if (defaultClass == null)
        {
            defaultClass = new Class
            {
                ClassId = Guid.NewGuid(),
                ClassName = "General Class",
                CreatedAt = DateTime.UtcNow,
                Status = "Active"
            };
            await _classRepo.CreateAsync(defaultClass);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        if (!await _classStudentRepo.IsStudentInClassAsync(defaultClass.ClassId, user.UserId, cancellationToken))
        {
            await _classStudentRepo.CreateAsync(new ClassStudent
            {
                ClassId = defaultClass.ClassId,
                UserId = user.UserId,
                JoinDate = DateTime.UtcNow,
                StudentStatus = "Active"
            });
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // --- Send welcome mail ---
        await _emailService.SendAsync(user.Email,
            "Registration Successful",
            $"<h2>Welcome {user.Username}!</h2><p>Your account has been created successfully.</p>",
            cancellationToken);

        // --- Return DTO ---
        return user.ToDto(_tokenService);
    }
}
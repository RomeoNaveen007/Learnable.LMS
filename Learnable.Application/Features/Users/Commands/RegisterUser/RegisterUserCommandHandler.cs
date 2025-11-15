using Learnable.Application.Features.Users;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;

    public RegisterUserCommandHandler(
        IUserRepository userRepo,
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        ITokenService tokenService,
        IEmailService emailService)
    {
        _userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // Get OTP record
        var otpRecord = await _userRepo.GetOtpByEmailAsync(dto.Email, cancellationToken);
        if (otpRecord == null)
            throw new Exception("OTP not generated. Please request OTP first.");

        // Check expiration
        if (otpRecord.ExpiresAt < DateTime.UtcNow)
            throw new Exception("OTP expired. Please request a new OTP.");

        // Validate OTP
        if (otpRecord.OtpCode != request.OtpCode)
        {
            otpRecord.Attempts++;

            if (otpRecord.Attempts >= 3)
            {
                // Generate new OTP
                var newOtp = new Random().Next(1000, 9999).ToString();
                otpRecord.OtpCode = newOtp;
                otpRecord.Attempts = 0;
                otpRecord.ExpiresAt = DateTime.UtcNow.AddMinutes(5);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Email new OTP
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

        // Validate email
        if (await _userRepo.ExistsByEmailAsync(dto.Email, cancellationToken))
            throw new Exception("Email already registered.");

        // Validate username
        if (await _userRepo.ExistsByUsernameAsync(dto.Username, cancellationToken))
            throw new Exception("Username already taken.");

        // Create password hash
        _passwordService.CreatePasswordHash(dto.Password, out string hash, out string salt);

        // Create user entity
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

        var created = await _userRepo.CreateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Send welcome mail
        await _emailService.SendAsync(user.Email,
            "Registration Successful",
            $"<h2>Welcome {user.Username}!</h2><p>Your account has been created successfully.</p>",
            cancellationToken);

        // generate jwt
        var token = _tokenService.CreateToken(user);

        // return dto
        return new UserDto
        {
            UserId = user.UserId,
            Email = user.Email,
            Username = user.Username,
            DisplayName = user.DisplayName,
            FullName = user.FullName,
            Role = user.Role,
            Token = token
        };
    }
}

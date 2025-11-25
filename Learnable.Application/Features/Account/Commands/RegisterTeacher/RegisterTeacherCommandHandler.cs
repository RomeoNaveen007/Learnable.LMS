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
    public class RegisterTeacherCommandHandler
        : IRequestHandler<RegisterTeacherCommand, TeacherUserDto>
    {
        private readonly IUserRepository _userRepo;
        private readonly ITeacherRepository _teacherRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public RegisterTeacherCommandHandler(
            IUserRepository userRepo,
            ITeacherRepository teacherRepo,
            IUnitOfWork unitOfWork,
            ITokenService tokenService)
        {
            _userRepo = userRepo;
            _teacherRepo = teacherRepo;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<TeacherUserDto> Handle(
            RegisterTeacherCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // 1. Check OTP
            var otp = await _userRepo.GetOtpByEmailAsync(dto.Email, cancellationToken);
            if (otp == null)
                throw new Exception("OTP not found. Request a new OTP.");

            if (otp.ExpiresAt < DateTime.UtcNow)
                throw new Exception("OTP expired.");

            if (otp.OtpCode != request.OtpCode)
                throw new Exception("Invalid OTP.");

            await _userRepo.DeleteOtpAsync(otp, cancellationToken);

            // 2. Load existing user
            var user = await _userRepo.GetUserByIdAsync(dto.UserId, cancellationToken);
            if (user == null)
                throw new Exception("User does not exist.");

            // 3. Check if teacher already exists
            var existingTeacher = await _teacherRepo.GetByUserIdAsync(dto.UserId, cancellationToken);
            if (existingTeacher != null)
                throw new Exception("Teacher profile already exists.");

            // 4. Convert user into teacher
            user.Role = "Teacher";
            user.FullName = dto.FullName ?? user.FullName;
            user.DisplayName = dto.DisplayName ?? user.DisplayName;
            await _userRepo.UpdateAsync(user);

            // 5. Create Teacher record
            var teacher = new Learnable.Domain.Entities.Teacher
            {
                ProfileId = Guid.NewGuid(),
                UserId = dto.UserId,
                DateOfBirth = dto.DateOfBirth,
                ContactPhone = dto.ContactPhone,
                Bio = dto.Bio,
                AvatarUrl = dto.AvatarUrl,
                LastUpdatedAt = DateTime.UtcNow
            };

            await _teacherRepo.CreateAsync(teacher);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TeacherUserDto
            {
                User = user.ToDto(_tokenService),
                Teacher = teacher.ToDto()
            };
        }
    }
}

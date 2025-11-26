using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Users.Queries.LoginUser
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;

        public LoginQueryHandler(
            IUserRepository userRepository,
            ITeacherRepository teacherRepository,
            ITokenService tokenService,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _teacherRepository = teacherRepository;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        public async Task<LoginResponseDto> Handle(
            LoginQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Find user by email
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user == null)
                throw new Exception("Invalid email or password.");

            // 2. Verify password
            var passwordOk = _passwordService.VerifyPassword(
                request.Password,
                user.PasswordHash,
                user.PasswordSalt
            );

            if (!passwordOk)
                throw new Exception("Invalid email or password.");

            // 3. Convert to UserDto (includes JWT)
            var userDto = user.ToDto(_tokenService);

            TeacherDto? teacherDto = null;

            // 4. If user is teacher → load teacher profile + classes
            if (user.Role == "Teacher")
            {
                var teacher = await _teacherRepository.GetByUserIdAsync(
                    user.UserId,
                    cancellationToken
                );

                if (teacher != null)
                {
                    // Convert entity → extended DTO (with User + Classes)
                    teacherDto = teacher.ToDto();
                }
            }

            // 5. Final combined response
            return new LoginResponseDto
            {
                User = userDto,
                Teacher = teacherDto   // null for students
            };
        }
    }
}


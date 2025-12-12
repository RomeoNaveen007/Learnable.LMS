using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Services;
using MediatR;

namespace Learnable.Application.Features.Account.Queries.LoginUser
{
    public class LoginQueryHandler(
        IUserRepository userRepository,
        ITeacherRepository teacherRepository,
        IClassStudentRepository classStudentRepo,
        ITokenService tokenService,
        IPasswordService passwordService)
        : IRequestHandler<LoginQuery, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITeacherRepository _teacherRepository = teacherRepository;
        private readonly IClassStudentRepository _classStudentRepo = classStudentRepo;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IPasswordService _passwordService = passwordService;

        public async Task<LoginResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            // 1. Find user by email
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email address.");

            // 2. Verify password
            var passwordOk = _passwordService.VerifyPassword(
                request.Password,
                user.PasswordHash,
                user.PasswordSalt);

            if (!passwordOk)
                throw new UnauthorizedAccessException("Invalid password.");

            TeacherDto? teacherDto = null;
            UserWithClassesDto? studentDto = null;

            // ------------------- ROLE CHECK -------------------
            if (user.Role == "Teacher")
            {
                var teacher = await _teacherRepository.GetByUserIdAsync(user.UserId, cancellationToken);
                teacherDto = teacher?.ToDto();
            }
            else if (user.Role == "Student")
            {
                var classes = await _classStudentRepo.GetClassesForStudentAsync(user.UserId, cancellationToken);
                studentDto = user.ToUserWithClassesDto(classes, _tokenService);
            }

            // 3. Always return base UserDto (no classes)
            var userDto = user.ToDto(_tokenService);

            return new LoginResponseDto
            {
                User = userDto,
                Teacher = teacherDto,
                Student = studentDto,
                
                

            };
        }
    }
}

using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Services;
using MediatR;

namespace Learnable.Application.Features.User.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        // Constructor-ல் ClassStudentRepo தேவையில்லை
        public GetUserByIdQueryHandler(
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            // 1. User டேட்டாவை முழுமையாக (Classes உடன்) எடுக்கிறோம்
            var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            // 2. Classes List-ஐ Role-க்கு ஏற்றவாறு தயார் செய்கிறோம்
            List<ClassDto> classDtos = new();

            // Case A: User ஒரு Teacher
            if (user.Teacher != null && user.Teacher.Classes != null)
            {
                classDtos = user.Teacher.Classes.Select(c => new ClassDto
                {
                    ClassId = c.ClassId,
                    ClassName = c.ClassName,
                    ClassJoinName = c.ClassJoinName,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt
                }).ToList();
            }
            else if (user.Role == "Student" && user.ClassStudents != null)
            {
                classDtos = user.ClassStudents.Select(cs => new ClassDto
                {
                    ClassId = cs.Class.ClassId,
                    ClassName = cs.Class.ClassName,
                    ClassJoinName = cs.Class.ClassJoinName,
                    Description = cs.Class.Description,
                    CreatedAt = cs.Class.CreatedAt
                }).ToList();
            }

            // 3. Final Return
            return new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Username = user.Username,
                DisplayName = user.DisplayName,
                FullName = user.FullName,
                Role = user.Role,
                Classes = classDtos, // இங்கே சரியான List போகும்
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
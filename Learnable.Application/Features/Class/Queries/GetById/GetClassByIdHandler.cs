using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Class.Queries.GetById;
using Learnable.Application.Interfaces.Repositories;
using MediatR;

namespace Learnable.Application.Features.Class.Queries
{

    public class GetClassByIdQueryHandler
        : IRequestHandler<GetClassByIdQuery, ClassResponseDto?>
    {
        private readonly IClassRepository _repo;

        public GetClassByIdQueryHandler(IClassRepository repo)
        {
            _repo = repo;
        }

        public async Task<ClassResponseDto?> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _repo.GetClassWithIncludesAsync(request.ClassId, cancellationToken);

            if (data == null)
                return null;

            return new ClassResponseDto
            {
                ClassId = data.ClassId,
                ClassName = data.ClassName ?? string.Empty,
                ClassJoinName = data.ClassJoinName ?? string.Empty,
                Description = data.Description,
                Status = data.Status,

                Teacher = data.Teacher == null ? null : new ClassTeacherDto
                {
                    ProfileId = data.Teacher.ProfileId,
                    UserId = data.Teacher.UserId ?? Guid.Empty,
                    ContactPhone = data.Teacher.ContactPhone ?? string.Empty,
                    AvatarUrl = data.Teacher.AvatarUrl ?? string.Empty,
                    Bio = data.Teacher.Bio ?? string.Empty,
                    DateOfBirth = data.Teacher.DateOfBirth,

                    Email = data.Teacher.User?.Email ?? string.Empty,
                    FullName = data.Teacher.User?.FullName ?? string.Empty,
                    DisplayName = data.Teacher.User?.DisplayName ?? string.Empty,
                    Username = data.Teacher.User?.Username ?? string.Empty
                },

                Students = data.ClassStudents?
                    .Where(cs => cs.User != null)
                    .Select(cs => new StudentDto
                    {
                        UserId = cs.UserId,
                        FullName = cs.User.FullName ?? string.Empty
                    }).ToList() ?? new List<StudentDto>(),

                Repositories = data.Repositories?
                    .Select(r => new RepositoryDto
                    {
                        RepositoryId = (Guid)r.RepoId,
                        FileName = r.RepoName ?? string.Empty,
                        RepoCertification=r.RepoCertification ?? string.Empty,
                        RepoDescription=r.RepoDescription ?? string.Empty,
                    }).ToList() ?? new List<RepositoryDto>()
            };
        }
    }
}

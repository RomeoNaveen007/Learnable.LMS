using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Dtos
{
    public class ClassDto
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public string ClassJoinName { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? TeacherId { get; set; }
        public string? Status { get; set; }
    }
    public class ClassResponseDto
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string ClassJoinName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Status { get; set; }

        public ClassTeacherDto? Teacher { get; set; }
        public List<StudentDto> Students { get; set; } = new();
        public List<RepositoryDto> Repositories { get; set; } = new();
    }

    public class ClassTeacherDto
    {
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string ContactPhone { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
    public class StudentDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
    public class RepositoryDto
    {
        public Guid RepositoryId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string RepoDescription { get; set; } = string.Empty;
        public string RepoCertification { get; set; } = string.Empty;
    }
}

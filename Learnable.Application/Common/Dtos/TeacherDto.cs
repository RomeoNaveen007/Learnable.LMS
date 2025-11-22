using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Dtos
{
    public class TeacherDto
    {
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? ContactPhone { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        // User-related fields
        public string? DisplayName { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; } = null!;
    }

    // Combined DTO for returning both User + Teacher
    public class TeacherUserDto
    {
        public UserDto User { get; set; } = null!;
        public TeacherDto Teacher { get; set; } = null!;
    }
}

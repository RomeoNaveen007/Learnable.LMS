using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Account.Commands.RegisterTeacher
{
    public class RegisterTeacherDto
    {
        // User fields
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string? FullName { get; set; }

        // Teacher-specific fields
        public DateOnly? DateOfBirth { get; set; }
        public string? ContactPhone { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
    }
}

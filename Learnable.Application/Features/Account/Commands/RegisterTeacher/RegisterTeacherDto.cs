using System;

namespace Learnable.Application.Features.Account.Commands.RegisterTeacher
{
    public class RegisterTeacherDto
    {
        public Guid UserId { get; set; }

        // Optional user update fields
        public string? DisplayName { get; set; }
        public string? FullName { get; set; }

        // Teacher-specific fields
        public DateOnly? DateOfBirth { get; set; }
        public string? ContactPhone { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
    }
}

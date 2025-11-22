using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Teacher.Commands.UpdateTeacher
{
    public class UpdateTeacherDto
    {
        // Teacher fields
        public DateOnly? DateOfBirth { get; set; }
        public string? ContactPhone { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }

        // User fields
        public string? DisplayName { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
    }
}

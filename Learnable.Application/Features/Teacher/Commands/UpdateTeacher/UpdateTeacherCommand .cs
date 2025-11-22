using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Teacher.Commands.UpdateTeacher
{
    public class UpdateTeacherCommand : IRequest<TeacherDto?>
    {
        public Guid ProfileId { get; set; }

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

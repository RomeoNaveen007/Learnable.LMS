using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Dtos
{
    public class GlobalSearchDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? SubTitle { get; set; } // Nullable
    }


    public class SimpleClassDto
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UserDetailsDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? Bio { get; set; }
        public string Role { get; set; } = string.Empty;

        // For student: classes they are enrolled in
        public List<SimpleClassDto> EnrolledClasses { get; set; } = new();

        // For teacher: classes created/by teacher
        public List<SimpleClassDto> CreatedClasses { get; set; } = new();
    }


}

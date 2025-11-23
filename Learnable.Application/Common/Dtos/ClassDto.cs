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
}

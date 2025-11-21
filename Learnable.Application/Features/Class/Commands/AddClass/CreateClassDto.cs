using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Commands.AddClass
{
    public class CreateClassDto
    {
        public string ClassName { get; set; } = null!;
        public string ClassJoinName { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? TeacherId { get; set; }
        public string? Status { get; set; }
    }
}

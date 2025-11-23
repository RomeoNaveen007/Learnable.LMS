using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Commands.UpdateClass
{
    public class UpdateClassDto
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassJoinName { get; set; }
        public string Description { get; set; }
        public Guid TeacherId { get; set; }
        public string Status { get; set; }
    }
}

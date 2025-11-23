using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Commands.UpdateClass
{
    public class UpdateClassCommand : IRequest<Learnable.Application.Common.Dtos.ClassDto?>
    {
        public Guid ClassId { get; set; }  // set in Controller
        public string? ClassName { get; set; }
        public string? ClassJoinName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public Guid? TeacherId { get; set; }
    }
}

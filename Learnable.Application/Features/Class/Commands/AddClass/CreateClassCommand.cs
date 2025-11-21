using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Commands.AddClass
{
    public class CreateClassCommand : IRequest<ClassDto>
    {
        public CreateClassDto ClassDto { get; set; } = null!;
    }
}

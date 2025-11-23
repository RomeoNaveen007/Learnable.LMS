using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Commands.DeleteClass
{
    public record DeleteClassCommand(Guid ClassId) : IRequest<bool>;
}

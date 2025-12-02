using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Marks.Commands.UpdateMark
{
    public record UpdateMarkCommand(MarksDto Mark) : IRequest<MarksDto?>;

}

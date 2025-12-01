using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Marks.Commands.CreateMark
{
   
    public record AddMarkCommand(Guid Examid,Guid ClassId) : IRequest<List<Guid>>
    {
        public Guid ExamId { get; set; }
        public Guid ClassId { get; set; }
    }
}

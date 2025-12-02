using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Marks.Queries.GetMarksByExam
{
    public record GetMarksByExamIdQuery(Guid ExamId) : IRequest<List<MarksDto>>;

}

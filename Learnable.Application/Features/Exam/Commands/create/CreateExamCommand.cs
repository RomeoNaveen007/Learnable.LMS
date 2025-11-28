using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Exam.Commands.Create
{
    public class CreateExamCommand : IRequest<ExamDto>
    {
        public CreateExamDto Exam { get; set; } = null!;
    }
}

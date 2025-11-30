using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.AiServises.Queries.Question.GetQuestions
{
    public class GetQuestionQuery : IRequest<List<ExamQuestionDto>>
    {
        public List<Guid>? Asset_Id { get; set; }
        public int Question_Count { get; set; }

    }
}

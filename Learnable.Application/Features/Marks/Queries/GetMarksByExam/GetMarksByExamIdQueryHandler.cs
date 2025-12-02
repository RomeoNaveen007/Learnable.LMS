using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Marks.Queries.GetMarksByExam
{
    public class GetMarksByExamIdQueryHandler
    : IRequestHandler<GetMarksByExamIdQuery, List<MarksDto>>
    {
        private readonly IMarksRepostiory _repository;

        public GetMarksByExamIdQueryHandler(IMarksRepostiory repository)
        {
            _repository = repository;
        }

        public async Task<List<MarksDto>> Handle(GetMarksByExamIdQuery request, CancellationToken cancellationToken)
        {
            var marks = await _repository.GetMarksByExamIdAsync(request.ExamId);

            return marks.Select(m => new MarksDto
            {
                ExamId = m.ExamId,
                StudentId = m.StudentId,
                Marks = m.Marks,
                ExamStatus = m.ExamStatus,
                StudentsAnswers = m.StudentsAnswers.Select(a => new StudentAnswerDto
                {
                    QuestionId = a.QuestionId,
                    AnswerIndex = a.AnswerIndex,
                    SubmittedAt = a.SubmittedAt
                }).ToList()
            }).ToList();
        }
    }

}

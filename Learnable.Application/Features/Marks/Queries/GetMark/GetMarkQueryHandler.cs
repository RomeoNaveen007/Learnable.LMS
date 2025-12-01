using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Marks.Queries.GetMark
{
    public class GetMarkQueryHandler : IRequestHandler<GetMarkQuery, MarksDto?>
    {
        private readonly IMarksRepostiory _repository;

        public GetMarkQueryHandler(IMarksRepostiory repository)
        {
            _repository = repository;
        }

        public async Task<MarksDto?> Handle(GetMarkQuery request, CancellationToken cancellationToken)
        {
            var mark = await _repository.GetMarkAsync(request.ExamId, request.StudentId);

            if (mark == null) return null;

            return new MarksDto
            {
                ExamId = mark.ExamId,
                StudentId = mark.StudentId,
                Marks = mark.Marks,
                ExamStatus = mark.ExamStatus,
                StudentsAnswers = mark.StudentsAnswers
                    .Select(a => new StudentAnswerDto
                    {
                        QuestionId = a.QuestionId,
                        AnswerIndex = a.AnswerIndex,
                        SubmittedAt = a.SubmittedAt
                    }).ToList()
            };
        }
    }

}

using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Marks.Commands.UpdateMark
{
    public class UpdateMarkCommandHandler
     : IRequestHandler<UpdateMarkCommand, MarksDto?>
    {
        private readonly IMarksRepostiory _repository;

        public UpdateMarkCommandHandler(IMarksRepostiory repository)
        {
            _repository = repository;
        }

        public async Task<MarksDto?> Handle(UpdateMarkCommand request, CancellationToken cancellationToken)
        {
            var markEntity = new Mark
            {
                ExamId = request.Mark.ExamId,
                StudentId = request.Mark.StudentId,
                Marks = request.Mark.Marks,
                ExamStatus = request.Mark.ExamStatus,
                StudentsAnswers = request.Mark.StudentsAnswers.Select(a => new StudentsAnswer
                {
                    QuestionId = a.QuestionId,
                    AnswerIndex = a.AnswerIndex,
                    SubmittedAt = a.SubmittedAt
                }).ToList()
            };

            var updated = await _repository.UpdateMarkAsync(markEntity);

            if (updated == null) return null;

            return request.Mark;
        }
    }

}
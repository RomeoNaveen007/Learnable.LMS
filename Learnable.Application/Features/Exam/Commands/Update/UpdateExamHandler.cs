using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Exam.Commands.Update
{
    public class UpdateExamHandler : IRequestHandler<UpdateExamCommand, ExamDto?>
    {
        private readonly IExamRepository _examRepository;

        public UpdateExamHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ExamDto?> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {

            //var examentity = await _examRepository.GetExamByIdAsync()

            //if (examentity == null)
            //    return null;
            var updatedExam = new Learnable.Domain.Entities.Exam
            {
                ExamId = request.Exam.ExamId,
                RepoId = request.Exam.RepoId,
                Title = request.Exam.Title!,
                Description = request.Exam.Description,
                StartDatetime = request.Exam.StartDatetime,
                EndDatetime = request.Exam.EndDatetime,
                Duration = request.Exam.Duration
            };

            var updatedQuestions = request.Exam.Questions.Select(q => new ExamQuestion
            {
                QuestionId = Guid.NewGuid(),
                Question = q.Question,
                Answers = q.Answers,
                CorrectAnswerIndex = q.CorrectAnswerIndex
            }).ToList();

            var result = await _examRepository.UpdateExamAsync(updatedExam, updatedQuestions);

            return result?.ToDto();
        }
    }
}

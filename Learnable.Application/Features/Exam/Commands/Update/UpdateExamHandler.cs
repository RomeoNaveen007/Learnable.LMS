using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Exceptions;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            // 1️⃣ check if exam already exists
            var existingExam = await _examRepository.GetExamByIdAsync(request.Exam.ExamId);
            if (existingExam == null)
                throw new KeyNotFoundException("Exam not found.");

            // 2️⃣ validate exam dates (extra business validation)
            if (request.Exam.StartDatetime >= request.Exam.EndDatetime)
                throw new BadRequestException("StartDatetime must be before EndDatetime.");

            // Create updated exam entity
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

            // Map updated questions and assign ExamId + navigation property
            var updatedQuestions = request.Exam.Questions.Select(q => new ExamQuestion
            {
                QuestionId = Guid.NewGuid(),
                Question = q.Question,
                Answers = q.Answers,
                CorrectAnswerIndex = q.CorrectAnswerIndex,
                ExamId = request.Exam.ExamId,
                Exam = updatedExam // Important
            }).ToList();

            // Call repository to update
            var result = await _examRepository.UpdateExamAsync(updatedExam, updatedQuestions);

            if (result == null)
                throw new Exception("Exam update failed due to a server error.");

            return result?.ToDto();
        }
    }
}

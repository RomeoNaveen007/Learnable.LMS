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

namespace Learnable.Application.Features.Exam.Commands.Create
{
    public class CreateExamHandler : IRequestHandler<CreateExamCommand, ExamDto>
    {
        private readonly IExamRepository _examRepository;

        public CreateExamHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ExamDto> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            var exam = new Learnable.Domain.Entities.Exam
            {
                ExamId = Guid.NewGuid(),
                RepoId = request.Exam.RepoId,
                Title = request.Exam.Title,
                Description = request.Exam.Description,
                StartDatetime = request.Exam.StartDatetime,
                EndDatetime = request.Exam.EndDatetime,
                Duration = request.Exam.Duration,
            };

            var questions = request.Exam.Question.Select(q => new ExamQuestion
            {
                QuestionId = Guid.NewGuid(),
                Question = q.Question,
                Answers = q.Answers,
                CorrectAnswerIndex = q.CorrectAnswerIndex
            }).ToList();

            var savedExam = await _examRepository.AddExamAsync(exam, questions);

            return savedExam.ToDto();
        }
    }
}


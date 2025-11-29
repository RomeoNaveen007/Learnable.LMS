using Learnable.Application.Common.Dtos;
using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Extensions
{
    public static class ExamMappingExtensions
    {
        public static ExamDto ToDto(this Exam exam)
        {
            return new ExamDto
            {
                ExamId = exam.ExamId,
                RepoId = exam.RepoId,
                Title = exam.Title,
                Description = exam.Description,
                StartDatetime = exam.StartDatetime,
                EndDatetime = exam.EndDatetime,
                Duration = exam.Duration,
                Questions = exam.Questions.Select(q => new ExamQuestionDto
                {
                    QuestionId = q.QuestionId,
                    ExamId = exam.ExamId,
                    Question = q.Question,
                    Answers = q.Answers,
                    CorrectAnswerIndex = q.CorrectAnswerIndex
                }).ToList()
            };
        }
    }
}

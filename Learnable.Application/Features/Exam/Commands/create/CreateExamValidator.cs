using FluentValidation;
using Learnable.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Exam.Commands.Create
{
    public class CreateExamValidator : AbstractValidator<CreateExamDto>
    {
        public CreateExamValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Exam Title is required")
                .MaximumLength(100);

            RuleForEach(x => x.Question).ChildRules(q =>
            {
                q.RuleFor(x => x.Question)
                    .NotEmpty().WithMessage("Question is required");

                q.RuleFor(x => x.Answers.Count)
                    .GreaterThan(1).WithMessage("Minimum 2 answers required");

                q.RuleFor(x => x.CorrectAnswerIndex)
                    .GreaterThanOrEqualTo(0);
            });
        }
    }
}

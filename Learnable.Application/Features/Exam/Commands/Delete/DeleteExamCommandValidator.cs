using FluentValidation;
using Learnable.Application.Features.Class.Commands.DeleteClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Exam.Commands.Delete
{
    public class DeleteExamCommandValidator : AbstractValidator<DeleteExamCommand>
    {
        public DeleteExamCommandValidator()
        {
            RuleFor(x => x.ExamId)
                .NotEmpty()
                .WithMessage("Exam is required.");
        }
    }


}

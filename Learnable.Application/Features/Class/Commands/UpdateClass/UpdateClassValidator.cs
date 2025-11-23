using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Commands.UpdateClass
{
    public class UpdateClassCommandValidator : AbstractValidator<UpdateClassCommand>
    {
        public UpdateClassCommandValidator()
        {
            RuleFor(x => x.ClassId)
                .NotEmpty()
                .WithMessage("ClassId is required");

            RuleFor(x => x.ClassName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.ClassName));

            RuleFor(x => x.ClassJoinName)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.ClassJoinName));

            RuleFor(x => x.Description)
                .MaximumLength(200)
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Status)
                .MaximumLength(20)
                .When(x => !string.IsNullOrEmpty(x.Status));
        }
    }
}

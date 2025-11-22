using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Commands.AddClass
{
    public class CreateClassValidator : AbstractValidator<CreateClassDto>
    {
        public CreateClassValidator()
        {
            RuleFor(x => x.ClassName)
                .NotEmpty().WithMessage("Class name is required")
                .MaximumLength(100);

            RuleFor(x => x.ClassJoinName)
                .NotEmpty().WithMessage("Class join name is required")
                .MaximumLength(50);

            RuleFor(x => x.Description)
                .MaximumLength(200);

            RuleFor(x => x.Status)
                .MaximumLength(20);
        }
    }
}
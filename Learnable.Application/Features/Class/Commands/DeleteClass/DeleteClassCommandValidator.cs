using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Commands.DeleteClass
{
    public class DeleteClassCommandValidator : AbstractValidator<DeleteClassCommand>
    {
        public DeleteClassCommandValidator()
        {
            RuleFor(x => x.ClassId)
                .NotEmpty()
                .WithMessage("UserId is required.");
        }
    }
}

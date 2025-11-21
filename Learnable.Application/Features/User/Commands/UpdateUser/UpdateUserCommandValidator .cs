using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(x => x.DisplayName)
                .MaximumLength(50)
                .WithMessage("DisplayName cannot exceed 50 characters.")
                .When(x => x.DisplayName != null);

            RuleFor(x => x.FullName)
                .MaximumLength(100)
                .WithMessage("FullName cannot exceed 100 characters.")
                .When(x => x.FullName != null);

            RuleFor(x => x.Username)
                .MaximumLength(50)
                .WithMessage("Username cannot exceed 50 characters.")
                .When(x => x.Username != null);
        }
    }
}

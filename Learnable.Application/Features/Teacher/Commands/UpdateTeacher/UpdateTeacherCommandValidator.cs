using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Teacher.Commands.UpdateTeacher
{
    public class UpdateTeacherCommandValidator : AbstractValidator<UpdateTeacherCommand>
    {
        public UpdateTeacherCommandValidator()
        {
            RuleFor(x => x.ProfileId)
                .NotEmpty()
                .WithMessage("ProfileId is required.");

            RuleFor(x => x.ContactPhone)
                .MaximumLength(20)
                .When(x => x.ContactPhone != null);

            RuleFor(x => x.Bio)
                .MaximumLength(500)
                .When(x => x.Bio != null);

            RuleFor(x => x.AvatarUrl)
                .MaximumLength(255)
                .When(x => x.AvatarUrl != null);

            RuleFor(x => x.DisplayName)
                .MaximumLength(100)
                .When(x => x.DisplayName != null);

            RuleFor(x => x.FullName)
                .MaximumLength(100)
                .When(x => x.FullName != null);

            RuleFor(x => x.Username)
                .MaximumLength(50)
                .When(x => x.Username != null);
        }
    }
}

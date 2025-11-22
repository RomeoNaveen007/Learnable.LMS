using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Account.Commands.RegisterTeacher
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterTeacherCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Dto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Dto.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(x => x.Dto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.OtpCode)
                .NotEmpty().WithMessage("OTP code is required.")
                .Length(4).WithMessage("OTP code must be 4 digits.");

            RuleFor(x => x.Dto.DisplayName)
                .MaximumLength(50).WithMessage("DisplayName cannot exceed 50 characters.")
                .When(x => x.Dto.DisplayName != null);

            RuleFor(x => x.Dto.FullName)
                .MaximumLength(100).WithMessage("FullName cannot exceed 100 characters.")
                .When(x => x.Dto.FullName != null);

            RuleFor(x => x.Dto.ContactPhone)
                .MaximumLength(20).WithMessage("ContactPhone cannot exceed 20 characters.")
                .When(x => x.Dto.ContactPhone != null);
        }
    }
}

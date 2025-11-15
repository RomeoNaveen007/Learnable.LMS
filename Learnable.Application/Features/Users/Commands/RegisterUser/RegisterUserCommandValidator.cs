using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Dto.Username).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Dto.Password).NotEmpty().MinimumLength(6);

            RuleFor(x => x.Dto.DisplayName)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Dto.DisplayName));

            RuleFor(x => x.Dto.FullName)    
                .MaximumLength(100);

            RuleFor(x => x.OtpCode)
                .NotEmpty()
                .Matches(@"^\d{4}$")
                .WithMessage("OTP must be a 4-digit number");
        }
    }
}

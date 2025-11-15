using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Verifications.Commands.SendOtp
{
    public class SendOtpValidator : AbstractValidator<SendOtpCommand>
    {
        public SendOtpValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Commands.Sent
{
    public class CreateClassJoinRequestValidator : AbstractValidator<CreateClassJoinRequestDto>
    {
        public CreateClassJoinRequestValidator()
        {
            RuleFor(x => x.SenderId)
                .NotEmpty().WithMessage("SenderId is required");

            RuleFor(x => x.ReceiverId)
                .NotEmpty().WithMessage("ReceiverId is required");

            RuleFor(x => x.ClassId)
                .NotEmpty().WithMessage("ClassId is required");
        }
    }
}

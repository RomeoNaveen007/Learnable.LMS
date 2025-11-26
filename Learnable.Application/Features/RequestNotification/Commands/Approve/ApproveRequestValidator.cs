using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Commands.Approve
{
    public class ApproveRequestValidator : AbstractValidator<ApproveRequestDto>
    {
        public ApproveRequestValidator()
        {
            RuleFor(x => x.NotificationId)
                .NotEmpty().WithMessage("NotificationId is required");
        }
    }
}

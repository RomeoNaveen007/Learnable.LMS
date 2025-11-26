using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Commands.Approve
{
    public class ApproveRequestCommand : IRequest<ApproveRequestDto>
    {
        public ApproveRequestDto RequestDto { get; set; } = null!;
    }
}


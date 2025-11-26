using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Commands.Sent
{
    public class CreateClassJoinRequestCommand : IRequest<CreateClassJoinRequestDto>
    {
        public CreateClassJoinRequestDto RequestDto { get; set; } = null!;
    }
}

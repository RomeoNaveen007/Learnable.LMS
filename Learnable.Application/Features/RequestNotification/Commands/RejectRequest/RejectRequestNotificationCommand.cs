using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Commands.RejectRequest
{
    public class RejectRequestNotificationCommand : IRequest<bool>
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid ClassId { get; set; }
    }

}

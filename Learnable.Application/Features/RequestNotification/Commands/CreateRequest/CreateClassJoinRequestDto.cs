using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Commands.Sent
{
    public class CreateClassJoinRequestDto
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid ClassId { get; set; }
        public string? NotificationStatus { get; set; } = "Pending";
    }
}

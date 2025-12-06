using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Queries.Recevied
{
    public class RequestNotificationDto
    {
        public Guid NotificationId { get; set; }
        public Guid? SenderId { get; set; }
        public string? SenderName { get; set; }
        public Guid? ClassId { get; set; }
        public string? ClassName { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }

        public Guid? ReceiverId { get; set; }
        public string? ReceiverName { get; set; }
    }
}

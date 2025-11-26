using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Commands.Approve
{
    public class ApproveRequestDto
    {
        public Guid NotificationId { get; set; }
        public string Status { get; set; } = "Approved";
    }
}

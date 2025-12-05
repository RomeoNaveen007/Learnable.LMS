using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Queries.Recevied
{
    public class GetReceivedRequestsQueryHandler
     : IRequestHandler<GetReceivedRequestsQuery, IEnumerable<RequestNotificationDto>>
    {
        private readonly IRequestNotificationRepository _repo;

        public GetReceivedRequestsQueryHandler(IRequestNotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<RequestNotificationDto>> Handle(GetReceivedRequestsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _repo.GetPendingRequestsByReceiverIdAsync(request.UserId, cancellationToken);

            return notifications.Select(n => new RequestNotificationDto
            {
                NotificationId = n.NotificationId,
                SenderId = n.SenderId,
                SenderName = n.Sender?.FullName,
                ClassId = n.ClassId,
                ClassName = n.Class?.ClassName,
                Status = n.NotificationStatus,
                CreatedAt = n.CreatedAt
            });
        }
    }
}

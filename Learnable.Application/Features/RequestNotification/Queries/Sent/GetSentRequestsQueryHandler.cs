using Learnable.Application.Features.RequestNotification.Queries.Recevied;
using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Queries.Sent
{
    public class GetSentRequestsQueryHandler
     : IRequestHandler<GetSentRequestsQuery, IEnumerable<RequestNotificationDto>>
    {
        private readonly IRequestNotificationRepository _repo;

        public GetSentRequestsQueryHandler(IRequestNotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<RequestNotificationDto>> Handle(GetSentRequestsQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _repo.GetSentRequestsBySenderIdAsync(request.UserId, cancellationToken);

            return notifications.Select(n => new RequestNotificationDto
            {
                NotificationId = n.NotificationId,
                ReceiverId = n.ReceiverId,
                ReceiverName = n.Receiver?.FullName,
                ClassId = n.ClassId,
                ClassName = n.Class?.ClassName,
                Status = n.NotificationStatus,
                CreatedAt = n.CreatedAt
            });
        }
    }
}

using Learnable.Application.Features.RequestNotification.Queries.Recevied;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Queries.Sent
{
    public record GetSentRequestsQuery(Guid UserId)
       : IRequest<IEnumerable<RequestNotificationDto>>;
}

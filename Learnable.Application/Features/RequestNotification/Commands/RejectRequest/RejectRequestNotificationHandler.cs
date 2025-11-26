using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Commands.RejectRequest
{
    public class RejectRequestNotificationHandler : IRequestHandler<RejectRequestNotificationCommand, bool>
    {
        private readonly IRequestNotificationRepository _requestRepo;
        private readonly IUnitOfWork _unitOfWork;

        public RejectRequestNotificationHandler(
            IRequestNotificationRepository requestRepo,
            IUnitOfWork unitOfWork)
        {
            _requestRepo = requestRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RejectRequestNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = await _requestRepo.GetByIdAsync(n =>
                n.SenderId == request.SenderId &&
                n.ReceiverId == request.ReceiverId &&
                n.ClassId == request.ClassId
            );

            if (notification == null) return false;

            notification.NotificationStatus = "Rejected";

            // DB commit using UnitOfWork
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

}

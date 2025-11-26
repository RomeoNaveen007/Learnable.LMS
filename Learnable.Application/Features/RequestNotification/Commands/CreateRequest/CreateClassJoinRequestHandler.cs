using Learnable.Application.Features.RequestNotification.Commands.Sent;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Commands
{
    public class CreateClassJoinRequestHandler : IRequestHandler<CreateClassJoinRequestCommand, CreateClassJoinRequestDto>
    {
        private readonly IRequestNotificationRepository _requestRepository;
        private readonly IUnitOfWork _unitOfWork; // <- add this

        public CreateClassJoinRequestHandler(
            IRequestNotificationRepository requestRepository,
            IUnitOfWork unitOfWork)  // inject unit of work
        {
            _requestRepository = requestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateClassJoinRequestDto> Handle(CreateClassJoinRequestCommand request, CancellationToken cancellationToken)
        {
            var newRequest = new Learnable.Domain.Entities.RequestNotification
            {
                NotificationId = Guid.NewGuid(),
                SenderId = request.RequestDto.SenderId,
                ReceiverId = request.RequestDto.ReceiverId,
                ClassId = request.RequestDto.ClassId,
                NotificationStatus = request.RequestDto.NotificationStatus,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _requestRepository.CreateAsync(newRequest);

            // --- IMPORTANT: Save changes to database ---
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateClassJoinRequestDto
            {
                SenderId = created.SenderId!.Value,
                ReceiverId = created.ReceiverId!.Value,
                ClassId = created.ClassId!.Value,
                NotificationStatus = created.NotificationStatus
            };
        }
    }

}

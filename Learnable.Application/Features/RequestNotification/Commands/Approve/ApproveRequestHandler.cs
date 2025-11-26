using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.RequestNotification.Commands.Approve
{
    public class ApproveRequestHandler : IRequestHandler<ApproveRequestCommand, ApproveRequestDto>
    {
        private readonly IRequestNotificationRepository _requestRepository;
        private readonly IClassStudentRepository _classStudentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApproveRequestHandler(
            IRequestNotificationRepository requestRepository,
            IClassStudentRepository classStudentRepository,
            IUnitOfWork unitOfWork)
        {
            _requestRepository = requestRepository;
            _classStudentRepository = classStudentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApproveRequestDto> Handle(ApproveRequestCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ Get existing request
            var existing = await _requestRepository.GetByIdAsync(
                x => x.NotificationId == request.RequestDto.NotificationId
            );

            if (existing == null)
                throw new Exception("Request not found");

            // 2️⃣ Approve the request
            existing.NotificationStatus = "Approved";
            await _requestRepository.UpdateAsync(existing);

            // 3️⃣ Check if student already joined the class
            var alreadyJoined = await _classStudentRepository
                .IsStudentAlreadyJoinedAsync(existing.ClassId!.Value, existing.SenderId!.Value, cancellationToken);

            // 4️⃣ Add student to class if not already joined
            if (!alreadyJoined)
            {
                var cs = new ClassStudent
                {
                    ClassId = existing.ClassId!.Value,
                    UserId = existing.SenderId!.Value,
                    JoinDate = DateTime.UtcNow,
                    StudentStatus = "Active"
                };
                await _classStudentRepository.CreateAsync(cs);
            }

            // 5️⃣ Save all changes in a single transaction
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 6️⃣ Return response
            return new ApproveRequestDto
            {
                NotificationId = existing.NotificationId,
                Status = existing.NotificationStatus
            };
        }
    }
}


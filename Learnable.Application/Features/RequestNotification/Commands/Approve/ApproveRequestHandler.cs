using Learnable.Application.Common.Exceptions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Domain.Entities;
using MediatR;

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
            // 1️⃣ Validate NotificationId
            if (request.RequestDto.NotificationId == Guid.Empty)
                throw new BadRequestException("NotificationId is required.");

            // 2️⃣ Get existing request
            var existing = await _requestRepository.GetByIdAsync(
                x => x.NotificationId == request.RequestDto.NotificationId
            );

            if (existing == null)
                throw new KeyNotFoundException("Request not found."); // 404 handled by middleware

            // 3️⃣ Approve the request
            existing.NotificationStatus = "Approved";
            await _requestRepository.UpdateAsync(existing);

            // 4️⃣ Check if already joined
            if (existing.ClassId == null || existing.SenderId == null)
                throw new BadRequestException("Invalid class or student information.");

            var alreadyJoined = await _classStudentRepository
                .IsStudentAlreadyJoinedAsync(existing.ClassId.Value, existing.SenderId.Value, cancellationToken);

            // 5️⃣ Add student ONLY if not already joined
            if (!alreadyJoined)
            {
                var cs = new ClassStudent
                {
                    ClassId = existing.ClassId.Value,
                    UserId = existing.SenderId.Value,
                    JoinDate = DateTime.UtcNow,
                    StudentStatus = "Active"
                };

                await _classStudentRepository.CreateAsync(cs);
            }

            // 6️⃣ Save all changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 7️⃣ Return response DTO
            return new ApproveRequestDto
            {
                NotificationId = existing.NotificationId,
                Status = existing.NotificationStatus
            };
        }
    }
}

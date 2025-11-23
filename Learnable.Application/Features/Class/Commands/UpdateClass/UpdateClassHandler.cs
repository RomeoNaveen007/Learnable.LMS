using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Commands.UpdateClass
{
    public class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, ClassDto?>
    {
        private readonly IGenericRepository<Learnable.Domain.Entities.Class> _classRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateClassCommandHandler(
            IGenericRepository<Learnable.Domain.Entities.Class> classRepository,
            IUnitOfWork unitOfWork)
        {
            _classRepository = classRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ClassDto?> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
        {
            var classEntity = await _classRepository.GetByIdAsync(
                x => x.ClassId == request.ClassId
            );

            if (classEntity == null)
                return null;

            // Update only provided fields
            if (!string.IsNullOrEmpty(request.ClassName))
                classEntity.ClassName = request.ClassName;

            if (!string.IsNullOrEmpty(request.ClassJoinName))
                classEntity.ClassJoinName = request.ClassJoinName;

            if (!string.IsNullOrEmpty(request.Description))
                classEntity.Description = request.Description;

            if (!string.IsNullOrEmpty(request.Status))
                classEntity.Status = request.Status;

            if (request.TeacherId.HasValue)
                classEntity.TeacherId = request.TeacherId;


            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ClassDto
            {
                ClassId = classEntity.ClassId,
                ClassName = classEntity.ClassName,
                ClassJoinName = classEntity.ClassJoinName,
                Description = classEntity.Description,
                TeacherId = classEntity.TeacherId,
                Status = classEntity.Status
            };
        }
    }
}

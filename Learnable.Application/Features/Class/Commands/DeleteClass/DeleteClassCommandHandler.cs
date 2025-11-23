using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Commands.DeleteClass
{

    public class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, bool>
    {
        private readonly IClassRepository _classRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteClassCommandHandler(IClassRepository classRepository, IUnitOfWork unitOfWork)
        {
            _classRepository = classRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
        {
            var classEntity = await _classRepository.GetByIdAsync(c => c.ClassId == request.ClassId);

            if (classEntity == null)
                return false;

            await _classRepository.DeleteAsync(classEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

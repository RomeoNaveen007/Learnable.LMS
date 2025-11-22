using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Teacher.Commands.DeleteTeacher
{
    public class DeleteTeacherCommandHandler : IRequestHandler<DeleteTeacherCommand, bool>
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTeacherCommandHandler(
            ITeacherRepository teacherRepository,
            IUnitOfWork unitOfWork)
        {
            _teacherRepository = teacherRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
        {
            // Get teacher with user reference
            var teacher = await _teacherRepository.GetTeacherWithUserAsync(request.UserId, cancellationToken);

            if (teacher == null)
                return false;

            if (teacher.UserId != request.UserId)
                return false;

            await _teacherRepository.DeleteAsync(teacher);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return true;
        }
    }
}

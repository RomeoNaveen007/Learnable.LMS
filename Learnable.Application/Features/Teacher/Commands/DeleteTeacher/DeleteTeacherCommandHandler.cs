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
    public class DeleteTeacherCommandHandler : IRequestHandler<DeleteTeacherByUserIdCommand, bool>
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTeacherCommandHandler(
            ITeacherRepository teacherRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _teacherRepository = teacherRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(DeleteTeacherByUserIdCommand request, CancellationToken cancellationToken)
        {
            // Get the teacher by UserId
            var teacher = await _teacherRepository.DeleteTeacherByUserIdAsync(request.UserId, cancellationToken);
            if (teacher == null)
                return false;

            // Update the User role to "Student"
            if (teacher.User != null)
            {
                teacher.User.Role = "Student"; // Assuming Role is a string
                await _userRepository.UpdateAsync(teacher.User);
            }

            // Delete Teacher entity
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

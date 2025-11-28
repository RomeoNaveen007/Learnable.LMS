using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(IUserRepository userRepository, IClassStudentRepository classStudentRepo, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand,bool>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IClassStudentRepository _classStudentRepo = classStudentRepo;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);

            if (user == null)
                return false;

            // 1️⃣ Block Teacher deletion
            if (user.Role == "Teacher")
                throw new InvalidOperationException("Teachers cannot be deleted.");

            // If student → delete class enrollments
            if (user.Role == "Student")
            {
                var all = await _classStudentRepo.GetAllAsync();  // await first!

                var classStudents = all
                    .Where(x => x.UserId == request.UserId)
                    .ToList();

                if (classStudents.Count != 0)
                {
                    foreach (var cs in classStudents)
                        await _classStudentRepo.DeleteAsync(cs);
                }
            }

            await _userRepository.DeleteAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

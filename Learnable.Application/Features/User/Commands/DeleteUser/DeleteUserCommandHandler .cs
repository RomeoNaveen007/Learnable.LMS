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
    public class DeleteUserCommandHandler: IRequestHandler<DeleteUserCommand,bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);

            if (user == null)
                return false;

            await _userRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

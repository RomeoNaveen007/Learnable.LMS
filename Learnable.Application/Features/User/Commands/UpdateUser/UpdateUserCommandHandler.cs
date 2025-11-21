using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
           
            if (user == null)
                return null;

            // Update only non-null fields
            if (request.DisplayName is not null)
                user.DisplayName = request.DisplayName;

            if (request.FullName is not null)
                user.FullName = request.FullName;

            if (request.Username is not null)
                user.Username = request.Username;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UserDto
            {
                UserId = user.UserId,
                DisplayName = user.DisplayName,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}

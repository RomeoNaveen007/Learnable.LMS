using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.User.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery,UserDto?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IClassStudentRepository _classStudentRepo;
        private readonly ITokenService _tokenService;

        public GetUserByIdQueryHandler(IUserRepository userRepository,IClassStudentRepository classStudentRepo, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _classStudentRepo = classStudentRepo;
            _tokenService = tokenService;
        }
        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                return null;

            var classes = await _classStudentRepo.GetClassesForStudentAsync(user.UserId, cancellationToken);

            if (!classes.Any()) return null;

            return user.ToUserWithClassesDto(classes, _tokenService);
        }

    }
}

using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Services;
using Learnable.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.User.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler
    : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public GetAllUsersQueryHandler(IUserRepository userRepository,ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(user => user.ToDto(_tokenService));
        }
    }
 }
 

using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.User.Queries.GetAllUsers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.User.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto?>;
}

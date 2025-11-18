using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Users.Queries.LoginUser
{
    public record LoginQuery(string Email, string Password) : IRequest<UserDto>;

}


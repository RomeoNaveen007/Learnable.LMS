using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.User.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid UserId) : IRequest<bool>;
}

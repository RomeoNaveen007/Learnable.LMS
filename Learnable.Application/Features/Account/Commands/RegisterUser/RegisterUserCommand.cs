using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(RegisterUserDto Dto, string OtpCode) : IRequest<UserDto>;
}

using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Account.Commands.RegisterUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Account.Commands.RegisterUser
{
    public record RegisterUserCommand(RegisterUserDto Dto, string OtpCode) : IRequest<UserDto>;
}

using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<UserDto?>
    {
        public Guid UserId { get; set; }   
        public string? DisplayName { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
    }
}

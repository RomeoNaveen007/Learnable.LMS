using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserDto
    {
        public string? DisplayName { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
    }
}

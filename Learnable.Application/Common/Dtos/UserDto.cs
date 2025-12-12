using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string? FullName { get; set; }
        public string Role { get; set; } = null!;
        public List<ClassDto> Classes { get; set; } = [];

        public string Token { get; set; } = null!;
    }


}
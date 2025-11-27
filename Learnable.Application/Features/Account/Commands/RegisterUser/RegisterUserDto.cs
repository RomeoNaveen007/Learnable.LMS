using Learnable.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Account.Commands.RegisterUser
{
    public class RegisterUserDto
    {

        // Required fields for registering a user
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        
        // using this to get in profile updates 
        public string? DisplayName { get; set; }
        public string? FullName { get; set; }

    }
}

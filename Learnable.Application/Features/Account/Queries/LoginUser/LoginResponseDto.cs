using Learnable.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Account.Queries.LoginUser
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; } = null!;
        public TeacherDto? Teacher { get; set; }
        public UserWithClassesDto? Student { get; set; } // Only for Students

    }
}

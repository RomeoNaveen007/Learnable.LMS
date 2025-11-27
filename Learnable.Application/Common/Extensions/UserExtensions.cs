using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Services;
using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Extensions
{
    public static class UserExtensions
    {
        public static UserDto ToDto(this User user, ITokenService tokenService)
        {
            return new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Username = user.Username,
                DisplayName = user.DisplayName,
                FullName = user.FullName,
                Role = user.Role,
                Token = tokenService.CreateToken(user),
            };
        }

        public static UserWithClassesDto ToUserWithClassesDto(
            this User user,
            IEnumerable<Class> classes,
            ITokenService tokenService)
        {
            return new UserWithClassesDto
            {
                UserId = user.UserId,
                Email = user.Email,
                Username = user.Username,
                DisplayName = user.DisplayName,
                FullName = user.FullName,
                Role = user.Role,
                Token = tokenService.CreateToken(user),

                Classes = [.. classes.Select(c => c.ToDto())]
            };
        }
    }
}

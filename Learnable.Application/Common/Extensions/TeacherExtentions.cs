using Learnable.Application.Common.Dtos;
using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Extensions
{
    public static class TeacherExtentions
    {
        public static TeacherDto ToDto(this Teacher teacher)
        {
            return new TeacherDto
            {
                ProfileId = teacher.ProfileId,
                UserId = teacher.UserId ?? Guid.Empty,
                DateOfBirth = teacher.DateOfBirth,
                ContactPhone = teacher.ContactPhone,
                Bio = teacher.Bio,
                AvatarUrl = teacher.AvatarUrl,
                LastUpdatedAt = teacher.LastUpdatedAt,

                // Map user fields if navigation property is loaded
                DisplayName = teacher.User?.DisplayName,
                FullName = teacher.User?.FullName,
                Username = teacher.User?.Username,
                Email = teacher.User?.Email ?? string.Empty
            };
        }
    }
}

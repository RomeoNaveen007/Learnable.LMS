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
    public static class ClassExtensions
    {
        public static ClassDto ToDto(this Class entity)
        {
            return new ClassDto
            {
                ClassId = entity.ClassId,
                ClassName = entity.ClassName,
                ClassJoinName = entity.ClassJoinName,
                Description = entity.Description,
                TeacherId = entity.TeacherId,
                Status = entity.Status,
            };
        }
    }
}

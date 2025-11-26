using Learnable.Application.Common.Dtos;
using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Extensions
{
    public static class RepositoryExtensions
    {
        public static RepositoryDtos ToDto(this Repository entity)
        {
            return new RepositoryDtos
            {
                RepoId = entity.RepoId,
                ClassId = entity.ClassId,
                RepoName = entity.RepoName,
                RepoDescription = entity.RepoDescription,
                RepoCertification = entity.RepoCertification,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
            };
        }
    }
}

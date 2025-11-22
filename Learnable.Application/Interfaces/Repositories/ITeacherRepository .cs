using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Repositories
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        Task<Teacher?> GetTeacherWithUserAsync(Guid profileId, CancellationToken cancellationToken);

    }
}

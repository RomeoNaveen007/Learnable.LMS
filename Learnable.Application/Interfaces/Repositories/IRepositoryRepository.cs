using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Repositories
{
    public interface IRepositoryRepository : IGenericRepository<Repository>
    {
        Task<Repository?> GetRepositoryWithIncludesAsync(Guid repoId, CancellationToken cancellationToken);
        Task<IEnumerable<Repository>> GetAllRepositoriesWithIncludesAsync(CancellationToken cancellationToken);
    }
}

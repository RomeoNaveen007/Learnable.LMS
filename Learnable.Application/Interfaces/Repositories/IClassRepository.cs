using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Repositories
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        Task<IEnumerable<Class>> GetAllClassesWithIncludesAsync(CancellationToken cancellationToken);
        Task<Class?> GetClassWithIncludesAsync(Guid classId, CancellationToken cancellationToken);
    }
}

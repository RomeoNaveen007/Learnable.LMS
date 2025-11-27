using Learnable.Application.Interfaces.Repositories;
using Learnable.Domain.Entities;
using Learnable.Infrastructure.Implementations.Repositories.Generic;
using Learnable.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Infrastructure.Implementations.Repositories
{
    public class RepositoryRepository : GenericRepository<Repository>, IRepositoryRepository
    {
        private readonly ApplicationDbContext _context;

        public RepositoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Repository?> GetRepositoryWithIncludesAsync(Guid repoId, CancellationToken cancellationToken)
        {
            return await _context.Repositories
                .Include(r => r.Assets) // Include related Assets entity
                .Include(r => r.Exams) // Include related Exams entity
                .FirstOrDefaultAsync(r => r.RepoId == repoId, cancellationToken);
        }

        public async Task<IEnumerable<Repository>> GetAllRepositoriesWithIncludesAsync(CancellationToken cancellationToken)
        {
            return await _context.Repositories
                .Include(r => r.Assets) // Include related Assets entity
                .ToListAsync(cancellationToken);
        }
    }
}

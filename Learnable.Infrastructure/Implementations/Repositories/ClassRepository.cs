using Learnable.Application.Interfaces.Repositories;
using Learnable.Infrastructure.Persistence.Data;
using Learnable.Infrastructure.Implementations.Repositories.Generic;
using Learnable.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Infrastructure.Implementations.Repositories
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        private readonly ApplicationDbContext _context;

        public ClassRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Class?> GetClassWithIncludesAsync(Guid classId, CancellationToken cancellationToken)
        {
            return await _context.Classes
                .Include(x => x.Teacher)
                .Include(x => x.ClassStudents)
                    .ThenInclude(cs => cs.Student)
                .Include(x => x.Repositories)
                .FirstOrDefaultAsync(x => x.ClassId == classId, cancellationToken);
        }

        public async Task<IEnumerable<Class>> GetAllClassesWithIncludesAsync(CancellationToken cancellationToken)
        {
            return await _context.Classes
                .Include(x => x.Teacher)
                .Include(x => x.ClassStudents)
                    .ThenInclude(cs => cs.Student)
                .Include(x => x.Repositories)
                .ToListAsync(cancellationToken);
        }
    }
}

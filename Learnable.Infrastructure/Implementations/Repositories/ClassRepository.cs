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

        public async Task<Class?> GetByJoinNameAsync(string joinName)
        {
            return await _context.Classes
                .FirstOrDefaultAsync(x => x.ClassJoinName == joinName);
        }
    }
}

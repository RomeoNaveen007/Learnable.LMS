using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Infrastructure.Implementations.Repositories.Generic
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext context = context;

        public async Task<T> CreateAsync(T entity)
        {
            var result = await context.Set<T>().AddAsync(entity);
            return result.Entity;
        }

        public async Task DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            await Task.CompletedTask;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> condition)
        {
            return await context.Set<T>().FirstOrDefaultAsync(condition);
        }
    }
}

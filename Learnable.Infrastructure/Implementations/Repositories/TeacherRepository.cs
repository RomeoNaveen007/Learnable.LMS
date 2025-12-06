 using Learnable.Application.Interfaces.Repositories;
using Learnable.Domain.Entities;
using Learnable.Infrastructure.Implementations.Repositories.Generic;
using Learnable.Infrastructure.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Infrastructure.Implementations.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        private readonly ApplicationDbContext context;

        public TeacherRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Teacher?> GetTeacherWithUserAndClassesAsync(Guid profileId, CancellationToken cancellationToken)
        {
            return await context.Teachers
                .Include(t => t.User)
                .Include(t => t.Classes)
                .FirstOrDefaultAsync(t => t.ProfileId == profileId);
        }

        public async Task<List<Teacher>> GetAllTeachersWithUserAndClassesAsync()
        {
            return await context.Teachers
                .Include(t => t.User)
                .Include(t => t.Classes)
                .ToListAsync();
        }

        public async Task<Teacher?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await context.Teachers
                .Include(t => t.Classes)
                .FirstOrDefaultAsync(t => t.UserId == userId, cancellationToken);
        }

    }
}

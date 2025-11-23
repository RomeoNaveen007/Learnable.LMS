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
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        private readonly ApplicationDbContext context;

        public TeacherRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        
        public Task<Teacher?> GetTeacherWithUserAsync(Guid userId, CancellationToken token)
        {
            return context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.UserId == userId, token);
        }
        public Task<Teacher?> GetTeacherByProfileIdAsync(Guid profileId, CancellationToken token)
        {
            return context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.ProfileId == profileId, token);
        }
    }
}

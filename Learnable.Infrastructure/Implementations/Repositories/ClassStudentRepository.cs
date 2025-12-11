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
    public class ClassStudentRepository(ApplicationDbContext context)
        : GenericRepository<ClassStudent>(context), IClassStudentRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<bool> IsStudentInClassAsync(Guid classId, Guid userId, CancellationToken cancellationToken)
        {
            return await _context.ClassStudents
                .AnyAsync(cs => cs.ClassId == classId && cs.UserId == userId, cancellationToken);
        }

        public async Task<IEnumerable<ClassStudent>> GetStudentClassesAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.ClassStudents
                .Where(cs => cs.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ClassStudent>> GetClassStudentsAsync(Guid classId, CancellationToken cancellationToken)
        {
            return await _context.ClassStudents
                .Where(cs => cs.ClassId == classId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsStudentAlreadyJoinedAsync(Guid classId, Guid userId, CancellationToken ct)
        {
            return await _context.ClassStudents
                .AnyAsync(cs => cs.ClassId == classId && cs.UserId == userId, ct);
        }

        public async Task<List<Class>> GetClassesForStudentAsync(Guid userId, CancellationToken ct)
        {
            return await _context.ClassStudents
                .Where(cs => cs.UserId == userId)
                .Include(cs => cs.Class)
                .Select(cs => cs.Class)
                .ToListAsync(ct);
        }

        //grt student id by class id
        public async Task<List<Guid>> GetStudentIdsByClassIdAsync(Guid classId)
        {
            return await _context.ClassStudents
                .Where(cs => cs.ClassId == classId)
                .Select(cs => cs.UserId)
                .ToListAsync();
        }

    }
}

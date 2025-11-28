using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Repositories
{
    public interface IClassStudentRepository : IGenericRepository<ClassStudent>
    {
        Task<bool> IsStudentInClassAsync(Guid classId, Guid userId, CancellationToken ct);
        Task<IEnumerable<ClassStudent>> GetStudentClassesAsync(Guid userId, CancellationToken ct);
        Task<IEnumerable<ClassStudent>> GetClassStudentsAsync(Guid classId, CancellationToken ct);
        Task<List<Class>> GetClassesForStudentAsync(Guid userId, CancellationToken ct);

        Task<bool> IsStudentAlreadyJoinedAsync(Guid classId, Guid userId, CancellationToken ct);
    }

}

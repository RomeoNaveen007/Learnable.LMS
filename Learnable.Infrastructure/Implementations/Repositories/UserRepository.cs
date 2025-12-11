using Learnable.Application.Interfaces.Repositories;
using Learnable.Domain.Common.OTP;
using Learnable.Domain.Entities;
using Learnable.Infrastructure.Implementations.Repositories.Generic;
using Learnable.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Learnable.Infrastructure.Implementations.Repositories
{
    public class UserRepository(ApplicationDbContext context) : GenericRepository<User>(context), IUserRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<UserOtp?> GetOtpByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.UserOtps.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task AddOtpAsync(UserOtp otp, CancellationToken cancellationToken)
        {
            await _context.UserOtps.AddAsync(otp, cancellationToken);
        }

        public async Task DeleteOtpAsync(UserOtp otp, CancellationToken cancellationToken)
        {
            _context.UserOtps.Remove(otp);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(u => u.Username == username, cancellationToken);
        }
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
        
        public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users
                // Teacher → Classes
                .Include(u => u.Teacher)
                    .ThenInclude(t => t.Classes)
             .Include(u => u.ClassStudents)
                        .ThenInclude(cs => cs.Class)

        .FirstOrDefaultAsync(u => u.UserId == id, cancellationToken);
            ;
        }

    }
}

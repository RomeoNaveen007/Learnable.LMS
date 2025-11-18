using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Domain.Common.OTP;
using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<UserOtp?> GetOtpByEmailAsync(string email, CancellationToken cancellationToken);
        Task AddOtpAsync(UserOtp otp, CancellationToken cancellationToken);
        Task DeleteOtpAsync(UserOtp otp, CancellationToken cancellationToken);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
        Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    }
}

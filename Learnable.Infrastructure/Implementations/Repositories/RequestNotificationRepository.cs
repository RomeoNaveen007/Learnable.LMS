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
    public class RequestNotificationRepository : GenericRepository<RequestNotification>, IRequestNotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestNotificationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RequestNotification>> GetPendingRequestsByReceiverIdAsync(Guid receiverId, CancellationToken cancellationToken)
        {
            return await _context.RequestNotifications
                .Include(r => r.Class)
                .Include(r => r.Sender)
                .Where(r => r.ReceiverId == receiverId && r.NotificationStatus == "Pending")
                .ToListAsync(cancellationToken);
        }


    }
}

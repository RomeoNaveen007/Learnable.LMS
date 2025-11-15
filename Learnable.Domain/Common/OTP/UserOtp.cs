using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Domain.Common.OTP
{
    public class UserOtp
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = null!;
        public string OtpCode { get; set; } = null!;
        public int Attempts { get; set; } = 0;
        public DateTime ExpiresAt { get; set; }

    }
}

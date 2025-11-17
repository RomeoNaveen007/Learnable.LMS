using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Domain.Common.OTP
{
    [Table("UserOtp")]
    [Index(nameof(Email), Name = "IX_UserOtp_Email")]
    public partial class UserOtp
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string OtpCode { get; set; } = null!;

        public int Attempts { get; set; } = 0;

        [Column(TypeName = "datetime")]
        public DateTime ExpiresAt { get; set; }
    }
}

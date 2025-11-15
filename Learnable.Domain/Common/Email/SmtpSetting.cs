using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Domain.Common.Email
{
    [Table("SmtpSetting")] 
    public partial class SmtpSetting
    { 
        [Key] 
        public Guid Id { get; set; } 
        [StringLength(200)] 
        public string Host { get; set; } = ""; 
        public int Port { get; set; } = 587; 
        public bool UseSsl { get; set; } = true; 
        [StringLength(200)] 
        public string Username { get; set; } = ""; 
        [StringLength(200)] 
        public string Password { get; set; } = ""; 
        [StringLength(100)] 
        public string FromName { get; set; } = ""; 
        [StringLength(150)] 
        public string FromEmail { get; set; } = ""; 
    
    }

}

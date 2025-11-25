using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

[Table("User")]
[Index("Username", Name = "UQ__User__536C85E401B99EE3", IsUnique = true)]
[Index("Email", Name = "UQ__User__A9D105340B8B0374", IsUnique = true)]
public partial class User
{
    [Key]
    public Guid UserId { get; set; }

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(100)]
    public string? DisplayName { get; set; }

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(255)]
    public string PasswordSalt { get; set; } = null!;

    [StringLength(50)]
    public string Role { get; set; } = null!;

    [StringLength(100)]
    public string? FullName { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("Receiver")]
    public virtual ICollection<RequestNotification> RequestNotificationReceivers { get; set; } = new List<RequestNotification>();

    [InverseProperty("Sender")]
    public virtual ICollection<RequestNotification> RequestNotificationSenders { get; set; } = new List<RequestNotification>();

    [InverseProperty("User")]
    public virtual Student? Student { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();


    [InverseProperty("User")]
    public virtual Teacher? Teacher { get; set; }
}

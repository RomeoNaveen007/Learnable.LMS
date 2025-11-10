using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

[Table("AuditLog")]
public partial class AuditLog
{
    [Key]
    public Guid LogId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ClassId { get; set; }

    [StringLength(50)]
    public string? Action { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Timestamp { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("AuditLogs")]
    public virtual Class? Class { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("AuditLogs")]
    public virtual User? User { get; set; }
}

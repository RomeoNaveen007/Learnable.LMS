using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

[Table("Class")]
[Index("ClassJoinName", Name = "UQ__Class__F103E8E0B0547C57", IsUnique = true)]
public partial class Class
{
    [Key]
    public Guid ClassId { get; set; }

    public Guid? TeacherId { get; set; }

    [StringLength(100)]
    public string ClassName { get; set; } = null!;

    [StringLength(100)]
    public string ClassJoinName { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [InverseProperty("Class")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("Class")]
    public virtual ICollection<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();

    [InverseProperty("Class")]
    public virtual ICollection<Repository> Repositories { get; set; } = new List<Repository>();

    [ForeignKey("TeacherId")]
    [InverseProperty("Classes")]
    public virtual Teacher? Teacher { get; set; }
}

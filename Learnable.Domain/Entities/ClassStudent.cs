using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

[PrimaryKey("ClassId", "StudentId")]
[Table("ClassStudent")]
public partial class ClassStudent
{
    [Key]
    public Guid ClassId { get; set; }

    [Key]
    public Guid StudentId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? JoinDate { get; set; }

    [StringLength(50)]
    public string? StudentStatus { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("ClassStudents")]
    public virtual Class Class { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("ClassStudents")]
    public virtual Student Student { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

[Table("Student")]
[Index("UserId", Name = "UQ__Student__1788CC4D4FFBBC12", IsUnique = true)]
public partial class Student
{
    [Key]
    public Guid StudentId { get; set; }

    public Guid? UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EnrollmentDate { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

    [ForeignKey("UserId")]
    [InverseProperty("Student")]
    public virtual User? User { get; set; }
}

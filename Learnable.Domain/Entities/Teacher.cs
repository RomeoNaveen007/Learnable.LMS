using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

[Table("Teacher")]
[Index("UserId", Name = "UQ__Teacher__1788CC4DFD7991BE", IsUnique = true)]
public partial class Teacher
{
    [Key]
    public Guid ProfileId { get; set; }  // teacherid

    public Guid? UserId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(20)]
    public string? ContactPhone { get; set; }

    public string? Bio { get; set; }  // string to keep

    [StringLength(255)]
    public string? AvatarUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastUpdatedAt { get; set; }

    [InverseProperty("Teacher")]
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    [ForeignKey("UserId")]
    [InverseProperty("Teacher")]
    public virtual User? User { get; set; }
}

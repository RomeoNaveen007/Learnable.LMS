using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

[Table("Repository")]
public partial class Repository
{
    [Key]
    public Guid? RepoId { get; set; }

    public Guid ClassId { get; set; }

    [StringLength(100)]
    public string RepoName { get; set; } = null!;

    public string? RepoDescription { get; set; }

    [StringLength(255)]
    public string? RepoCertification { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [InverseProperty("Repo")]
    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();

    [ForeignKey("ClassId")]
    [InverseProperty("Repositories")]
    public virtual Class? Class { get; set; }

    [InverseProperty("Repo")]
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

public partial class Asset
{
    [Key]
    public Guid AssetsProfileId { get; set; }

    public Guid? RepoId { get; set; }

    [StringLength(50)]
    public string Type { get; set; } = null!;

    [StringLength(150)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastUpdatedAt { get; set; }

    [StringLength(255)]
    public string Url { get; set; } = null!;

    [ForeignKey("RepoId")]
    [InverseProperty("Assets")]
    public virtual Repository? Repo { get; set; }
}

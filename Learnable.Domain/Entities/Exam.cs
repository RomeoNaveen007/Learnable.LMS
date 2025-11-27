using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

[Table("Exam")]
public partial class Exam
{
    [Key]
    public Guid ExamId { get; set; }

    public Guid? RepoId { get; set; }

    [StringLength(100)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDatetime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDatetime { get; set; }

    public int? Duration { get; set; }

    [InverseProperty("Exam")]
    public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

    [InverseProperty("Exam")]
    public virtual ICollection<ExamQuestion> Questions { get; set; } = new List<ExamQuestion>();

    [ForeignKey("RepoId")]
    [InverseProperty("Exams")]
    public virtual Repository? Repo { get; set; }
}

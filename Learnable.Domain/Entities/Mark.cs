using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

[PrimaryKey("ExamId", "StudentId")]
public partial class Mark
{
    [Key]
    public Guid ExamId { get; set; }

    [Key]
    public Guid StudentId { get; set; }

    public int? Marks { get; set; }

    [StringLength(50)]
    public string? ExamStatus { get; set; }

    [ForeignKey("ExamId")]
    [InverseProperty("Marks")]
    public virtual Exam Exam { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("Marks")]
    public virtual Student Student { get; set; } = null!;

    // 🔥 Add this navigation
    [InverseProperty("Mark")]
    public virtual ICollection<StudentsAnswer> StudentsAnswers { get; set; } = new List<StudentsAnswer>();
}

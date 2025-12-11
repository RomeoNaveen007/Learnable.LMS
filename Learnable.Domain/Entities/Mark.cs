using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

public partial class Mark
{
    // 🔥 NEW PRIMARY KEY
    [Key]
    public Guid MarkId { get; set; } = Guid.NewGuid();

    // 🔥 Foreign Keys
    public Guid ExamId { get; set; }
    public Guid StudentId { get; set; }

    public int? Marks { get; set; }

    [StringLength(50)]
    public string? ExamStatus { get; set; }

    // 🔥 Relationship → Exam
    [ForeignKey("ExamId")]
    [InverseProperty("Marks")]
    public virtual Exam Exam { get; set; } = null!;

    // 🔥 Relationship → User (Student)
    [ForeignKey("StudentId")]
    [InverseProperty("Marks")]
    public virtual User User { get; set; } = null!;

    // 🔥 Relationship → StudentsAnswer
    [InverseProperty("Mark")]
    public virtual ICollection<StudentsAnswer> StudentsAnswers { get; set; }
        = new List<StudentsAnswer>();
}

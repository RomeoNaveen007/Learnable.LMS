using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learnable.Domain.Entities
{
    [Table("StudentsAnswers")]
    public class StudentsAnswer
    {
        [Key]
        public Guid Id { get; set; }

        // ---- Foreign Key to Mark (Composite Key) ----
        public Guid ExamId { get; set; }
        public Guid StudentId { get; set; }

        // ---- Foreign Key to ExamQuestion ----
        public Guid QuestionId { get; set; }

        public int AnswerIndex { get; set; }
        public DateTime SubmittedAt { get; set; }

        // ---- Navigation Properties ----
        [ForeignKey(nameof(ExamId) + "," + nameof(StudentId))]
        public virtual Mark Mark { get; set; } = null!;

        [ForeignKey(nameof(QuestionId))]
        public virtual ExamQuestion Question { get; set; } = null!;
    }
}

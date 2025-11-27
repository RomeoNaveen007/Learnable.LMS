using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Domain.Entities
{
    [Table("ExamQuestion")]
    public class ExamQuestion
    {
        [Key]
        public Guid QuestionId { get; set; }

        // Foreign key to Exam
        public Guid ExamId { get; set; }

        [Required]
        public string Question { get; set; } 

        // Stored as JSON or separate table depending on your choice
        [NotMapped]
        public List<string> Answers { get; set; } = new List<string>();

        public int CorrectAnswerIndex { get; set; }


        [ForeignKey("ExamId")]
        [InverseProperty("Questions")]
        public virtual Exam Exam { get; set; } = null!;
    }
}

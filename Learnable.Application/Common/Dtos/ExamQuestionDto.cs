using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Dtos
{
    public class ExamQuestionDto
    {
        public Guid QuestionId { get; set; }
        public Guid ExamId { get; set; }
        public string? Question { get; set; }
        public List<string> Answers { get; set; } = new List<string>();
        public int CorrectAnswerIndex { get; set; }

        public virtual Exam Exam { get; set; } = null!;
    }
}

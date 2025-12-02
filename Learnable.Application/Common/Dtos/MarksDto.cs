using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Dtos
{
    public class MarksDto
    {
        public Guid ExamId { get; set; }
        public Guid StudentId { get; set; }
        public int? Marks { get; set; }
        public string? ExamStatus { get; set; }
        public virtual ICollection<StudentAnswerDto> StudentsAnswers { get; set; }
    }
}

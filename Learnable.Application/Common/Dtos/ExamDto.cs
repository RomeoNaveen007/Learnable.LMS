using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Dtos
{
    public class ExamDto
    {
        public Guid ExamId { get; set; }
        public Guid? RepoId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? StartDatetime { get; set; }
        public DateTime? EndDatetime { get; set; }
        public int? Duration { get; set; }



        /*public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();*/
        public virtual ICollection<ExamQuestionDto> Questions { get; set; } = new List<ExamQuestionDto>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Dtos
{
    internal class StudentAnswerDto
    {
        public Guid QuestionId { get; set; }
        public int AnswerIndex { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}

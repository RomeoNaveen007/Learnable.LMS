using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Exam.Commands.Update
{
    public class UpdateExamDto
    {
        public Guid ExamId { get; set; }
        public Guid? RepoId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDatetime { get; set; }
        public DateTime? EndDatetime { get; set; }
        public int? Duration { get; set; }

        public List<UpdateExamQuestionDto> Questions { get; set; } = new();
    }

    public class UpdateExamQuestionDto
    {
        public string Question { get; set; } = null!;
        public List<string> Answers { get; set; } = new();
        public int CorrectAnswerIndex { get; set; }
    }
}

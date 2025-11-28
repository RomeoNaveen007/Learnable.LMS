using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Exam.Commands.Create
{
    public class CreateExamDto
    {
        public Guid? RepoId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? StartDatetime { get; set; }
        public DateTime? EndDatetime { get; set; }
        public int? Duration { get; set; }
        public List<CreateExamQuestionDto> Question { get; set; } = new();

    }
    public class CreateExamQuestionDto
    {
        public string Question { get; set; } = null!;
        public List<string> Answers { get; set; } = new();
        public int CorrectAnswerIndex { get; set; }
    }
}

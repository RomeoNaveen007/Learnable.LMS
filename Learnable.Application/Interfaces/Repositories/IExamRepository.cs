using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Repositories
{
    public interface IExamRepository
    {
        Task<Exam> AddExamAsync(Exam exam, List<ExamQuestion> questions);
        Task<Exam?> GetExamByIdAsync(Guid examId);
        Task<bool> DeleteExamAsync(Guid examId);
        Task<Exam?> UpdateExamAsync(Exam updatedExam, List<ExamQuestion> updatedQuestions);
        Task<Exam?> GetLastCreatedExamAsync();
    }
}

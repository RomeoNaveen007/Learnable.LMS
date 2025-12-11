using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Repositories
{
    public interface IMarksRepostiory
    {
        // Add a Mark with StudentsAnswers
        Task<Mark> AddMarkAsync(Mark mark);

        // Get a Mark with StudentsAnswers by ExamId + StudentId
        Task<Mark?> GetMarkAsync(Guid examId, Guid studentId);

        // Get all Marks for a specific ExamId
        Task<List<Mark>> GetMarksByExamIdAsync(Guid examId);

        // Update a Mark and its StudentsAnswers
        Task<Mark?> UpdateMarkAsync(Mark updatedMark);

        // Delete a Mark and its related StudentsAnswers
        Task<bool> DeleteMarkAsync(Guid examId, Guid studentId);
        Task<bool> DeleteMarksByExamIdAsync(Guid examId);
    }
}

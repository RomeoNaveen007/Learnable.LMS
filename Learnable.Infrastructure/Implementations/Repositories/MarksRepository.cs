using Learnable.Application.Interfaces.Repositories;
using Learnable.Domain.Entities;
using Learnable.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learnable.Infrastructure.Implementations.Repositories
{
    public class MarksRepository : IMarksRepostiory
    {
        private readonly ApplicationDbContext _context;

        public MarksRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Add Mark with StudentsAnswers
        public async Task<Mark> AddMarkAsync(Mark mark)
        {
            if (mark.StudentsAnswers != null && mark.StudentsAnswers.Any())
            {
                foreach (var answer in mark.StudentsAnswers)
                {
                    answer.ExamId = mark.ExamId;
                    answer.StudentId = mark.StudentId;
                }
            }

            _context.Marks.Add(mark);
            await _context.SaveChangesAsync();
            return mark;
        }

        // 2️⃣ Get Mark with StudentsAnswers by ExamId + StudentId
        public async Task<Mark?> GetMarkAsync(Guid examId, Guid studentId)
        {
            return await _context.Marks
                .Include(m => m.StudentsAnswers)
                .SingleOrDefaultAsync(m => m.ExamId == examId && m.StudentId == studentId);
        }

        // 3️⃣ Get Marks by ExamId
        public async Task<List<Mark>> GetMarksByExamIdAsync(Guid examId)
        {
            return await _context.Marks
                .Include(m => m.StudentsAnswers)
                .Where(m => m.ExamId == examId)
                .ToListAsync();
        }

        // 4️⃣ Update Mark and StudentsAnswers
        public async Task<Mark?> UpdateMarkAsync(Mark updatedMark)
        {
            var existingMark = await _context.Marks
                .Include(m => m.StudentsAnswers)
                .SingleOrDefaultAsync(m => m.ExamId == updatedMark.ExamId && m.StudentId == updatedMark.StudentId);

            if (existingMark == null) return null;

            // Update Mark fields
            existingMark.Marks = updatedMark.Marks;
            existingMark.ExamStatus = updatedMark.ExamStatus;

            // Update or add StudentsAnswers
            foreach (var answer in updatedMark.StudentsAnswers)
            {
                var existingAnswer = existingMark.StudentsAnswers
                    .SingleOrDefault(a => a.QuestionId == answer.QuestionId);

                if (existingAnswer != null)
                {
                    existingAnswer.AnswerIndex = answer.AnswerIndex;
                    existingAnswer.SubmittedAt = answer.SubmittedAt;
                }
                else
                {
                    answer.ExamId = updatedMark.ExamId;
                    answer.StudentId = updatedMark.StudentId;
                    existingMark.StudentsAnswers.Add(answer);
                }
            }

            await _context.SaveChangesAsync();
            return existingMark;
        }

        // 5️⃣ Delete Mark and related StudentsAnswers
        public async Task<bool> DeleteMarkAsync(Guid examId, Guid studentId)
        {
            var mark = await _context.Marks
                .Include(m => m.StudentsAnswers)
                .SingleOrDefaultAsync(m => m.ExamId == examId && m.StudentId == studentId);

            if (mark == null) return false;

            _context.StudentsAnswers.RemoveRange(mark.StudentsAnswers);
            _context.Marks.Remove(mark);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

using Learnable.Application.Interfaces.Repositories;
using Learnable.Domain.Entities;
using Learnable.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Infrastructure.Implementations.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly ApplicationDbContext _context;

        public ExamRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        // 1. ADD EXAM + QUESTIONS

        public async Task<Exam> AddExamAsync(Exam exam, List<ExamQuestion> questions)
        {
            exam.ExamId = Guid.NewGuid();

            foreach (var q in questions)
            {
                q.QuestionId = Guid.NewGuid();
                q.ExamId = exam.ExamId;
            }

            exam.Questions = questions;

            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

            return exam;
        }


        // 2. GET EXAM + QUESTIONS BY ID

        public async Task<Exam?> GetExamByIdAsync(Guid examId)
        {
            return await _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.ExamId == examId);
        }


        // 3. DELETE EXAM + QUESTIONS BY ID

        public async Task<bool> DeleteExamAsync(Guid examId)
        {
            var exam = await _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.ExamId == examId);

            if (exam == null) return false;

            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();

            return true;
        }


        // 4. UPDATE EXAM + QUESTIONS

        public async Task<Exam?> UpdateExamAsync(Exam updatedExam, List<ExamQuestion> updatedQuestions)
        {
            var existingExam = await _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.ExamId == updatedExam.ExamId);

            if (existingExam == null) return null;

            // Update exam fields
            existingExam.Title = updatedExam.Title;
            existingExam.Description = updatedExam.Description;
            existingExam.StartDatetime = updatedExam.StartDatetime;
            existingExam.EndDatetime = updatedExam.EndDatetime;
            existingExam.Duration = updatedExam.Duration;
            existingExam.RepoId = updatedExam.RepoId;

            // Remove old questions
            _context.ExamQuestions.RemoveRange(existingExam.Questions);

            // Add new questions
            foreach (var q in updatedQuestions)
            {
                q.QuestionId = Guid.NewGuid();
                q.ExamId = existingExam.ExamId;
            }
            existingExam.Questions = updatedQuestions;

            await _context.SaveChangesAsync();

            return existingExam;
        }
    }
}

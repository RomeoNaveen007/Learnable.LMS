using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Domain.Entities;
using Learnable.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Infrastructure.Implementations.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ExamRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }


        // 1. ADD EXAM + QUESTIONS
        public async Task<Exam> AddExamAsync(Exam exam, List<ExamQuestion> questions)
        {
            exam.ExamId = Guid.NewGuid();

            foreach (var q in questions)
            {
                q.QuestionId = Guid.NewGuid();
                q.ExamId = exam.ExamId;
                q.Exam = exam; // Important
            }

            exam.Questions = questions;

            _context.Exams.Add(exam);
            await _unitOfWork.SaveChangesAsync();

            return exam;
        }


        // 2. GET EXAM + QUESTIONS BY ID
        public async Task<Exam?> GetExamByIdAsync(Guid examId)
        {
            return await _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.ExamId == examId);
        }


        // 3. DELETE EXAM + QUESTIONS + MARKS (Updated)
        public async Task<bool> DeleteExamAsync(Guid examId)
        {
            // 🔥 UPDATE: Marks-ஐயும் சேர்த்து Include செய்கிறோம்
            var exam = await _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.ExamId == examId);

            if (exam == null) return false;

            _context.Exams.Remove(exam);

            await _unitOfWork.SaveChangesAsync();

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
            if (existingExam.Questions.Any())
                _context.ExamQuestions.RemoveRange(existingExam.Questions);

            // Add new questions and assign navigation property
            foreach (var q in updatedQuestions)
            {
                q.QuestionId = Guid.NewGuid();
                q.ExamId = existingExam.ExamId;
                q.Exam = existingExam; // Important
                _context.ExamQuestions.Add(q); // Ensure EF Core tracks
            }

            existingExam.Questions = updatedQuestions;

            await _unitOfWork.SaveChangesAsync();

            return existingExam;
        }

        // 5. GET LAST ADDED EXAM
        public async Task<Exam?> GetLastCreatedExamAsync()
        {
            return await _context.Exams
                .Include(e => e.Questions)
                .OrderByDescending(e => e.StartDatetime)
                .FirstOrDefaultAsync();
        }

    }
}
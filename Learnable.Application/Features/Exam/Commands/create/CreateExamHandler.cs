using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Exam.Commands.Create
{
    public class CreateExamHandler : IRequestHandler<CreateExamCommand, ExamDto>
    {
        private readonly IExamRepository _examRepository;
        private readonly IRepositoryRepository _repositoryRepository;
        private readonly IClassStudentRepository _classRepo;
        private readonly IMarksRepostiory _marksRepo;

        public CreateExamHandler(
            IExamRepository examRepository,
            IRepositoryRepository repositoryRepository,
            IClassStudentRepository classRepo,
            IMarksRepostiory marksRepo)
        {
            _examRepository = examRepository;
            _repositoryRepository = repositoryRepository;
            _classRepo = classRepo;
            _marksRepo = marksRepo;
        }

        public async Task<ExamDto> Handle(CreateExamCommand request, CancellationToken cancellationToken)
        {
            // ==========================================================
            // 1️⃣ Create Exam Object
            // ==========================================================

            // Note: இங்க ID create பண்ணுனாலும், DB ல Save ஆன பிறகு 
            // நாம புதுசா Fetch பண்ண போறோம்.

            var exam = new Learnable.Domain.Entities.Exam
            {
                // ExamId = Guid.NewGuid(), // Repository-லேயே ID create ஆகுது, so இங்க தேவையில்லை
                RepoId = request.Exam.RepoId,
                Title = request.Exam.Title,
                Description = request.Exam.Description,
                StartDatetime = request.Exam.StartDatetime,
                EndDatetime = request.Exam.EndDatetime,
                Duration = request.Exam.Duration,
            };

            var questions = request.Exam.Question.Select(q => new ExamQuestion
            {
                QuestionId = Guid.NewGuid(),
                Question = q.Question,
                Answers = q.Answers,
                CorrectAnswerIndex = q.CorrectAnswerIndex
            }).ToList();

            // ==========================================================
            // 2️⃣ SAVE EXAM FIRST
            // ==========================================================
            await _examRepository.AddExamAsync(exam, questions);

            // ==========================================================
            // 3️⃣ FETCH LAST CREATED EXAM (நீங்க கேட்ட புது Logic 🔥)
            // ==========================================================
            // DB-ல் Save ஆன பிறகு, கடைசியா விழுந்த ரெக்கார்டை எடுக்குறோம்
            var lastAddedExam = await _examRepository.GetLastCreatedExamAsync();

            if (lastAddedExam == null)
            {
                throw new Exception("Exam Creation Failed: Could not retrieve the saved exam.");
            }

            // இதுதான் DB-ல உறுதியா Save ஆன ID
            Guid finalExamId = lastAddedExam.ExamId;

            // ==========================================================
            // 4️⃣ Get Class & Students
            // ==========================================================
            var repositoryInfo = await _repositoryRepository.GetRepositoryWithIncludesAsync(request.Exam.RepoId, cancellationToken);

            if (repositoryInfo != null)
            {
                var studentIds = await _classRepo.GetStudentIdsByClassIdAsync(repositoryInfo.ClassId);

                // ==========================================================
                // 5️⃣ LOOP & SAVE MARK (Using Fetched ID)
                // ==========================================================
                foreach (var studentId in studentIds)
                {
                    var newMark = new Mark
                    {
                        ExamId = finalExamId,          // ✅ Fetch பண்ண ID-ஐ இங்க யூஸ் பண்றோம்
                        StudentId = studentId,
                        Marks = 0,
                        ExamStatus = "NonActive"
                    };

                    await _marksRepo.AddMarkAsync(newMark);
                }
            }

            // Return the fetched exam as DTO
            return lastAddedExam.ToDto();
        }
    }
}
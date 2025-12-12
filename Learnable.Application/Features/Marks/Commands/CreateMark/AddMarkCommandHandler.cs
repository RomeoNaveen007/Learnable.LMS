using Learnable.Application.Interfaces.Repositories;
using Learnable.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Marks.Commands.CreateMark
{
    internal class AddMarkCommandHandler
    : IRequestHandler<AddMarkCommand, List<Guid>>
    {
        private readonly IClassStudentRepository _classRepo;
        private readonly IMarksRepostiory _marksRepo;
        private readonly IExamRepository _examRepository;


        public AddMarkCommandHandler(
            IClassStudentRepository classRepo,
            IMarksRepostiory marksRepo,
            IExamRepository examRepository)
        {
            _classRepo = classRepo;
            _marksRepo = marksRepo;
            _examRepository = examRepository;
        }

        public async Task<List<Guid>> Handle(
            AddMarkCommand request,
            CancellationToken ct)
        {
            var examExists = await _examRepository.GetExamByIdAsync(request.ExamId);
            if (examExists == null)
                throw new KeyNotFoundException("Exam not found.");


            // 1️⃣ Get student ids from class
            var studentIds = await _classRepo.GetStudentIdsByClassIdAsync(request.ClassId);

            if (studentIds == null || studentIds.Count == 0)
                throw new KeyNotFoundException("Class not found or class has no students.");

            // 2️⃣ For returning values (list of newly created studentIds)
            List<Guid> createdForStudents = new();

            // 3️⃣ Create mark for each student
            foreach (var studentId in studentIds)
            {
                var newMark = new Mark
                {
                    ExamId = request.ExamId,
                    StudentId = studentId,
                    Marks = 0,                     // Default value
                    ExamStatus = "NonActive"     // Default status
                    
                };

                await _marksRepo.AddMarkAsync(newMark);
                createdForStudents.Add(studentId);
            }

            return createdForStudents;
        }
    }
}

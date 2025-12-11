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

        public AddMarkCommandHandler(
            IClassStudentRepository classRepo,
            IMarksRepostiory marksRepo)
        {
            _classRepo = classRepo;
            _marksRepo = marksRepo;
        }

        public async Task<List<Guid>> Handle(
            AddMarkCommand request,
            CancellationToken ct)
        {
            // 1️⃣ Get student ids from class
            var studentIds = await _classRepo.GetStudentIdsByClassIdAsync(request.ClassId);

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

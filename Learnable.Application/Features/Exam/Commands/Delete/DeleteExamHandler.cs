using Learnable.Application.Features.Exam.Commands.Delete;
using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Exam.Commands.Delete
{
    public class DeleteExamHandler : IRequestHandler<DeleteExamCommand, bool>
    {
        private readonly IExamRepository _examRepository;
        private readonly IMarksRepostiory _marksRepository; // 🔥 New Injection

        public DeleteExamHandler(IExamRepository examRepository, IMarksRepostiory marksRepository)
        {
            _examRepository = examRepository;
            _marksRepository = marksRepository;
        }

        public async Task<bool> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ Step 1: First, delete all Marks associated with this Exam
            // (Note: This will also delete StudentsAnswers via Cascade/Logic in Repo)
            // Return value is ignored here because even if false (no marks found), we proceed to delete the exam.
            await _marksRepository.DeleteMarksByExamIdAsync(request.ExamId);

            // 2️⃣ Step 2: Now that Marks are gone, safely delete the Exam
            return await _examRepository.DeleteExamAsync(request.ExamId);
        }
    }
}
using Learnable.Application.Features.Class.Commands.DeleteClass;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Exam.Commands.Delete
{
    public class DeleteExamHandler : IRequestHandler<DeleteExamCommand, bool>
    {
        private readonly IExamRepository _examRepository;

        public DeleteExamHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<bool> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
        {
            return await _examRepository.DeleteExamAsync(request.ExamId);
        }
    }
}

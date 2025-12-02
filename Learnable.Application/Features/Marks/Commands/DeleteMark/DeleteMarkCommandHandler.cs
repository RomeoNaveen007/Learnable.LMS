using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Marks.Commands.DeleteMark
{
    public class DeleteMarkCommandHandler
    : IRequestHandler<DeleteMarkCommand, bool>
    {
        private readonly IMarksRepostiory _repository;

        public DeleteMarkCommandHandler(IMarksRepostiory repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteMarkCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteMarkAsync(request.ExamId, request.StudentId);
        }
    }

}

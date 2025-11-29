using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Exam.Queries.GetByID
{
    public class GetExamByIdHandler : IRequestHandler<GetExamByIdQuery, ExamDto?>
    {
        private readonly IExamRepository _examRepository;

        public GetExamByIdHandler(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<ExamDto?> Handle(GetExamByIdQuery request, CancellationToken cancellationToken)
        {
            var exam = await _examRepository.GetExamByIdAsync(request.ExamId);

            return exam?.ToDto();
        }
    }

}

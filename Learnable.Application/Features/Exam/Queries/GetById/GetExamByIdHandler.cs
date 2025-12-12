using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Exceptions;
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
            // 1️⃣ (Optional) Defensive check - should never happen because validator catches empty Guid
            if (request.ExamId == Guid.Empty)
                throw new BadRequestException("ExamId is invalid.");

            // 2️⃣ Get exam
            var exam = await _examRepository.GetExamByIdAsync(request.ExamId);

            // 3️⃣ If not found → trigger middleware
            if (exam == null)
                throw new KeyNotFoundException("Exam not found.");

            // 4️⃣ return mapped DTO
            return exam.ToDto();
        }
    }

}

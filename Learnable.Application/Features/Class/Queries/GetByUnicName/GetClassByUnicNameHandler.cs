using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Queries.GetByUnicName
{
    public class GetClassByUnicNameHandler : IRequestHandler<GetClassByUnicNameQuery, ClassDto?>
    {
        private readonly IClassRepository _repo;
        public GetClassByUnicNameHandler(IClassRepository repo)
        {
            _repo = repo;
        }
        public async Task<ClassDto?> Handle(GetClassByUnicNameQuery request, CancellationToken cancellationToken)
        {
            // 1. Repository-ல் இருந்து Data-வை எடுக்கிறோம்
            var data = await _repo.GetClassByJoinNameAsync(request.UnicName, cancellationToken);

            // 2. Data இல்லை என்றால் null return செய்கிறோம்
            if (data == null)
                return null;

            return new ClassDto
            {
                ClassId = data.ClassId,
                ClassName = data.ClassName ?? string.Empty,
                ClassJoinName = data.ClassJoinName ?? string.Empty,
                Description = data.Description,
                Status = data.Status,
                TeacherId = data.TeacherId
            };
        }
    }
}

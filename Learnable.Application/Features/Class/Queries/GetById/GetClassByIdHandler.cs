using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Queries.GetById
{
    public class GetClassByIdQueryHandler
        : IRequestHandler<GetClassByIdQuery, ClassDto?>
    {
        private readonly IClassRepository _repo;

        public GetClassByIdQueryHandler(IClassRepository repo)
        {
            _repo = repo;
        }

        public async Task<ClassDto?> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _repo.GetClassWithIncludesAsync(request.ClassId, cancellationToken);

            if (data == null)
                return null;

            return new ClassDto
            {
                ClassId = data.ClassId,
                ClassName = data.ClassName,
                ClassJoinName = data.ClassJoinName,
                Description = data.Description,
                TeacherId = data.TeacherId,
                Status = data.Status
            };
        }
    }
}

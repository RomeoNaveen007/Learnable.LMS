using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Teacher.Queries.GetAllTeachers
{
    public class GetAllTeachersQueryHandler :
        IRequestHandler<GetAllTeachersQuery, List<TeacherDto>>
    {
        private readonly ITeacherRepository _repo;

        public GetAllTeachersQueryHandler(ITeacherRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<TeacherDto>> Handle(
            GetAllTeachersQuery request,
            CancellationToken cancellationToken)
        {
            var teachers = await _repo.GetAllTeachersWithUserAndClassesAsync();

            return teachers.Select(t => t.ToDto()).ToList();
        }
    }
}

using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Teacher.Queries.GetTeacherById
{
    public class GetTeacherByIdQueryHandler :
        IRequestHandler<GetTeacherByIdQuery, TeacherDto?>
    {
        private readonly ITeacherRepository _repo;

        public GetTeacherByIdQueryHandler(ITeacherRepository repo)
        {
            _repo = repo;
        }

        public async Task<TeacherDto?> Handle(
            GetTeacherByIdQuery request,
            CancellationToken cancellationToken)
        {
            var teacher = await _repo.GetTeacherWithUserAndClassesAsync(request.ProfileId, cancellationToken);

            if (teacher == null)
                return null;

            if (teacher.UserId != request.LoggedInUserId)
                throw new UnauthorizedAccessException(
                    "You are not allowed to view this teacher.");

            return teacher.ToDto();
        }
    }
}

using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Teacher.Commands.UpdateTeacher
{
    public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand, TeacherDto?>
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTeacherCommandHandler(
            ITeacherRepository teacherRepository,
            IUnitOfWork unitOfWork)
        {
            _teacherRepository = teacherRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TeacherDto?> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
        {
            // Fetch Teacher + related User
            var teacher = await _teacherRepository
                .GetTeacherWithUserAndClassesAsync(request.ProfileId, cancellationToken);

            if (teacher == null)
                return null;

            // === Update Teacher fields ===
            if (request.DateOfBirth is not null)
                teacher.DateOfBirth = request.DateOfBirth;

            if (request.ContactPhone is not null)
                teacher.ContactPhone = request.ContactPhone;

            if (request.Bio is not null)
                teacher.Bio = request.Bio;

            if (request.AvatarUrl is not null)
                teacher.AvatarUrl = request.AvatarUrl;

            teacher.LastUpdatedAt = DateTime.UtcNow;

            // === Update User fields ===
            if (teacher.User is not null)
            {
                if (request.DisplayName is not null)
                    teacher.User.DisplayName = request.DisplayName;

                if (request.FullName is not null)
                    teacher.User.FullName = request.FullName;

                if (request.Username is not null)
                    teacher.User.Username = request.Username;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return teacher.ToDto();
        }
    }
}

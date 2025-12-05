using Learnable.Application.Common.Dtos;
using MediatR;

namespace Learnable.Application.Features.Account.Commands.RegisterTeacher
{
    public record RegisterTeacherCommand(
        RegisterTeacherDto Dto
    ) : IRequest<TeacherUserDto>;
}

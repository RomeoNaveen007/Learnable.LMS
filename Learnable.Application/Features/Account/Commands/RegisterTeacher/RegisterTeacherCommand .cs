using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Users.Commands.RegisterUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Account.Commands.RegisterTeacher
{
    public record RegisterTeacherCommand(
        RegisterTeacherDto Dto,  
        string OtpCode       
    ) : IRequest<TeacherUserDto>;
}

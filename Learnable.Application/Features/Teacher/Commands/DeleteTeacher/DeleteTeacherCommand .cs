using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Teacher.Commands.DeleteTeacher
{
    public class DeleteTeacherCommand(Guid profileId) : IRequest<bool>
    {
        public Guid ProfileId { get; set; } = profileId;
    }
}

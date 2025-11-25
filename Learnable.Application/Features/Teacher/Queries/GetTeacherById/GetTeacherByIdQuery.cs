using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Teacher.Queries.GetTeacherById
{
    public class GetTeacherByIdQuery : IRequest<TeacherDto?>
    {
        public Guid ProfileId { get; set; }
        public Guid LoggedInUserId { get; set; }
    }
}

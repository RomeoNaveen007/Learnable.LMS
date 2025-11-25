using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Teacher.Queries.GetAllTeachers
{
    public class GetAllTeachersQuery : IRequest<List<TeacherDto>>
    {
    }
}

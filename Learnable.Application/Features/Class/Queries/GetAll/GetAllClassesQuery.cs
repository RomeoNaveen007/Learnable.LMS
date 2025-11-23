using Learnable.Application.Common.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Queries.GetAll
{
    public record GetAllClassesQuery() : IRequest<IEnumerable<ClassDto>>;
}

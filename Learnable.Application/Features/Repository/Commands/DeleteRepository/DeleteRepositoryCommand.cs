using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Commands.DeleteRepository
{
    public record DeleteRepositoryCommand(Guid RepoId) : IRequest<bool>;
}

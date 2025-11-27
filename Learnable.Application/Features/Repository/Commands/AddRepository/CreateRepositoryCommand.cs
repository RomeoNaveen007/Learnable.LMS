using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Class.Commands.AddClass;
using Learnable.Application.Features.Repository.Commands.AddRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Commands.AddRepository
{
    // Fix: Ensure CreateRepositoryCommand implements IRequest<RepositoryDtos>
    public class CreateRepositoryCommand : IRequest<RepositoryDtos>
    {
        public CreateRepositoryDto CreateRepositoryDto { get; set; }
    }
}
/*RepositoryDtos*/


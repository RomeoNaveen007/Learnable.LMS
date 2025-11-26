using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Class.Commands.AddClass;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Commands.AddRepository
{
    public class CreateRepositoryCommand : IRequest<RepositoryDtos>
    {
        private CreateRepositoryDto dto;

        public CreateRepositoryCommand(CreateRepositoryDto dto)
        {
            this.dto = dto;
        }

        public CreateRepositoryDto CreateRepositoryDto { get; set; } = null!;
    }
}

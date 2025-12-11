using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Exceptions;
using Learnable.Application.Common.Extensions;
using Learnable.Application.Features.Class.Commands.AddClass;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Commands.AddRepository
{
    public class CreateRepositoryHandler : IRequestHandler<CreateRepositoryCommand, RepositoryDtos>
    {
        private readonly IGenericRepository<Learnable.Domain.Entities.Repository> _repositoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRepositoryHandler(
            IGenericRepository<Learnable.Domain.Entities.Repository> repositoryRepository,
            IUnitOfWork unitOfWork)
        {
            _repositoryRepository = repositoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RepositoryDtos> Handle(CreateRepositoryCommand request, CancellationToken cancellationToken)
        {

            var newRepository = new Learnable.Domain.Entities.Repository
            {
                RepoId = Guid.NewGuid(),
                ClassId = (Guid)request.CreateRepositoryDto.ClassId,
                RepoName = request.CreateRepositoryDto.RepoName,
                RepoDescription = request.CreateRepositoryDto.RepoDescription,
                RepoCertification = request.CreateRepositoryDto.RepoCertification,
                Status = request.CreateRepositoryDto.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _repositoryRepository.CreateAsync(newRepository);

            await _unitOfWork.SaveChangesAsync();

            return newRepository.ToDto();
        }
    }

   
}

using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Class.Commands.UpdateClass;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Commands.UpdateRepository
{
     class UpdateRepositoryHandler : IRequestHandler<UpdateRepositoryCommand, RepositoryDtos?>
    {
        private readonly IGenericRepository<Learnable.Domain.Entities.Repository> _repositoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRepositoryHandler(
            IGenericRepository<Learnable.Domain.Entities.Repository> repositoryRepository,
            IUnitOfWork unitOfWork)
        {
            _repositoryRepository = repositoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RepositoryDtos?> Handle(UpdateRepositoryCommand request, CancellationToken cancellationToken)
        {
            var repositoryEntity = await _repositoryRepository.GetByIdAsync(
                r => r.RepoId == request.RepoId
            );

            if (repositoryEntity == null)
                return null;

            // Update only provided fields
            if (!string.IsNullOrEmpty(request.RepoName))
                repositoryEntity.RepoName = request.RepoName;

            if (!string.IsNullOrEmpty(request.RepoDescription))
                repositoryEntity.RepoDescription = request.RepoDescription;

            if (!string.IsNullOrEmpty(request.RepoCertification))
                repositoryEntity.RepoCertification = request.RepoCertification;

            if (!string.IsNullOrEmpty(request.Status))
                repositoryEntity.Status = request.Status;

            if (request.CreatedAt != null)
                repositoryEntity.CreatedAt = request.CreatedAt;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RepositoryDtos
            {
                ClassId = repositoryEntity.ClassId,
                RepoName = repositoryEntity.RepoName,
                RepoDescription = repositoryEntity.RepoDescription,
                RepoCertification = repositoryEntity.RepoCertification,
                CreatedAt = repositoryEntity.CreatedAt,
                Status = repositoryEntity.Status
            };
        }
    }
}

using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Queries.GetById
{
    public class GetRepositoryByIdQueryHandler : IRequestHandler<GetRepositoryByIdQuery, RepositoryDtos?>
    {
        private readonly IRepositoryRepository _repositoryRepository;

        public GetRepositoryByIdQueryHandler(IRepositoryRepository repositoryRepository)
        {
            _repositoryRepository = repositoryRepository;
        }

        public async Task<RepositoryDtos?> Handle(GetRepositoryByIdQuery request, CancellationToken cancellationToken)
        {
            var repository = await _repositoryRepository.GetRepositoryWithIncludesAsync(request.RepoId, cancellationToken);

            if (repository == null)
                return null;

            return new RepositoryDtos
            {
                RepoId = repository.RepoId,
                ClassId = repository.ClassId,
                RepoName = repository.RepoName ?? string.Empty,
                RepoDescription = repository.RepoDescription ?? string.Empty,
                RepoCertification = repository.RepoCertification ?? string.Empty,
                Status = repository.Status ?? string.Empty,
                CreatedAt = repository.CreatedAt
            };
        }
    }
}

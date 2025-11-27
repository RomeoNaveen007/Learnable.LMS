using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Queries.GetAll
{
    public class GetAllRepositoryQueryHandler : IRequestHandler<GetAllRepositoryQuery, IEnumerable<RepositoryDtos>>
    {
        private readonly IRepositoryRepository _repositoryRepository;

        public GetAllRepositoryQueryHandler(IRepositoryRepository repositoryRepository)
        {
            _repositoryRepository = repositoryRepository;
        }

        public async Task<IEnumerable<RepositoryDtos>> Handle(GetAllRepositoryQuery request, CancellationToken cancellationToken)
        {
            var repositories = await _repositoryRepository.GetAllRepositoriesWithIncludesAsync(cancellationToken);

            return repositories.Select(r => new RepositoryDtos
            {
                RepoId = r.RepoId,
                ClassId = r.ClassId,
                RepoName = r.RepoName,
                RepoDescription = r.RepoDescription,
                RepoCertification = r.RepoCertification,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            }).ToList();
        }
    }
}

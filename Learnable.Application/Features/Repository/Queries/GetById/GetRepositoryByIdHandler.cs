using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Repositories;
using MediatR;
using System.Linq;
using System.Threading;
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
                ClassId = repository.ClassId,
                RepoName = repository.RepoName ?? string.Empty,
                RepoDescription = repository.RepoDescription ?? string.Empty,
                RepoCertification = repository.RepoCertification ?? string.Empty,
                Status = repository.Status ?? string.Empty,
                CreatedAt = repository.CreatedAt,

                // Map Exams to ExamDto
                Exams = repository.Exams.Select(e => new ExamDto
                {
                    ExamId = e.ExamId,
                    RepoId = e.RepoId,
                    Title = e.Title,
                    Description = e.Description,
                    StartDatetime = e.StartDatetime,
                    EndDatetime = e.EndDatetime,
                    Duration = e.Duration,
                }).ToList(),

                // Map Assets to AssetDto
                Assets = repository.Assets.Select(a => new AssetDto
                {
                    AssetId = a.AssetsProfileId,
                    RepoId = a.RepoId ?? Guid.Empty,
                    Type = a.Type,
                    Title = a.Title,
                    Description = a.Description,
                    CreatedAt = a.CreatedAt,
                    LastUpdatedAt = a.LastUpdatedAt,
                    Url = a.Url,
                }).ToList()
            };
        }
    }
}

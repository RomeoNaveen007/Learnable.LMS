using Learnable.Application.Features.Class.Commands.DeleteClass;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Commands.DeleteRepository
{
    public class DeleteRepositoryCommandHandler : IRequestHandler<DeleteRepositoryCommand, bool>
    {
        private readonly IGenericRepository<Learnable.Domain.Entities.Repository> _repositoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRepositoryCommandHandler(
            IGenericRepository<Learnable.Domain.Entities.Repository> repositoryRepository,
            IUnitOfWork unitOfWork)
        {
            _repositoryRepository = repositoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteRepositoryCommand request, CancellationToken cancellationToken)
        {
            var repositoryEntity = await _repositoryRepository.GetByIdAsync(
                x => x.RepoId == request.RepoId
            );

            if (repositoryEntity == null)
                return false;

            await _repositoryRepository.DeleteAsync(repositoryEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

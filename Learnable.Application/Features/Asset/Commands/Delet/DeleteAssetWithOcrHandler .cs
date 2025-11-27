using MediatR;
using Learnable.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

public class DeleteAssetWithOcrHandler : IRequestHandler<DeleteAssetWithOcrCommand, bool>
{
    private readonly IAssetReopsitory _assetRepository;

    public DeleteAssetWithOcrHandler(IAssetReopsitory assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<bool> Handle(DeleteAssetWithOcrCommand request, CancellationToken cancellationToken)
    {
        return await _assetRepository.DeleteAssetWithOcrAsync(request.AssetId);
    }
}

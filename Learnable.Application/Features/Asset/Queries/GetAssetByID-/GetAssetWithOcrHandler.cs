using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Asset.Queries.GetAssetByID_;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetAssetWithOcrHandler : IRequestHandler<GetAssetWithOcrQuery, AssetDto?>
{
    private readonly IAssetReopsitory _assetRepository;

    public GetAssetWithOcrHandler(IAssetReopsitory assetRepository)
    {
        _assetRepository = assetRepository;
    }

    public async Task<AssetDto?> Handle(GetAssetWithOcrQuery request, CancellationToken cancellationToken)
    {
       
        var asset = await _assetRepository.GetAssetWithOcrAsync(request.AssetId);

        if (asset == null) return null;

        var assetDto = new AssetDto
        {
            AssetId = asset.AssetsProfileId,
            Type = asset.Type,
            Title = asset.Title,
            RepoId= (Guid)asset.RepoId,
            Description = asset.Description,
            CreatedAt = asset.CreatedAt,
            LastUpdatedAt = asset.LastUpdatedAt,
            Url = asset.Url,
            OcrPdfs = asset.OcrPdfs.Select(o => new OcrPdfDto
            {
                OcrPdfId = o.OcrPdfId,
                ChunkId = o.ChunkId,
                Chunk = o.Chunk
            }).ToList()
        };

        return assetDto;
    }
}

using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Application.Interfaces.Services.External;
using Learnable.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssetEntity = Learnable.Domain.Entities.Asset;
using OcrPdfEntity = Learnable.Domain.Entities.OcrPdf;

namespace Learnable.Application.Features.Asset.Commands.AddAsset
{
    internal class AddAssetQuerieHandler : IRequestHandler<AddAssetQuerie, AssetDto>
    {
        private readonly IAssetReopsitory _assetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAiApiService _aiApiService;

        public AddAssetQuerieHandler(
            IAssetReopsitory assetRepository,
            IUnitOfWork unitOfWork,
            IAiApiService aiApiService)
        {
            _assetRepository = assetRepository;
            _unitOfWork = unitOfWork;
            _aiApiService = aiApiService;
        }

        public async Task<AssetDto> Handle(AddAssetQuerie request, CancellationToken cancellationToken)
        {
            // 1️⃣ Map DTO → Entity
            var asset = new AssetEntity
            {
                AssetsProfileId = Guid.NewGuid(),
                Title = request.Title,
                Type = request.Type,
                Url = request.Url,
                RepoId = request.RepoId,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                OcrPdfs = new List<OcrPdfEntity>()
            };

            // 2️⃣ Send initial OCR chunks to AI if available
            List<OcrPdfDto> aiResult = new List<OcrPdfDto>();
            if (request.OcrPdfs != null && request.OcrPdfs.Count > 0)
            {
                List<Application.Common.Dtos.OcrPdfDto>? dd = new List<Application.Common.Dtos.OcrPdfDto>();
                dd= request.OcrPdfs.Select(x => new Application.Common.Dtos.OcrPdfDto
                {
                    ChunkId = x.ChunkId,
                    Chunk = x.Chunk
                }).ToList();
                // AI service call
                aiResult = await _aiApiService.AskAiAsync(dd);
            }

            // 3️⃣ Map AI response → OcrPdf entities
            if (aiResult != null && aiResult.Count > 0)
            {
                int index = 1; // new ChunkIndex starting from 1
                foreach (var o in aiResult)
                {
                    asset.OcrPdfs.Add(new OcrPdfEntity
                    {
                        OcrPdfId = Guid.NewGuid(),
                        ChunkId = index++, // assign new index
                        Chunk = o.Chunk,
                        AssetId = asset.AssetsProfileId
                    });
                }
            }

            // 4️⃣ Save asset using repository
            var savedAsset = await _assetRepository.AddAssetWithOcrAsync(asset);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5️⃣ Return AssetDto with AI-corrected chunks
            return new AssetDto
            {
                AssetId = savedAsset.AssetsProfileId,
                Title = savedAsset.Title,
                Type = savedAsset.Type,
                Url = savedAsset.Url,
                RepoId = (Guid)savedAsset.RepoId,
                Description = savedAsset.Description,
                CreatedAt = savedAsset.CreatedAt,
                LastUpdatedAt = savedAsset.LastUpdatedAt,
                OcrPdfs = savedAsset.OcrPdfs.Select(x => new OcrPdfDto
                {
                    OcrPdfId = x.OcrPdfId,
                    ChunkId = x.ChunkId,
                    Chunk = x.Chunk
                }).ToList()
            };
        }
    }
}

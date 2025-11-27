using MediatR;
using Learnable.Domain.Entities;
using System;
using System.Collections.Generic;
using Learnable.Application.Common.Dtos;

namespace Learnable.Application.Features.Asset.Commands.AddAsset
{
    public class AddAssetQuerie : IRequest<AssetDto>
    {
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? Description { get; set; }
        public Guid RepoId { get; set; }
        public List<OcrPdfDto>? OcrPdfs { get; set; } // Optional OCR chunks
       

        public class OcrPdfDto
        {
            public int ChunkId { get; set; }
            public string Chunk { get; set; } = null!;
        }
    }
}

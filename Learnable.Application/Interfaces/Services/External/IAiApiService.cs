using System.Collections.Generic;
using System.Threading.Tasks;
using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Asset.Commands.AddAsset;

namespace Learnable.Application.Interfaces.Services.External
{
    public interface IAiApiService
    {
        Task<List<OcrPdfDto>> AskAiAsync(List<OcrPdfDto> chunks); // ✅ public DTO
    }
}

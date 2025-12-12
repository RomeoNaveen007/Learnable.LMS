using Learnable.Application.Common.Dtos;
using Learnable.Application.Common.Exceptions;
using Learnable.Application.Features.AiServises.Queries.Question.GetQuestions;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Services.External;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*GetQuestionsHandler*/
namespace Learnable.Application.Features.AiServises.Queries.Question.GetQuestions
{
    public class GetQuestionsHandler : IRequestHandler<GetQuestionQuery, List<ExamQuestionDto>>
    {
        private readonly IAssetReopsitory _assetRepo;
        private readonly IExamAiApiService _aiService;

        public GetQuestionsHandler (IAssetReopsitory assetRepo, IExamAiApiService aiService)
        {
            _assetRepo = assetRepo;
            _aiService = aiService;
        }

        public async Task<List<ExamQuestionDto>> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
        {
            if (request.Asset_Id == null || request.Asset_Id.Count == 0)
                throw new BadRequestException("No Asset Ids provided.");

            if (request.Question_Count <= 0)
                throw new BadRequestException("Question Count must be greater than zero.");

            List<OcrPdfDto> allChunks = new();

            // Get chunks from each asset
            foreach (var assetId in request.Asset_Id)
            {
                var asset = await _assetRepo.GetAssetWithOcrAsync(assetId);

                if (asset == null)
                    continue;

                foreach (var ocr in asset.OcrPdfs)
                {
                    allChunks.Add(new OcrPdfDto
                    {
                        OcrPdfId = ocr.OcrPdfId,
                        ChunkId = ocr.ChunkId,
                        Chunk = ocr.Chunk
                    });
                }
            }

            if (allChunks.Count == 0)
                throw new BadRequestException("No OCR data available for given asset IDs.");

            // Call AI generator
            var questions = await _aiService.GenerateQuestions(allChunks, request.Question_Count);

            return questions;
        }
    }
}

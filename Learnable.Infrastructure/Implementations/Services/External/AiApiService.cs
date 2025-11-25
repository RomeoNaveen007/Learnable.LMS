using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Learnable.Application.Features.Asset.Commands.AddAsset.AddAssetCommand;

namespace Learnable.Infrastructure.Implementations.Services.External
{
    // Service class (no IActionResult here)
    public class AiApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "AIzaSyBRKn8-cWzVZamJJXB0WZ52yw1XCq7bSeA";
        private readonly string google_cloud = "AIzaSyATEmVTy9AekgAdbNAIu_c5KR13QFlbxzM";

        private readonly string _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        public AiApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<OcrPdfDto>> AskAiAsync(List<OcrPdfDto> chunks)
        {
            if (chunks == null || chunks.Count == 0)
                throw new ArgumentException("Chunk list cannot be empty.");

            string prompt = "<task><description>You will receive text data in small chunk from a PDF OCR scan. Some letters may be missing and some words may have incorrect meanings. Correct each chunk and turn them into neat verses. Data is provided as a list of objects: { \"chunkIndex\": int, \"chunkText\": string } Return the response in the string list format,Use $ to highlight each meaningful verse. Place $ at the beginning and end of the verse., with each object containing the corrected  text. Maintain the order of chunks. Each verse must remain in its own chunk.</description></task>";

            foreach (var c in chunks)
                prompt += $"Chunk {c.ChunkId}: {c.Chunk}\n";

            var requestBody = new
            {
                contents = new[]
                {
                    new {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}?key={_apiKey}", content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"AI API error: {result}");

            using var doc = JsonDocument.Parse(result);
            var text = doc.RootElement
                          .GetProperty("candidates")[0]
                          .GetProperty("content")
                          .GetProperty("parts")[0]
                          .GetProperty("text")
                          .GetString();
            List<OcrPdfDto> gg = new List<OcrPdfDto>();
            string[] dd = text.Split('$');

            foreach (var c in dd)
            {
                OcrPdfDto hhh = new OcrPdfDto();
                hhh.Chunk = $"{c}";
                gg.Add(hhh);

            }
            for (int i = gg.Count - 1; i >= 0; i--)
            {
                if (i % 2 == 0)
                {
                    gg.RemoveAt(i);
                }

            }
            return gg;
        }
    }

   

    
    
}

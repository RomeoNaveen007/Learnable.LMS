using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Services.External;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Learnable.Application.Common.Dtos.OcrPdfDto;

namespace Learnable.Infrastructure.Implementations.Services.External
{
    public class AiApiService : IAiApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string google_cloud = "AIzaSyATEmVTy9AekgAdbNAIu_c5KR13QFlbxzM";
        private readonly string _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

        public AiApiService()
        {
            _httpClient = new HttpClient();
            _apiKey = "AIzaSyBRKn8-cWzVZamJJXB0WZ52yw1XCq7bSeA";
        }

        // Main AI processing method
        public async Task<List<OcrPdfDto>> AskAiAsync(List<OcrPdfDto> chunks)
        {
            if (chunks == null || chunks.Count == 0)
                throw new ArgumentException("Chunk list cannot be empty.");

            // Build prompt for AI
            string prompt = "<task><description>You will receive text data in small chunk from a PDF OCR scan. Some letters may be missing and some words may have incorrect meanings. Correct each chunk and turn them into neat verses. Data is provided as a list of objects: { \"chunkIndex\": int, \"chunkText\": string } Return the response in the string list format, Use $ to highlight each meaningful verse. Place $ at the beginning and end of the verse., with each object containing the corrected text. Maintain the order of chunks. Each verse must remain in its own chunk.</description></task>";

            foreach (var c in chunks)
                prompt += $"Chunk {c.ChunkId}: {c.Chunk}\n";

            // Prepare request payload
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

            // Call AI API
            var response = await _httpClient.PostAsync($"{_apiUrl}?key={_apiKey}", content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"AI API error: {result}");

            // Parse AI response
            using var doc = JsonDocument.Parse(result);
            var text = doc.RootElement
                          .GetProperty("candidates")[0]
                          .GetProperty("content")
                          .GetProperty("parts")[0]
                          .GetProperty("text")
                          .GetString();

            // Convert AI text into list of OcrPdfDto
            List<OcrPdfDto> processedChunks = new List<OcrPdfDto>();
            string[] dd = text.Split('$');

            int index = 1;
            foreach (var c in dd)
            {
                var trimmed = c.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    processedChunks.Add(new OcrPdfDto
                    {
                        ChunkId = index,
                        Chunk = trimmed
                    });
                    index++;
                }
            }

            for (int i = processedChunks.Count - 1; i >= 0; i--)
            {
                if (i % 2 == 0)
                {
                    processedChunks.RemoveAt(i);
                }

            }

            return processedChunks;
        }

    }
}

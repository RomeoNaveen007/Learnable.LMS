using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Services.External;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private readonly string _apiUrl;
        private readonly string _model;

        public AiApiService()
        {
            _httpClient = new HttpClient();

            // ⚠️ உங்கள் Groq API Key
            _apiKey = "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";

            // Groq Endpoint
            _apiUrl = "https://api.groq.com/openai/v1/chat/completions";

            // Model
            _model = "llama-3.3-70b-versatile";

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<List<OcrPdfDto>> AskAiAsync(List<OcrPdfDto> chunks)
        {
            if (chunks == null || chunks.Count == 0)
                throw new ArgumentException("Chunk list cannot be empty.");

            // Build prompt for AI
            string prompt = "<task><description>You will receive text data in small chunk from a PDF OCR scan. Some letters may be missing and some words may have incorrect meanings. Correct each chunk and turn them into neat verses. Data is provided as a list of objects: { \"chunkIndex\": int, \"chunkText\": string } Return the response in the string list format, Use $ to highlight each meaningful verse. Place $ at the beginning and end of the verse., with each object containing the corrected text. Maintain the order of chunks. Each verse must remain in its own chunk.</description></task>\n";

            foreach (var c in chunks)
                prompt += $"Chunk {c.ChunkId}: {c.Chunk}\n";

            // Prepare request payload
            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0.2
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Call AI API
            var response = await _httpClient.PostAsync(_apiUrl, content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Groq API error: {response.StatusCode} - {result}");

            // Parse AI response
            using var doc = JsonDocument.Parse(result);

            if (!doc.RootElement.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)
            {
                return new List<OcrPdfDto>();
            }

            var text = choices[0]
                       .GetProperty("message")
                       .GetProperty("content")
                       .GetString();

            if (string.IsNullOrEmpty(text))
                return new List<OcrPdfDto>();

            // Convert AI text into list of OcrPdfDto
            List<OcrPdfDto> processedChunks = new List<OcrPdfDto>();

            // Split by $ delimiter
            string[] dd = text.Split('$');

            int index = 1;
            foreach (var c in dd)
            {
                var trimmed = c.Trim();

                // ✅ UPDATE: இங்கே மாற்றம் செய்யப்பட்டுள்ளது
                // 30 எழுத்துக்களுக்கு குறைவாக இருந்தால் அது சேர்க்கப்படாது.
                if (!string.IsNullOrEmpty(trimmed) && trimmed.Length >= 20)
                {
                    processedChunks.Add(new OcrPdfDto
                    {
                        ChunkId = index,
                        Chunk = trimmed
                    });
                    index++;
                }
            }

            return processedChunks;
        }
    }
}
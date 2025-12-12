using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Services.External;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers; // புதிதாக சேர்க்கப்பட்டது
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Learnable.Infrastructure.Implementations.Services.External
{
    public class ExplanationService : IExplanationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly string _model;

        public ExplanationService(IConfiguration config)
        {
            _httpClient = new HttpClient();

            // ⚠️ இங்கே உங்கள் Groq API Key-ஐ பேஸ்ட் செய்யவும் (gsk_ என தொடங்கும்)
            _apiKey = "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";

            // Groq API URL
            _apiUrl = "https://api.groq.com/openai/v1/chat/completions";


            // ✅ புதியது (இதை சேர்க்கவும்):
            _model = "llama-3.3-70b-versatile";

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<List<string>> ExplainText(ExplainDto dto)
        {
            // Safe null/empty check
            if (dto == null || string.IsNullOrWhiteSpace(dto.text))
            {
                return new List<string> { "Input text cannot be empty." };
            }

            string prompt = "<TASK> Explain this text in exactly 10 bullet points. Each bullet point should be around 50 words. Do not include introduction or summary. Write clearly and concisely.</TASK>" +
                            "<INPUT>" + dto.text + "</INPUT>";

            var body = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0.7
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            try
            {
                response = await _httpClient.PostAsync(_apiUrl, content);
            }
            catch (HttpRequestException ex)
            {
                // Network/HTTP errors handled safely
                return new List<string> { $"Error connecting to Groq API: {ex.Message}" };
            }

            string result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Safe return for middleware
                return new List<string> { $"Groq API returned error {response.StatusCode}: {result}" };
            }

            try
            {
                using var doc = JsonDocument.Parse(result);

                if (!doc.RootElement.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)
                    return new List<string> { "No explanation generated." };

                string? text = choices[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (string.IsNullOrWhiteSpace(text))
                    return new List<string> { "No explanation generated." };

                var lines = text.Split('\n')
                                .Where(x => !string.IsNullOrWhiteSpace(x))
                                .Select(x => x.Trim('-', '•', '*', ' '))
                                .ToList();

                return lines;
            }
            catch (JsonException)
            {
                return new List<string> { "Error parsing Groq API response." };
            }
        }
    }
}
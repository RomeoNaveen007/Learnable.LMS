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

            // ❌ பழையது (இதை நீக்கவும்):
            // _model = "llama3-70b-8192";

            // ✅ புதியது (இதை சேர்க்கவும்):
            _model = "llama-3.3-70b-versatile";

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<List<string>> ExplainText(ExplainDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.text))
                throw new System.Exception("Text cannot be empty.");

            // Prompt
            string prompt = "<TASK> Explain this text in exactly 10 bullet points. Each bullet point should be around 50 words. Do not include introduction or summary. Write clearly and concisely.</TASK>" +
                            "<INPUT>" + dto.text + "</INPUT>";

            // Request Body Creation (OpenAI/Groq Format)
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

            // API Call (Key is already in Header, so just POST to URL)
            var response = await _httpClient.PostAsync(_apiUrl, content);
            var result = await response.Content.ReadAsStringAsync();

            // Error Handling
            if (!response.IsSuccessStatusCode)
            {
                throw new System.Exception($"Groq API Error: {response.StatusCode} - {result}");
            }

            // JSON Parsing (OpenAI/Groq Response Format)
            using var doc = JsonDocument.Parse(result);

            try
            {
                // OpenAI format: choices[0].message.content
                if (!doc.RootElement.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)
                {
                    return new List<string> { "No explanation generated." };
                }

                string? text = choices[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                if (string.IsNullOrEmpty(text))
                    return new List<string>();

                // Formatting the list
                var lines = text.Split('\n')
                                .Where(x => !string.IsNullOrWhiteSpace(x))
                                .Select(x => x.Trim('-', '•', '*', ' '))
                                .ToList();

                return lines;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception($"Error parsing Groq response: {ex.Message}");
            }
        }
    }
}
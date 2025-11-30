using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Services.External;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public ExplanationService(IConfiguration config)
        {
            _httpClient = new HttpClient();

            _apiKey = "AIzaSyAC7cPvh8pynRrXo6kz0pYyXebaC0SgpG8";
            _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
        }

        public async Task<List<string>> ExplainText(ExplainDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.text))
                throw new System.Exception("Text cannot be empty.");

            string prompt =
                         "<TASK> Explain this text in exactly 10 bullet points. Each bullet point should be around 50 words. Do not include introduction or summary. Write clearly and concisely.</TASK>" +
                         "<INPUT>" + dto.text + "</INPUT>";


            var body = new
            {
                contents = new[]                                                        
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}?key={_apiKey}", content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new System.Exception(result);

            using var doc = JsonDocument.Parse(result);

            string? text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            var lines = text.Split('\n')
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Select(x => x.Trim('-', '•', ' '))
                            .ToList();

           
            return lines;
        }
    }
}

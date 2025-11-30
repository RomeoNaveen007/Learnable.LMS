using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Services.External;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Learnable.Infrastructure.Implementations.Services.External
{
    public class ExamAiApiService : IExamAiApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;

        public ExamAiApiService(IConfiguration config)
        {
            _httpClient = new HttpClient();

            _apiKey = "AIzaSyAC7cPvh8pynRrXo6kz0pYyXebaC0SgpG8";
            _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
        }

        public async Task<List<ExamQuestionDto>> GenerateQuestions(List<OcrPdfDto> chunks , int questionCount)
        {
            if (chunks == null || chunks.Count == 0 || questionCount==0)
                throw new Exception("Chunk list cannot be empty.");
            

            string prompt =
                "<TASK> Using these text fragments, generate "+ questionCount + " MCQ questions. </TASK>" +
                "<FORMAT> $ question $ Answer1 $ Answer2 $ Answer3 $ Answer4 $ Answer5 $ correctIndex $ </FORMAT>" +
                "<INPUT>";

            foreach (var c in chunks)
                prompt += $"Chunk {c.ChunkId}: {c.Chunk}\n";

            prompt += "</INPUT>";

            var body = new
            {
                contents = new[]
                {
                    new {
                        parts = new[] { new { text = prompt } }
                    }
                }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}?key={_apiKey}", content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception(result);

            using var doc = JsonDocument.Parse(result);

            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            string[] dd = text.Split('$');
            var clean = dd.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            List<ExamQuestionDto> qList = new();

            for (int i = 0; i < clean.Count; i += 7)
            {
                qList.Add(new ExamQuestionDto
                {
                    Question = clean[i].Trim(),
                    Answers = new List<string>
                    {
                        clean[i + 1].Trim(),
                        clean[i + 2].Trim(),
                        clean[i + 3].Trim(),
                        clean[i + 4].Trim(),
                        clean[i + 5].Trim()
                    },
                    CorrectAnswerIndex = int.Parse(clean[i + 6].Trim())
                });
            }

            return qList;
        }
    }

}
// Read from appsettings.json
/*_apiKey = "AIzaSyAC7cPvh8pynRrXo6kz0pYyXebaC0SgpG8";
_apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
*/
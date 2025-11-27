using Learnable.Application.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Learnable.Application.Interfaces.Services.External
{
    public interface IExamAiApiService
    {    
        public class AiController : ControllerBase
        {
            private readonly HttpClient _httpClient;
            private readonly string _apiKey;
            private readonly string _apiUrl;

            public AiController(IConfiguration config)
            {
                _httpClient = new HttpClient();

                // Read from appsettings.json
                _apiKey = "AIzaSyAC7cPvh8pynRrXo6kz0pYyXebaC0SgpG8";
                _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
            }

            [HttpPost("generate-questions")]
            public async Task<IActionResult> GenerateQuestions([FromBody] List<OcrPdfDto> chunks)
            {
                if (chunks == null || chunks.Count == 0)
                    return BadRequest("Chunk list cannot be empty.");

                string prompt =
                    "<TASK> Using these text fragments, generate 10 MCQ questions. </TASK>" +
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
                    return StatusCode((int)response.StatusCode, result);

                using var doc = JsonDocument.Parse(result);
                var text = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                string[] dd = text.Split('$');
                var clean = dd.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                List<ExamQuestionDto> qList = new List<ExamQuestionDto>();

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

                return Ok(qList);
            }
        }
    }

}


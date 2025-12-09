using Learnable.Application.Common.Dtos;
using Learnable.Application.Interfaces.Services.External;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers; // புதிதாக சேர்க்கப்பட்டது
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
        private readonly string _model;

        public ExamAiApiService(IConfiguration config)
        {
            _httpClient = new HttpClient();

            // ⚠️ உங்கள் Groq API Key (முந்தைய சாட்டில் நீங்கள் கொடுத்தது)
            _apiKey = "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";

            // Groq Endpoint
            _apiUrl = "https://api.groq.com/openai/v1/chat/completions";

            // Model: Llama 3.3 70B (Complex logic மற்றும் Formatting-க்கு சிறந்தது)
            _model = "llama-3.3-70b-versatile";

            // Groq requires Bearer Token in Header
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<List<ExamQuestionDto>> GenerateQuestions(List<OcrPdfDto> chunks, int questionCount)
        {
            if (chunks == null || chunks.Count == 0 || questionCount == 0)
                throw new Exception("Chunk list cannot be empty.");

            // Prompt Construction
            string prompt =
                "<TASK> Using these text fragments, generate " + questionCount + " MCQ questions. </TASK>" +
                "<FORMAT> Strictly follow this format. Do NOT add any introduction or conclusion text. Output MUST start and end with $. \n" +
                "$ question $ Answer1 $ Answer2 $ Answer3 $ Answer4 $ Answer5 $ correctIndex (1-5) $ \n" +
                "Example: $ What is the capital of France? $ Berlin $ Madrid $ Paris $ Rome $ London $ 3 $ </FORMAT>" +
                "<INPUT>\n";

            foreach (var c in chunks)
                prompt += $"Chunk {c.ChunkId}: {c.Chunk}\n";

            prompt += "</INPUT>";

            // Request Body (OpenAI/Groq Format)
            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                // Temperature 0.3 சிறந்தது (Formatting மாறாமல் இருக்க)
                temperature = 0.3
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // API Call
            var response = await _httpClient.PostAsync(_apiUrl, content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Groq API Error: {response.StatusCode} - {result}");

            // JSON Parsing (OpenAI Format)
            using var doc = JsonDocument.Parse(result);

            // Safety Check
            if (!doc.RootElement.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)
            {
                throw new Exception("No response from Groq API");
            }

            var text = choices[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrEmpty(text))
                return new List<ExamQuestionDto>();

            // --- Parsing Logic (Existing logic preserved) ---

            string[] dd = text.Split('$');
            var clean = dd.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            List<ExamQuestionDto> qList = new();

            try
            {
                // Ensure we don't go out of bounds if AI misses a field
                for (int i = 0; i < clean.Count; i += 7)
                {
                    // Check if we have enough elements for a full question set
                    if (i + 6 >= clean.Count) break;

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
                        // TryParse பாதுகாப்பானது (AI சிலசமயம் text அனுப்பினால் crash ஆகாது)
                        CorrectAnswerIndex = int.TryParse(clean[i + 6].Trim(), out int idx) ? idx : 1
                    });
                }
            }
            catch (Exception ex)
            {
                // Parsing error வந்தால் லாக் செய்யவும் அல்லது வெறும் கிடைத்த கேள்விகளை அனுப்பவும்
                Console.WriteLine("Error parsing questions: " + ex.Message);
            }

            return qList;
        }
    }
}
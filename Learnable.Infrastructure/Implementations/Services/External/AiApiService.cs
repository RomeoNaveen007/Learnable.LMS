using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Learnable.Infrastructure.Implementations.Services.External
{
    public class AiApiService
    {
        // HttpClient used for calling external APIs
        private readonly HttpClient _httpClient;

        // Google Gemini API key
        private readonly string _apiKey = "AIzaSyATEmVTy9AekgAdbNAIu_c5KR13QFlbxzM";
        private readonly string ai_studio = "AIzaSyBLVFkkNtyFN253AJHmwHaT4Dp3s0LQ0hY";
        private readonly string google_cloud = "AIzaSyATEmVTy9AekgAdbNAIu_c5KR13QFlbxzM";
        private readonly string ai_studio_2 = "AIzaSyBRKn8-cWzVZamJJXB0WZ52yw1XCq7bSeA";
        private readonly string ai_studio_pro_version = "AIzaSyCVe7a6MqA2Ke9IvTCUlT4Nhw8KKStRwN8";

        // Gemini Flash model endpoint
        private readonly string _apiUrl =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";  

        public AiApiService()
        {
            // Initialize HttpClient
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Sends a prompt to Gemini AI and returns the response
        /// as a List<string> (each part returned as separate item).
        /// </summary>
        /// <param name="prompt">User text input</param>
        /// <returns>AI output as List<string></returns>
        public async Task<List<string>> AskAi(string prompt)
        {
            // Validate prompt
            if (string.IsNullOrWhiteSpace(prompt))
                return new List<string> { "Prompt cannot be empty." };

            // Prepare request body structure required by Gemini API
            var requestBody = new
            {
                contents = new[]
                {
                    new {
                        parts = new[]
                        {
                            new { text = prompt } // user prompt
                        }
                    }
                }
            };

            // Convert object to JSON string
            var json = JsonSerializer.Serialize(requestBody);

            // Wrap JSON as HTTP request content
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Send POST request to Gemini API
                var response = await _httpClient.PostAsync($"{_apiUrl}?key={_apiKey}", content);

                // Read the response body
                var result = await response.Content.ReadAsStringAsync();

                // If API returned an error, return the raw error as list item
                if (!response.IsSuccessStatusCode)
                    return new List<string> { result };

                // Parse the JSON response
                using var doc = JsonDocument.Parse(result);

                // Extract "parts" array containing text output
                var parts = doc.RootElement
                               .GetProperty("candidates")[0]
                               .GetProperty("content")
                               .GetProperty("parts");

                // List to store the final AI output lines
                List<string> outputList = new List<string>();

                // Loop through each part and extract text
                foreach (var part in parts.EnumerateArray())
                {
                    var text = part.GetProperty("text").GetString();

                    if (!string.IsNullOrWhiteSpace(text))
                        outputList.Add(text); // add each output string to list
                }

                // Return the list of AI outputs
                return outputList;
            }
            catch (Exception ex)
            {
                // Return error details as a list
                return new List<string> { "Error occurred", ex.Message };
            }
        }
    }
}

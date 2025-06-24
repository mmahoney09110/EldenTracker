using Microsoft.EntityFrameworkCore;
using OpenAI.Assistants;
using OpenAI.Files;
using OpenAI.VectorStores;
using System.ClientModel;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;


namespace OpenAI.Examples
{
    public class Persona
    {
        private readonly HttpClient _httpClient;
        public Persona(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("OpenAI");
        }

        public async Task<string> GenerateResponseAsync(string system, string message)
        {
            Console.WriteLine("[PERSONA] Starting GenerateResponseAsync (Chat Completion)...");

            string systemMessage = system;

            var requestBody = new
            {
                model = "gpt-4o-mini", // gpt-3.5-turbo to prioritizing speed and cost, gpt-4o-mini for better quality 
                messages = new[]
                {
            new { role = "system", content = systemMessage },
            new { role = "user", content = message }
        },
                temperature = 0.85,
                max_tokens = 1000
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "v1/chat/completions")
            {
                Content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            )
            };

            Console.WriteLine("[PERSONA] Sending Chat Completion request...");
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(request);
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                // Timeout occurred
                Console.WriteLine("[WARN] PERSONA request timed out after 30s.");
                return "My response took too long, please try again."; 
            }

            string responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"PERSONA [ERROR] OpenAI API Error: {response.StatusCode} - {responseJson}");
                return "An API error occured. If this persists please contact the owner of this project."; 
            }

            using var doc = System.Text.Json.JsonDocument.Parse(responseJson);
            string aiResponse = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "";

            // Optional: Trim, sanitize, and shorten
            aiResponse = Regex.Replace(aiResponse, "【.*?】", ""); // Remove citations if any
            aiResponse = aiResponse.Length > 1600 ? aiResponse.Substring(0, 1599) : aiResponse;

            Console.WriteLine("[PERSONA] Final response:");
            Console.WriteLine(aiResponse);

            return aiResponse.Trim();
        }


    }
}

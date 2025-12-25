using Database.Contracts;
using Domain;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AIRepository: IAIRepository
    {
        private readonly IDatabaseConfig _databaseConfig;
        private readonly HttpClient _httpClient;
        public AIRepository(IDatabaseConfig databaseConfig, HttpClient httpClient)
        {
            _databaseConfig = databaseConfig;
            _httpClient = httpClient;
        }
        public async Task<string> ClassifyMessage(MessageDTO message)
        {
            string? url = _databaseConfig.Configuration["AI_Models:ClassifyMessages"];

            if (string.IsNullOrEmpty(url))
            {
                throw new InvalidOperationException("AI Service URL is not configured");
            }

            var requestBody = new
            {
                data = new object[]
                {
                    message.Sender,
                    message.MessageContent
                }
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, requestBody);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<Gradio.Response>();

                if (result?.Data != null && result.Data.Length > 0)
                {
                    return result.Data[0]?.ToString() ?? "Unknown";
                }

                return "Unknown";
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to classify message: {ex.Message}", ex);
            }
        }

        public Task<Transaction> ExtractTransaction(Message message)
        {
            throw new NotImplementedException();
        }
    }
}

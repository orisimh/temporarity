using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace backend.Services
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;

        // Constructor to inject HttpClient
        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Method to perform a GET request
        public async Task<string> GetAsync(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Throws if not 200-299

                var content = await response.Content.ReadAsStringAsync();
                return content;

            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return $"Error: {ex.Message}";
            }
        }
        public async Task<List<T>> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<T>>(content); // Deserialize into a List of objects
        }

        // Method to perform a POST request
        public async Task<string> PostAsync<T>(string url, T data)
        {
            try
            {
                var jsonData = JsonSerializer.Serialize(data);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode(); // Throws if not 200-299

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return $"Error: {ex.Message}";
            }
        }
    }
}

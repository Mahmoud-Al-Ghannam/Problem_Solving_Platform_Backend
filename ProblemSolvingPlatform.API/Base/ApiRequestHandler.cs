using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Base {
    public class ApiRequestHandler {
        private readonly HttpClient _httpClient;

        public ApiRequestHandler(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint) {
            return await _httpClient.GetAsync(endpoint);
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, object content) {
            var jsonContent = JsonConvert.SerializeObject(content);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(endpoint, httpContent);
        }

        public async Task<HttpResponseMessage> PutAsync(string endpoint, object content) {
            var jsonContent = JsonConvert.SerializeObject(content);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync(endpoint, httpContent);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint) {
            return await _httpClient.DeleteAsync(endpoint);
        }
    }
}

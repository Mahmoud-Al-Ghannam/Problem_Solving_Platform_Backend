using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Base {
    public class ApiRequestHandler {
        private readonly HttpClient _httpClient;

        public ApiRequestHandler() {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string url) {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,url);
            httpRequest.Headers.Add("Accept", "application/json"); 
            return await _httpClient.SendAsync(httpRequest);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, object content) {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Headers.Add("Accept", "application/json");
            var jsonContent = JsonConvert.SerializeObject(content);
            httpRequest.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await _httpClient.SendAsync(httpRequest);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, object content) {
            var httpRequest = new HttpRequestMessage(HttpMethod.Put, url);
            httpRequest.Headers.Add("Accept", "application/json");
            var jsonContent = JsonConvert.SerializeObject(content);
            httpRequest.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await _httpClient.SendAsync(httpRequest);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url) {
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
            httpRequest.Headers.Add("Accept", "application/json");
            return await _httpClient.SendAsync(httpRequest);
        }
    }
}

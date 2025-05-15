using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Base {
    public class BaseApiService {
        private readonly ApiRequestHandler _apiRequestHandler;
        public BaseApiService(ApiRequestHandler apiRequestHandler) {
            _apiRequestHandler = apiRequestHandler;
        }

        public BaseApiService(HttpClient httpClient) {
            _apiRequestHandler = new ApiRequestHandler(httpClient);
        }

        public async Task<T?> GetAsync<T>(string endpoint) {
            var response = await GetAsync(endpoint);
            return await ApiResponseHandler.ParseResponse<T>(response);
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint) {
            var response = await _apiRequestHandler.GetAsync(endpoint);
            await ApiResponseHandler.HandleResponse(response);
            return response;
        }

        public async Task<T?> PostAsync<T>(string endpoint, object content) {
            var response = await PostAsync(endpoint, content);
            return await ApiResponseHandler.ParseResponse<T>(response);
        }

        public async Task<HttpResponseMessage> PostAsync (string endpoint, object content) {
            var response = await _apiRequestHandler.PostAsync(endpoint, content);
            await ApiResponseHandler.HandleResponse(response);
            return response;
        }

        public async Task<T?> PutAsync<T>(string endpoint, object content) {
            var response = await PutAsync(endpoint, content);
            return await ApiResponseHandler.ParseResponse<T>(response);
        }

        public async Task<HttpResponseMessage> PutAsync(string endpoint, object content) {
            var response = await _apiRequestHandler.PutAsync(endpoint, content);
            await ApiResponseHandler.HandleResponse(response);
            return response;
        }

        public async Task<T?> DeleteAsync<T>(string endpoint) {
            var response = await DeleteAsync(endpoint);
            return await ApiResponseHandler.ParseResponse<T>(response);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint) {
            var response = await _apiRequestHandler.DeleteAsync(endpoint);
            await ApiResponseHandler.HandleResponse(response);
            return response;
        }
    }
}

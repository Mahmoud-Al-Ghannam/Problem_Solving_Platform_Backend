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

        public BaseApiService() {
            _apiRequestHandler = new ApiRequestHandler();
        }

        public async Task<T?> GetAsync<T>(string url) {
            var response = await GetAsync(url);
            return await ApiResponseHandler.ParseResponse<T>(response);
        }

        public async Task<HttpResponseMessage> GetAsync(string url) {
            var response = await _apiRequestHandler.GetAsync(url);
            await ApiResponseHandler.HandleResponse(response);
            return response;
        }

        public async Task<T?> PostAsync<T>(string url, object content) {
            var response = await PostAsync(url, content);
            return await ApiResponseHandler.ParseResponse<T>(response);
        }

        public async Task<HttpResponseMessage> PostAsync (string url, object content,string acceptContentType = "application/json") {
            var response = await _apiRequestHandler.PostAsync(url, content,acceptContentType);
            await ApiResponseHandler.HandleResponse(response);
            return response;
        }

        public async Task<T?> PutAsync<T>(string url, object content) {
            var response = await PutAsync(url, content);
            return await ApiResponseHandler.ParseResponse<T>(response);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, object content) {
            var response = await _apiRequestHandler.PutAsync(url, content);
            await ApiResponseHandler.HandleResponse(response);
            return response;
        }

        public async Task<T?> DeleteAsync<T>(string url) {
            var response = await DeleteAsync(url);
            return await ApiResponseHandler.ParseResponse<T>(response);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url) {
            var response = await _apiRequestHandler.DeleteAsync(url);
            await ApiResponseHandler.HandleResponse(response);
            return response;
        }
    }
}

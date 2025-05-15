using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Base {
    public class ApiResponseHandler {
        public async static Task HandleResponse(HttpResponseMessage response) {
            string jsonResponse = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != System.Net.HttpStatusCode.OK &&
                response.StatusCode != System.Net.HttpStatusCode.Created &&
                response.StatusCode != System.Net.HttpStatusCode.NoContent) {
                throw new ApiException((int)response.StatusCode, jsonResponse);
            }
        }

        public static async Task<T?> ParseResponse<T>(HttpResponseMessage response) {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }
}

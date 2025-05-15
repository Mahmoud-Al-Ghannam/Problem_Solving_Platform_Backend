using ProblemSolvingPlatform.API.Base;
using ProblemSolvingPlatform.API.Compiler.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Compiler.Services {
    public class CompilerApiService : BaseApiService,ICompilerService {

        public CompilerApiService(HttpClient httpClient) : base(httpClient) {}

        public static string BaseAddress = "https://godbolt.org";

        static readonly List<string> CompilerIDs = new List<string>() {
            "clang900"
        };

        public enum enCompilers {
            cppClang900 = 0
        }

        public static class Endpoints {
            public static string compile(string compilerID) {
                return $"/api/compiler/{compilerID}/compile";
            }
        }

        public async Task<CompileResponseDTO> CompileAsync(CompileRequestDTO request) {
            CompileResponseDTO endResponse = new CompileResponseDTO();
            string compilerID = CompilerIDs[(int)request.compiler];
            string endpoint = Endpoints.compile(compilerID);
            object content = new {
                source = request.source,
                options = new {
                    compilerOptions = new {
                        executorRequest = true
                    },
                    executeParameters = new {
                        stdin = request.input
                    }
                }
            };
            var response = await PostAsync(endpoint, content);
            string responseJson = await response.Content.ReadAsStringAsync();

            string s1 = "Standard out:";
            int i1 = responseJson.IndexOf(s1);
            endResponse.standardOut = i1 == -1? null : responseJson.Substring(i1 + s1.Length);

            string s2 = "Standard error:";
            int i2 = responseJson.IndexOf(s2);
            endResponse.standardError = i2 == -1 ? null : responseJson.Substring(i2 + s2.Length);

            return endResponse;
        }


    }   
}

using Newtonsoft.Json;
using ProblemSolvingPlatform.API.Base;
using ProblemSolvingPlatform.API.Compiler.DTOs;
using ProblemSolvingPlatform.API.Compiler.Utils;
using ProblemSolvingPlatform.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ProblemSolvingPlatform.API.Compiler.Services {
    public class CompilerApiService : BaseApiService, ICompilerApiService {

        public static class Endpoints {
            private static string _BaseAddress => "https://godbolt.org";
            public static string compile(string compilerID) {
                return $"{_BaseAddress}/api/compiler/{compilerID}/compile";
            }
        }

        public async Task<List<CompileResponseDTO>> CompileAsync(CompileRequestDTO request) {
            List<CompileResponseDTO> endResponses = new List<CompileResponseDTO>();
            string url = Endpoints.compile(request.Compiler);

            foreach (string input in request.Inputs ?? []) {
                CompileResponseDTO compileResponse = new CompileResponseDTO();

                object content = new {
                    source = request.Source,
                    options = new {
                        compilerOptions = new {
                            executorRequest = true
                        },
                        executeParameters = new {
                            stdin = input
                        }
                    }
                };

                var response = await PostAsync(url, content);
                string responseJson = await response.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(responseJson) ?? "";
                compileResponse.Output = string.Join("\n", ((IEnumerable<dynamic>)responseObject.stdout).Select(obj => (string?)obj.text).Where(obj => obj != null));
                compileResponse.CompilationErrors = ((IEnumerable<dynamic>)responseObject.buildResult.stderr).Select(obj => (string?)obj.text).Where(obj => obj != null).ToList()!;
                compileResponse.ExecutionErrors = ((IEnumerable<dynamic>)responseObject.stderr).Select(obj => (string?)obj.text).Where(obj => obj != null).ToList()!;
                compileResponse.ExecutionTimeMs = responseObject.execTime;
                compileResponse.Timeout = compileResponse.ExecutionTimeMs == null ? false : compileResponse.ExecutionTimeMs > request.TimeoutMs;
                compileResponse.ExecutionCode = responseObject.code;
                compileResponse.CompilationCode = responseObject.buildResult.code;


                endResponses.Add(compileResponse);

                if (!compileResponse.CompilationSuccess) break;
            }
            return endResponses;
        }

        public List<CompilerDTO> GetAllCompilers() {
            return CompilerUtils.GetAllCompilers();
        }

        public async Task<string> SimpleCompileAsync(SimpleCompileRequestDTO request) {
            string url = Endpoints.compile(request.Compiler);

            object content = new {
                source = request.Source,
                options = new {
                    compilerOptions = new {
                        executorRequest = true
                    },
                    executeParameters = new {
                        stdin = request.Input
                    }
                }
            };

            var response = await PostAsync(url, content, "application/text");
            string responseJson = await response.Content.ReadAsStringAsync();
            
            return responseJson;
        }
    }
}

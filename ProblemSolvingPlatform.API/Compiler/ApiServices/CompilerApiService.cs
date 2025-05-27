using Newtonsoft.Json;
using ProblemSolvingPlatform.API.Base;
using ProblemSolvingPlatform.API.Compiler.DTOs;
using ProblemSolvingPlatform.API.Compiler.Utils;
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
                compileResponse.Output = ((IEnumerable<dynamic>)responseObject.stdout).Select(obj => (string?)obj.text).Where(obj => obj != null).FirstOrDefault("");
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


        public async Task<ExecuteResponseDTO> ExecuteCodeAsync(ExecuteRequestDTO request) {
            var fullUri = Endpoints.compile(request.Compiler);

            // body :)
            var content = new {
                source = request.Source,
                compiler = request.Compiler,
                options = new {
                    userArguments = "-O3",
                    executeParameters = new {
                        args = new[] { "arg1", "arg2" },
                        stdin = request.input,
                        runtimeTools = new[] {
                            new {
                                name    = "env",
                                options = new[] {
                                    new { name = "MYENV", value = "123" }
                                }
                            }
                        }
                    },
                    compilerOptions = new { executorRequest = true },
                    filters = new { execute = true },
                    tools = Array.Empty<object>(),
                    libraries = new[] {
                        new { id = "openssl", version = "111c" }
                    }
                },
                lang = "c++",
                allowStoreCodeDebug = true
            };


            try {
                // serialize 
                var contentJSON = JsonSerializer.Serialize(content);
                using var httpRequest = new HttpRequestMessage(HttpMethod.Post, fullUri) {
                    Content = new StringContent(contentJSON, Encoding.UTF8, "application/json")
                };
                httpRequest.Headers.Accept.Clear();
                httpRequest.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // sending 
                using var httpClient = new HttpClient();
                var httpResponse = await httpClient.SendAsync(httpRequest);
                httpResponse.EnsureSuccessStatusCode();

                var responseJson = await httpResponse.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseJson);
                var root = doc.RootElement;


                var stdout = root.GetProperty("stdout")
                                 .EnumerateArray()
                                 .Select(e => e.GetProperty("text").GetString())
                                 .Where(s => s != null);
                var stderr = root.GetProperty("stderr")
                                 .EnumerateArray()
                                 .Select(e => e.GetProperty("text").GetString())
                                 .Where(s => s != null);
                var executionTimeInMS = root.GetProperty("execTime").GetDouble();

                return new ExecuteResponseDTO {
                    StandardOut = string.Join('\n', stdout),
                    StandardError = string.Join('\n', stderr),
                    ExecutionTimeMs = executionTimeInMS,
                    MemoryKB = 0,
                    ExitCode = root.GetProperty("code").GetInt32()
                };
            }
            catch (Exception ex) {
                return new ExecuteResponseDTO {
                    StandardOut = null,
                    StandardError = $"Exception: {ex.Message}",
                    ExecutionTimeMs = 0,
                    MemoryKB = 0,
                    ExitCode = -1
                };
            }
        }

    }
}

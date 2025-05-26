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
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ProblemSolvingPlatform.API.Compiler.Services {
    public class CompilerApiService : BaseApiService, ICompilerApiService {

        public static string BaseAddress = "https://godbolt.org";

        public CompilerApiService() : base(new HttpClient() { BaseAddress = new Uri(CompilerApiService.BaseAddress) }) {

        }


        public static class Endpoints {
            public static string compile(string compilerID) {
                return $"/api/compiler/{compilerID}/compile";
            }
        }

        public async Task<List<CompileResponseDTO>> CompileAsync(CompileRequestDTO request) {
            List<CompileResponseDTO> endResponses = new List<CompileResponseDTO>();
            string endpoint = Endpoints.compile(request.compiler);

            foreach (string input in request.inputs) {
                CompileResponseDTO compileResponse = new CompileResponseDTO();

                object content = new {
                    source = request.source,
                    options = new {
                        compilerOptions = new {
                            executorRequest = true
                        },
                        executeParameters = new {
                            stdin = input
                        }
                    }
                };

                try {
                    var response = await PostAsync(endpoint, content);
                    string responseJson = await response.Content.ReadAsStringAsync();


                    string s1 = "Standard out:";
                    int i1 = responseJson.IndexOf(s1);
                    compileResponse.standardOut = i1 == -1 ? null : responseJson.Substring(i1 + s1.Length);

                    string s2 = "Standard error:";
                    int i2 = responseJson.IndexOf(s2);
                    compileResponse.standardError = i2 == -1 ? null : responseJson.Substring(i2 + s2.Length);
                }
                catch (Exception ex) {
                    compileResponse.standardOut = null;
                    compileResponse.standardError = ex.ToString();
                }

                endResponses.Add(compileResponse);
            }
            return endResponses;
        }

        public List<CompilerDTO> GetAllCompilers() {
            return CompilerUtils.GetAllCompilers();
        }


        public async Task<ExecuteResponseDTO> ExecuteCodeAsync(ExecuteRequestDTO request)
        {
            var baseUri = new Uri(BaseAddress);                                 
            var endpoint = Endpoints.compile(request.Compiler); 
            var fullUri = new Uri(baseUri, endpoint);

            // body :)
            var content = new
            {
                source = request.Source,
                compiler = request.Compiler,
                options = new
                {
                    userArguments = "-O3",
                    executeParameters = new
                    {
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


            try
            {
                // serialize 
                var contentJSON = JsonSerializer.Serialize(content);
                using var httpRequest = new HttpRequestMessage(HttpMethod.Post, fullUri)
                {
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

                return new ExecuteResponseDTO
                {
                    StandardOut = string.Join('\n', stdout),
                    StandardError = string.Join('\n', stderr),
                    ExecutionTimeMs = executionTimeInMS,
                    MemoryKB = 0,                           
                    ExitCode = root.GetProperty("code").GetInt32()
                };
            }
            catch (Exception ex)
            {
                return new ExecuteResponseDTO
                {
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

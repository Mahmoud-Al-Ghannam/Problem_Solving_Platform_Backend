using ProblemSolvingPlatform.API.Base;
using ProblemSolvingPlatform.API.Compiler.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Compiler.Services {
    public class CompilerApiService : BaseApiService, ICompilerApiService {

        public static string BaseAddress = "https://godbolt.org";

        public CompilerApiService() : base(new HttpClient() { BaseAddress = new Uri(CompilerApiService.BaseAddress) }) {

        }

        static readonly List<CompilerDTO> Compilers = new List<CompilerDTO>() {
            new CompilerDTO() {
                Language = "c++",
                CompilerName = "clang1810",
                Name = "x86-64 clang 18.1.0"
            },
            new CompilerDTO() {
                Language = "c++",
                CompilerName = "g132",
                Name = "x86-64 gcc 13.2"
            },
            new CompilerDTO() {
                Language = "c",
                CompilerName = "cclang1810",
                Name = "x86-64 clang 18.1.0"
            },
            new CompilerDTO() {
                Language = "c",
                CompilerName = "cg132",
                Name = "x86-64 gcc 13.2"
            },
            new CompilerDTO() {
                Language = "python",
                CompilerName = "python312",
                Name = "Python 3.12"
            },
             new CompilerDTO() {
                Language = "java",
                CompilerName = "java2102",
                Name = "jdk 21.0.2"
            },
        };

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
            return Compilers;
        }
    }
}

using ProblemSolvingPlatform.API.Compiler.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Compiler.Services {
    public class CompilerService : ICompilerService {

        static readonly List<string> CompilerIDs = new List<string>() {
            "clang900"
        };

        public enum Compilers {
            cppClang900 = 0
        }

        public static class Endpoints {
            public static string compile(string compilerID) {
                return $"https://godbolt.org/api/compiler/{compilerID}/compile";
            }
        }

        public Task<CompileResponseDTO> CompileAsync(CompileRequestDTO request) {
            throw new NotImplementedException();
            string compilerID = CompilerIDs[(int)Compilers.cppClang900];
            string endpoint = Endpoints.compile(compilerID);
        }
    }   
}

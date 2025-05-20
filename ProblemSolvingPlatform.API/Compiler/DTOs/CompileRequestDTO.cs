using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.API.Compiler.Services.CompilerApiService;

namespace ProblemSolvingPlatform.API.Compiler.DTOs {
    public class CompileRequestDTO {
        public string source {  get; set; }
        public List<string>? inputs {  get; set; }
        public string compiler { get; set; }
        public CompileRequestDTO() { }
    }
}

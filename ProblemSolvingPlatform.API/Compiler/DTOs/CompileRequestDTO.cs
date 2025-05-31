using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.API.Compiler.Services.CompilerApiService;

namespace ProblemSolvingPlatform.API.DTOs {
    public class CompileRequestDTO {
        public string Source {  get; set; }
        public List<string>? Inputs {  get; set; }
        public string Compiler { get; set; }
        public double TimeoutMs { get; set; } = 20000;
    }
}

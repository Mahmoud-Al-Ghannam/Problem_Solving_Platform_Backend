using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.API.Compiler.Services.CompilerApiService;

namespace ProblemSolvingPlatform.API.Compiler.DTOs {
    public class CompileRequestDTO {
        public string source {  get; set; }
        public string? input {  get; set; }
        public enCompilers compiler { get; set; }
        public CompileRequestDTO() { }
    }
}

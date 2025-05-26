using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Compiler.DTOs {
    public class CompileResponseDTO {
        public string? standardOut {  get; set; }
        public string? standardError {  get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Compiler.DTOs {
    public class SimpleCompileRequestDTO {
        public string Source { get; set; }
        public string? Input { get; set; }
        public string Compiler { get; set; }
    }
}

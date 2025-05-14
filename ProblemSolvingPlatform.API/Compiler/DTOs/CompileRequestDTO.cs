using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Compiler.DTOs {
    public class CompileRequestDTO {
        public string source {  get; set; }
        public string target { get; set; }
        public CompileRequestDTO() { }
    }
}

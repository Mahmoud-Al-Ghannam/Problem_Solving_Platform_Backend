using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.DTOs {
    public class CompileResponseDTO {
        public string? Output { get; set; }
        public IEnumerable<string>? CompilationErrors { get; set; }
        public IEnumerable<string>? ExecutionErrors { get; set; }
        public double? ExecutionTimeMs { get; set; }
        public int CompilationCode { get; set; }
        public int ExecutionCode { get; set; }
        public bool CompilationSuccess => CompilationCode == 0;
        public bool ExecutionSuccess => ExecutionCode == 0;
        public bool Timeout { get; set; }
    }
}

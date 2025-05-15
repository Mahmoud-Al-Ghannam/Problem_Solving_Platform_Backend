using ProblemSolvingPlatform.API.Compiler.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Compiler.Services {
    public interface ICompilerService {
        public Task<CompileResponseDTO> CompileAsync (CompileRequestDTO request); 
    }
}

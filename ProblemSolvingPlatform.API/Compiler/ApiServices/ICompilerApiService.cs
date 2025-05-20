using ProblemSolvingPlatform.API.Compiler.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Compiler.Services {
    public interface ICompilerApiService {
        public Task<List<CompileResponseDTO>> CompileAsync (CompileRequestDTO request); 
    }
}

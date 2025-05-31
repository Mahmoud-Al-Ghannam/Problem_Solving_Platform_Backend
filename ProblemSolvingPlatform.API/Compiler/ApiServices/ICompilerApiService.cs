using ProblemSolvingPlatform.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.API.Compiler.Services.CompilerApiService;

namespace ProblemSolvingPlatform.API.Compiler.Services {
    public interface ICompilerApiService {
        
        public Task<List<CompileResponseDTO>> CompileAsync(CompileRequestDTO request);
        public List<CompilerDTO> GetAllCompilers();

    }
}

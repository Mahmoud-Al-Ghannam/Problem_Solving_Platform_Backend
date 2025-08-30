using ProblemSolvingPlatform.API.Compiler.DTOs;
using ProblemSolvingPlatform.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Compiler {
    public interface ICompilerService {
        public Task<List<CompileResponseDTO>> CompileAsync(CompileRequestDTO request);
        public Task<string> SimpleCompileAsync(SimpleCompileRequestDTO request);
        public List<CompilerDTO> GetAllCompilers();
    }
}

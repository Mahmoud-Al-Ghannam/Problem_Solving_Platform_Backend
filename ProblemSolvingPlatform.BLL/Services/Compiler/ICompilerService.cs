using ProblemSolvingPlatform.API.Compiler.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Compiler {
    public interface ICompilerService {
        public Task<List<CompileResponseDTO>> CompileAsync(CompileRequestDTO request);
        public List<CompilerDTO> GetAllCompilers();
    }
}

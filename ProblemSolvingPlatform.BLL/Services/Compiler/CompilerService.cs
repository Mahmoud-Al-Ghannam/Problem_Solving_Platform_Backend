using ProblemSolvingPlatform.API.Compiler.DTOs;
using ProblemSolvingPlatform.API.Compiler.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Compiler {
    public class CompilerService : ICompilerService {

        private readonly ICompilerApiService _compilerApiService;

        public CompilerService(ICompilerApiService compilerApiService) {
            _compilerApiService = compilerApiService;
        }

        public async Task<List<CompileResponseDTO>> CompileAsync(CompileRequestDTO request) {
            return await _compilerApiService.CompileAsync(request);
        }
    }
}

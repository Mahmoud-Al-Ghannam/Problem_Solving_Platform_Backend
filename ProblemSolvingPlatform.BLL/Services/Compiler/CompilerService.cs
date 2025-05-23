﻿using ProblemSolvingPlatform.API.Compiler.DTOs;
using ProblemSolvingPlatform.API.Compiler.Services;
using ProblemSolvingPlatform.BLL.Exceptions;
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
            var compilers = GetAllCompilers();
            if (!compilers.Any(c => c.CompilerName == request.compiler))
                throw new CustomValidationException("compiler",[$"The compiler with name = {request.compiler} was not found."]);

            return await _compilerApiService.CompileAsync(request);
        }

        public List<CompilerDTO> GetAllCompilers() {
            return _compilerApiService.GetAllCompilers();
        }
    }
}

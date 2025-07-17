using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.API.DTOs;
using ProblemSolvingPlatform.BLL.Services.Compiler;
using ProblemSolvingPlatform.BLL.Services.Users;

namespace ProblemSolvingPlatform.Controllers {

    [ApiController]
    [Route("api/compilers")]

    public class CompilersController : GeneralController {

        private ICompilerService _compilerService { get; }
        public CompilersController(ICompilerService compilerService) {
            _compilerService = compilerService;
        }

        [HttpPost("compile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CompileResponseDTO>>> CompileAsync(CompileRequestDTO request) {
            return Ok(await _compilerService.CompileAsync(request));
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CompilerDTO>> GetAllCompilers() {
            return Ok(_compilerService.GetAllCompilers());
        }
    }
}

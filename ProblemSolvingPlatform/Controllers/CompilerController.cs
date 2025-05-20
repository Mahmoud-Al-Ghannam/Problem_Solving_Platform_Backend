using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.API.Compiler.DTOs;
using ProblemSolvingPlatform.BLL.Services.Compiler;
using ProblemSolvingPlatform.BLL.Services.User;

namespace ProblemSolvingPlatform.Controllers {

    [ApiController]
    [Route("api/compilers")]
    public class CompilerController : Controller {

        private ICompilerService _compilerService { get; }
        public CompilerController(ICompilerService compilerService) {
            _compilerService = compilerService;
        }

        [HttpPost("compile")]
        public async Task<ActionResult<IEnumerable<CompileResponseDTO>>> CompileAsync(CompileRequestDTO request) {
            try {
                return await _compilerService.CompileAsync(request);
            }
            catch (Exception ex) { 
                return StatusCode(StatusCodes.Status500InternalServerError,new {error=ex.Message});
            }
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<CompilerDTO>> GetAllCompilers() {
            try {
                return _compilerService.GetAllCompilers();
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
    }
}

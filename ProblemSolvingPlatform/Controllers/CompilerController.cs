using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.API.Compiler.DTOs;
using ProblemSolvingPlatform.API.Compiler.Services;

namespace ProblemSolvingPlatform.Controllers {

    [ApiController]
    [Route("api/compiler")]
    public class CompilerController : Controller {

        [HttpPost("compile")]
        public async Task<ActionResult<CompileResponseDTO>> CompileAsync(CompileRequestDTO request) {
            try {
                var compiler = new CompilerApiService(new HttpClient() { BaseAddress = new Uri(CompilerApiService.BaseAddress) });
                return await compiler.CompileAsync(request);
            }
            catch (Exception ex) { 
                return StatusCode(StatusCodes.Status500InternalServerError,new {error=ex.Message});
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.API.DTOs;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.Services.Compiler;
using ProblemSolvingPlatform.BLL.Services.Users;

namespace ProblemSolvingPlatform.Controllers.Public
{

    [ApiController]
    [Route($"/{Constants.Api.PrefixPublicApi}/compilers")]

    public class CompilersController : GeneralController
    {

        private ICompilerService _compilerService { get; }
        public CompilersController(ICompilerService compilerService)
        {
            _compilerService = compilerService;
        }


        /// <summary>
        /// No Auth
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("compile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CompileResponseDTO>>> CompileAsync(CompileRequestDTO request)
        {
            return Ok(await _compilerService.CompileAsync(request));
        }


        /// <summary>
        /// No Auth
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CompilerDTO>> GetAllCompilers()
        {
            return Ok(_compilerService.GetAllCompilers());
        }
    }
}

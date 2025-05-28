using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Services.Problems;

namespace ProblemSolvingPlatform.Controllers {

    [ApiController]
    [Route("api/problems")]
    public class ProblemsController : Controller {

        private readonly IProblemService _problemService;

        public ProblemsController(IProblemService problemService) {
            _problemService = problemService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int?>> AddProblem([FromBody] NewProblemDTO newProblemDTO) {
            int? id = null;

            id = await _problemService.AddProblemAsync(newProblemDTO);
            if (id == null) StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(id);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProblemDTO?>> GetProblemByID([FromRoute(Name = "id")] int problemID) {
            var problemDTO = await _problemService.GetProblemByIDAsync(problemID);
            if (problemDTO == null) StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(problemDTO);
        }
    }
}

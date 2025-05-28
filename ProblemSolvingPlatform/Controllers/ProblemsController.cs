using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Services.Problem;

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
            try {
                id = await _problemService.AddProblemAsync(newProblemDTO);
                if (id == null) StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch (CustomValidationException ex) {
                return BadRequest(ex.errors);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
            return Ok(id);
        }
    }
}

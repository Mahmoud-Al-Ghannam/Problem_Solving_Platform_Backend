using AuthHelper;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int?>> AddProblem([FromBody] NewProblemDTO newProblemDTO) {
            int? id = null;
            int userID = AuthUtils.GetUserId(User)??-1;
            id = await _problemService.AddProblemAsync(newProblemDTO,userID);
            if (id == null) StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(id);
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProblem([FromBody] UpdateProblemDTO updateProblemDTO) {
            bool ok;
            int userID = AuthUtils.GetUserId(User) ?? -1;
            ok = await _problemService.UpdateProblemAsync(updateProblemDTO, userID);
            if (!ok) StatusCode(StatusCodes.Status500InternalServerError);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProblem([FromRoute(Name = "id")] int problemID) {
            bool ok;
            int userID = AuthUtils.GetUserId(User) ?? -1;
            ok = await _problemService.DeleteProblemByIDAsync(problemID, userID);
            if (!ok) StatusCode(StatusCodes.Status500InternalServerError);
            return NoContent();
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

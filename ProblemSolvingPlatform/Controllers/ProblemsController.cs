using AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Services.Problems;
using ProblemSolvingPlatform.Responses;
using System.ComponentModel.DataAnnotations;

namespace ProblemSolvingPlatform.Controllers {

    [ApiController]
    [Route("api/problems")]
    public class ProblemsController : GeneralController {

        private readonly IProblemService _problemService;

        public ProblemsController(IProblemService problemService) {
            _problemService = problemService;
        }

        //[Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<int?>> AddProblem([FromBody] NewProblemDTO newProblemDTO) {
            int? id = null;
            int userID = AuthUtils.GetUserId(User)??-1;
            id = await _problemService.AddProblemAsync(newProblemDTO,userID);
            if (id == null) return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(id);
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProblem([FromBody] UpdateProblemDTO updateProblemDTO) {
            bool ok;
            int userID = AuthUtils.GetUserId(User) ?? -1;
            ok = await _problemService.UpdateProblemAsync(updateProblemDTO, userID);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProblem([FromRoute(Name = "id")] int problemID) {
            bool ok;
            int userID = AuthUtils.GetUserId(User) ?? -1;
            ok = await _problemService.DeleteProblemByIDAsync(problemID, userID);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError);
            return NoContent();
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProblemDTO?>> GetProblemByID([FromRoute(Name = "id")] int problemID) {
            var problemDTO = await _problemService.GetProblemByIDAsync(problemID);
            if (problemDTO == null) return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(problemDTO);
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ShortProblemDTO>?>> GetAllProblems([FromQuery] int page=1, [FromQuery]int limit=20, [FromQuery] string? title = null,[FromQuery] byte? difficulty = null, [FromQuery] int? createdBy = null, [FromQuery] byte? role = null, [FromQuery] DateTime? createdAt = null, [FromBody] IEnumerable<int>? tagIDs = null) {
            var problems = await _problemService.GetAllProblemsAsync(page,limit,title,difficulty,createdBy,role,createdAt,tagIDs);
            if(problems == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(problems);
        }


    }
}

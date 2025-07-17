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

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int?>> AddProblem([FromBody] NewProblemDTO newProblemDTO) {
            int? id = null;
            int? userID = AuthUtils.GetUserId(User);
            if (userID == null)
                return Unauthorized(BLL.Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

            id = await _problemService.AddProblemAsync(newProblemDTO,userID.Value);
            if (id == null) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
            return Ok(id);
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateProblem([FromBody] UpdateProblemDTO updateProblemDTO) {
            bool ok;
            int? userID = AuthUtils.GetUserId(User);
            if (userID == null)
                return Unauthorized(BLL.Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

            ok = await _problemService.UpdateProblemAsync(updateProblemDTO, userID.Value);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProblem([FromRoute(Name = "id")] int problemID)  {
            bool ok;
            int? userID = AuthUtils.GetUserId(User);
            if (userID == null)
                return Unauthorized(BLL.Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);
            ok = await _problemService.DeleteProblemByIDAsync(problemID, userID.Value);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
            return NoContent();
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProblemDTO?>> GetProblemByID([FromRoute(Name = "id")] int problemID) {
            var problemDTO = await _problemService.GetProblemByIDAsync(problemID);
            if (problemDTO == null) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
            return Ok(problemDTO);
        }

        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ShortProblemDTO>?>> GetAllProblems([FromQuery] int page= BLL.Constants.PaginationDefaultValues.Page, [FromQuery]int limit= BLL.Constants.PaginationDefaultValues.Limit, [FromQuery] string? title = null,[FromQuery] byte? difficulty = null, [FromQuery] int? createdBy = null, [FromQuery] byte? role = null, [FromQuery] DateTime? createdAt = null, [FromBody] IEnumerable<int>? tagIDs = null) {
            var problems = await _problemService.GetAllProblemsAsync(page,limit,title,difficulty,createdBy,role,createdAt,tagIDs);
            if(problems == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
            return Ok(problems);
        }


    }
}

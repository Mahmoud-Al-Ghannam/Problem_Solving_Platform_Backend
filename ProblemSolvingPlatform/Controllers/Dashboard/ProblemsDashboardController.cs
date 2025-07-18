using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.BLL.Services.Problems;
using ProblemSolvingPlatform.Responses;

namespace ProblemSolvingPlatform.Controllers.Dashboard {

    [ApiController]
    [Route($"/{Constants.Api.PrefixDashboardApi}/problems")]
    public class ProblemsDashboardController : DashboardController {

        private readonly IProblemService _problemService;

        public ProblemsDashboardController(IProblemService problemService) {
            _problemService = problemService;
        }

        /// <summary>
        /// JWt Bearer Auth With System Role
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="title"></param>
        /// <param name="difficulty"></param>
        /// <param name="createdBy"></param>
        /// <param name="IsSystemProblem"></param>
        /// <param name="createdAt"></param>
        /// <param name="tagIDs"></param>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ShortProblemDTO>?>> GetAllProblems([FromQuery] int page = Constants.PaginationDefaultValues.Page, [FromQuery] int limit = Constants.PaginationDefaultValues.Limit, [FromQuery] string? title = null, [FromQuery] byte? difficulty = null, [FromQuery] int? createdBy = null, [FromQuery] bool? IsSystemProblem = null, [FromQuery] DateTime? createdAt = null,[FromQuery] bool? isDeleted = null, [FromQuery] string? tagIDs = null) {
            var problems = await _problemService.GetAllProblemsAsync(page, limit, title, difficulty, createdBy, IsSystemProblem, createdAt, isDeleted, tagIDs);
            if (problems == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return Ok(problems);
        }
    }
}

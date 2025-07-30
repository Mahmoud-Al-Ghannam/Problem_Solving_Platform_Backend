using AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Services.Problems;
using ProblemSolvingPlatform.Responses;
using System.ComponentModel.DataAnnotations;

namespace ProblemSolvingPlatform.Controllers
{

    [ApiController]
    [Route($"/{Constants.Api.PrefixApi}/problems")]
    public class ProblemsController : GeneralController
    {

        private readonly IProblemService _problemService;

        public ProblemsController(IProblemService problemService)
        {
            _problemService = problemService;
        }


        /// <summary>
        /// JWt Bearer Auth
        /// </summary>
        /// <param name="newProblemDTO"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int?>> AddProblem([FromBody] NewProblemDTO newProblemDTO)
        {
            int? id = null;
            int? userID = AuthUtils.GetUserId(User);
            if (userID == null)
                return Unauthorized(Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

            bool isSystem = User.IsInRole(Constants.Roles.System);

            id = await _problemService.AddProblemAsync(newProblemDTO, userID.Value, isSystem);
            if (id == null) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return Ok(id);
        }


        /// <summary>
        /// JWt Bearer Auth
        /// </summary>
        /// <param name="updateProblemDTO"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateProblem([FromBody] UpdateProblemDTO updateProblemDTO)
        {
            bool ok;
            int? userID = AuthUtils.GetUserId(User);
            if (userID == null)
                return Unauthorized(Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

            ok = await _problemService.UpdateProblemAsync(updateProblemDTO, userID.Value);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return NoContent();
        }


        /// <summary>
        /// JWt Bearer Auth
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProblem([FromRoute(Name = "id")] int problemID)
        {
            bool ok;
            int? userID = AuthUtils.GetUserId(User);
            if (userID == null)
                return Unauthorized(Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);
            ok = await _problemService.DeleteProblemByIDAsync(problemID, userID.Value);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return NoContent();
        }


        /// <summary>
        /// No Auth
        /// </summary>
        /// <param name="problemID"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProblemDTO?>> GetProblemByID([FromRoute(Name = "id")] int problemID)
        {
            var problemDTO = await _problemService.GetProblemByIDAsync(problemID);
            if (problemDTO == null) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return Ok(problemDTO);
        }


        /// <summary>
        /// No Auth
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
        public async Task<ActionResult<PageDTO<ShortProblemDTO>?>> GetAllProblems([FromQuery] int page = Constants.PaginationDefaultValues.Page, [FromQuery] int limit = Constants.PaginationDefaultValues.Limit, [FromQuery] string? title = null, [FromQuery] byte? difficulty = null, [FromQuery] int? createdBy = null, [FromQuery] bool? IsSystemProblem = null, [FromQuery] DateTime? createdAt = null, [FromQuery] string? tagIDs = null)
        {
            var problems = await _problemService.GetAllProblemsAsync(page, limit, title, difficulty, createdBy, IsSystemProblem, createdAt, false, tagIDs);
            if (problems == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return Ok(problems);
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
        /// <param name="isDeleted"></param>
        /// <param name="tagIDs"></param>
        /// <returns></returns>
        [HttpGet("dashboard")]
        [Authorize(Roles = Constants.Roles.System)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PageDTO<ShortProblemDTO>?>> GetAllProblems([FromQuery] int page = Constants.PaginationDefaultValues.Page, [FromQuery] int limit = Constants.PaginationDefaultValues.Limit, [FromQuery] string? title = null, [FromQuery] byte? difficulty = null, [FromQuery] int? createdBy = null, [FromQuery] bool? IsSystemProblem = null, [FromQuery] DateTime? createdAt = null, [FromQuery] bool? isDeleted = null, [FromQuery] string? tagIDs = null)
        {
            var pageDTO = await _problemService.GetAllProblemsAsync(page, limit, title, difficulty, createdBy, IsSystemProblem, createdAt, isDeleted, tagIDs);
            if (pageDTO == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return Ok(pageDTO);
        }

    }
}

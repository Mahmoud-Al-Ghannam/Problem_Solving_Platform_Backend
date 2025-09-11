using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.Services.RequestApiLogs;
using ProblemSolvingPlatform.Responses;
using Microsoft.AspNetCore.Authorization;

namespace ProblemSolvingPlatform.Controllers {

    [ApiController]
    [Route($"/{Constants.Api.PrefixApi}/request-api-logs")]
    public class RequestApiLogsController : ControllerBase {

        private readonly IRequestApiLogService _requestApiLogService;

        public RequestApiLogsController(IRequestApiLogService requestApiLogService) {
            _requestApiLogService = requestApiLogService;
        }


        /// <summary>
        /// JWt Bearer Auth With System Role
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="username"></param>
        /// <param name="requestType"></param>
        /// <param name="statusCode"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        [HttpGet("dashboard")]
        [Authorize(Roles = Constants.Roles.System)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PageDTO<ShortProblemDTO>?>> GetAllProblems([FromQuery] int page = Constants.PaginationDefaultValues.Page, [FromQuery] int limit = Constants.PaginationDefaultValues.Limit, [FromQuery] string? username = null, [FromQuery] string? requestType = null, [FromQuery] int? statusCode = null, [FromQuery] string? endpoint = null) {
            var logs = await _requestApiLogService.GetAllLogsAsync(page, limit,username,requestType,statusCode,endpoint);
            if (logs == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return Ok(logs);
        }

    }
}

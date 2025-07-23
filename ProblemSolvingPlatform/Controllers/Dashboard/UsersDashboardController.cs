using AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs.UserProfile;
using ProblemSolvingPlatform.BLL.Services.Users;
using ProblemSolvingPlatform.Responses;
using System.ComponentModel.DataAnnotations;

namespace ProblemSolvingPlatform.Controllers.Dashboard {

    [ApiController]
    [Route($"/{Constants.Api.PrefixDashboardApi}/users")]
    public class UsersDashboardController : DashboardController {

        private readonly IUserService _userService;

        public UsersDashboardController(IUserService userService) {
            _userService = userService;
        }


        /// <summary>
        /// JWt Bearer Auth With System Role
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpPut("activation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateUserInfo([FromQuery][Required] int userID,[FromQuery][Required] bool isActive) {
            int? id = AuthUtils.GetUserId(User);
            if (id == null) return StatusCode(StatusCodes.Status401Unauthorized, new ErrorResponseBody(Constants.ErrorMessages.JwtDoesnotIncludeSomeFields));
            
            var isUpdated = await _userService.UpdateUserActivationAsync(userID, isActive,id.Value);
            if (!isUpdated)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return NoContent();
        }


        /// <summary>
        /// JWt Bearer Auth With System Role
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="username"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers([FromQuery] int page = Constants.PaginationDefaultValues.Page, [FromQuery] int limit = Constants.PaginationDefaultValues.Limit, [FromQuery] string? username = null, [FromQuery] bool? isActive = null) {
            var users = await _userService.GetAllUsersAsync(page, limit, username, isActive);
            if (users == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return Ok(users);
        }
    }
}

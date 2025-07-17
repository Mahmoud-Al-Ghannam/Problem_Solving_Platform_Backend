using AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensibility;
using ProblemSolvingPlatform.BLL.DTOs.UserProfile;
using ProblemSolvingPlatform.BLL.Services.Users;
using ProblemSolvingPlatform.Responses;
using System.Security.Claims;

namespace ProblemSolvingPlatform.Controllers;

[ApiController]
[Route("api/profiles")]
public class ProfilesController : GeneralController {
    private IUserService _userService { get; }
    public ProfilesController(IUserService userService) {
        _userService = userService;
    }


    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDTO>> GetUserInfo(int userId) {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
        return Ok(user);
    }

    [Authorize]
    [HttpPut("")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateUserInfo([FromForm] UpdateUserDTO updateUser) {
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return Unauthorized(BLL.Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

        var isUpdated = await _userService.UpdateUserInfoByIdAsync(userId.Value, updateUser);
        if (!isUpdated)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
        return NoContent();
    }

    // get all users (without token)
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers([FromQuery] int page = BLL.Constants.PaginationDefaultValues.Page, [FromQuery] int limit = BLL.Constants.PaginationDefaultValues.Limit, [FromQuery] string? username = null) {
        var users = await _userService.GetAllUsersAsync(page, limit, username);
        if (users == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
        return Ok(users);
    }
}

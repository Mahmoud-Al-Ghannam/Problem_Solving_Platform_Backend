using AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensibility;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs.UserProfile;
using ProblemSolvingPlatform.BLL.Services.Users;
using ProblemSolvingPlatform.Responses;
using System.Security.Claims;

namespace ProblemSolvingPlatform.Controllers.Public;

[ApiController]
[Route($"/{Constants.Api.PrefixPublicApi}/profiles")]
public class ProfilesController : GeneralController
{
    private IUserService _userService { get; }
    public ProfilesController(IUserService userService)
    {
        _userService = userService;
    }


    /// <summary>
    /// No Auth
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDTO>> GetUserInfo(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return Ok(user);
    }


    /// <summary>
    /// JWt Bearer Auth
    /// </summary>
    /// <param name="updateUser"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut("")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateUserInfo([FromForm] UpdateUserDTO updateUser)
    {
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return Unauthorized(Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

        var isUpdated = await _userService.UpdateUserInfoByIdAsync(userId.Value, updateUser);
        if (!isUpdated)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return NoContent();
    }

    /// <summary>
    /// No Auth
    /// </summary>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers([FromQuery] int page = Constants.PaginationDefaultValues.Page, [FromQuery] int limit = Constants.PaginationDefaultValues.Limit, [FromQuery] string? username = null)
    {
        var users = await _userService.GetAllUsersAsync(page, limit, username,true);
        if (users == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return Ok(users);
    }
}

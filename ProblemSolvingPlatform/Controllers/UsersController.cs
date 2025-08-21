using AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensibility;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Users;
using ProblemSolvingPlatform.BLL.Services.Users;
using ProblemSolvingPlatform.Responses;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ProblemSolvingPlatform.Controllers;

[ApiController]
[Route($"/{Constants.Api.PrefixApi}/users")]
public class UsersController : GeneralController
{
    private IUserService _userService { get; }
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }


    /// <summary>
    /// No Auth
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("id/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDTO>> GetUserInfoByID(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return Ok(user);
    }


    /// <summary>
    /// No Auth
    /// </summary>
    /// <param name="useranme"></param>
    /// <returns></returns>
    [HttpGet("username/{useranme}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDTO>> GetUserInfoByUsername(string useranme) {
        var user = await _userService.GetUserByUsernameAsync(useranme);
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
    public async Task<ActionResult<PageDTO<UserDTO>>> GetAllUsers([FromQuery] int page = Constants.PaginationDefaultValues.Page, [FromQuery] int limit = Constants.PaginationDefaultValues.Limit, [FromQuery] string? username = null)
    {
        var users = await _userService.GetAllUsersAsync(page, limit, username, true);
        if (users == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return Ok(users);
    }


    /// <summary>
    /// JWt Bearer Auth With System Role
    /// </summary>
    /// <param name="userID"></param>
    /// <param name="isActive"></param>
    /// <returns></returns>
    [HttpPut("activation")]
    [Authorize(Roles = Constants.Roles.System)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateUserInfo([FromQuery][Required] int userID, [FromQuery][Required] bool isActive)
    {
        int? id = AuthUtils.GetUserId(User);
        if (id == null) return StatusCode(StatusCodes.Status401Unauthorized, new ErrorResponseBody(Constants.ErrorMessages.JwtDoesnotIncludeSomeFields));

        var isUpdated = await _userService.UpdateUserActivationAsync(userID, isActive, id.Value);
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
    [HttpGet("dashboard")]
    [Authorize(Roles = Constants.Roles.System)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageDTO<UserDTO>>> GetAllUsers([FromQuery] int page = Constants.PaginationDefaultValues.Page, [FromQuery] int limit = Constants.PaginationDefaultValues.Limit, [FromQuery] string? username = null, [FromQuery] bool? isActive = null)
    {
        var users = await _userService.GetAllUsersAsync(page, limit, username, isActive);
        if (users == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return Ok(users);
    }
}

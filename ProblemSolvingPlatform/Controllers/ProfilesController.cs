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
    public ProfilesController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserInfo(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "User Not Found :)" });
        return Ok(user);
    }

    [Authorize]
    [HttpPut("")]
    public async Task<IActionResult> UpdateUserInfo([FromForm] UpdateUserDTO updateUser)
    {
        var userId = AuthUtils.GetUserId(User);
        if(userId == null)
            return Unauthorized("Invalid Token");

        var isUpdated = await _userService.UpdateUserInfoByIdAsync(userId.Value, updateUser);
        if (isUpdated)
            return Ok(new { message = "user info updated successfully" } );
        else return BadRequest( new { message = "Failed to update user info :)" } );
    }

    // get all users (without token)
    [HttpGet("")]
    public async Task<IActionResult> GetAllUsers([FromQuery] int page = BLL.Constants.PaginationDefaultValues.Page, [FromQuery] int limit = BLL.Constants.PaginationDefaultValues.Limit, [FromQuery] string? username = null)
    {
        if (page <= 0 || limit <= 0 || limit > 100)
            return BadRequest("Page must be ≥ 1 and limit between 1–100");

        var users = await _userService.GetAllUsersWithFiltersAsync(page, limit, username);
        if (users == null)
            return NotFound(new { message = "Users not found :| " });
        return Ok(users);
    }
}

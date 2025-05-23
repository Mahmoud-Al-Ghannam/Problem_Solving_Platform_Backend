﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensibility;
using ProblemSolvingPlatform.BLL.DTOs.UserProfile;
using ProblemSolvingPlatform.BLL.Services.User;
using System.Security.Claims;

namespace ProblemSolvingPlatform.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private IUserService _userService { get; }
    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserInfo([FromQuery] int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "User Not Found :)" });
        return Ok(user);
    }

    [Authorize]
    [HttpPut("update-info")]
    public async Task<IActionResult> UpdateUserInfo([FromForm] UpdateUserDTO updateUser)
    {
        var ClaimUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(ClaimUserId, out int userId))
            return Unauthorized("Invalid Token");

        var isUpdated = await _userService.UpdateUserInfoByIdAsync(userId, updateUser);
        if (isUpdated)
            return Ok(new { message = "user info updated successfully" } );
        else return BadRequest( new { message = "Failed to update user info :)" } );
    }

    // get all users (without token)
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int limit = 2, [FromQuery] string? username = null)
    {
        var users = await _userService.GetAllUsersWithFiltersAsync(page, limit, username);
        if (users == null)
            return NotFound(new { message = "Users not found :| " });
        return Ok(users);
    }
}

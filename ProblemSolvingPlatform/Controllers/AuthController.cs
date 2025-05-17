using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Request;
using ProblemSolvingPlatform.BLL.Services.Auth;
using ProblemSolvingPlatform.DAL.DTOs.Auth.Request;
using System.Security.Claims;

namespace ProblemSolvingPlatform.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private IAuthService _authService { get; }
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDTO loginDTO)
    {
        var result = await _authService.LoginAsync(loginDTO);
        if (result.Success)
            return StatusCode(result.StatusCode, new { result.Message, result.Token });
        else
            return StatusCode(result.StatusCode, new { result.Message });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm] RegisterRequestDTO registerDTO)
    {
        var result = await _authService.RegisterAsync(registerDTO);
        if (result.Success)
            return StatusCode(result.StatusCode, new { message = result.Message, token = result.Token });
        else
            return StatusCode(result.StatusCode, new { message = result.Message });
    }


    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
    {
        var ClaimUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(ClaimUserId, out int userId))
            return Unauthorized("Invalid Token");

        var isChanged = await _authService.ChangePasswordAsync(userId, changePasswordDTO);
        if (isChanged)
            return Ok(new { message = "Password updated successfully" });
        return BadRequest(new { message = "Invalid current password or request .. Please try again :)" });
    }

}
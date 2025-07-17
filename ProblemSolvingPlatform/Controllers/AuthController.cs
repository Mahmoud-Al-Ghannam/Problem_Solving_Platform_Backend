using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Request;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Response;
using ProblemSolvingPlatform.BLL.Services.Auth;
using ProblemSolvingPlatform.DAL.DTOs.Auth.Request;
using ProblemSolvingPlatform.Responses;
using System.Security.Claims;

namespace ProblemSolvingPlatform.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController : GeneralController {
    private IAuthService _authService { get; }
    public AuthController(IAuthService authService) {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<string>> LoginAsync([FromBody] LoginRequestDTO loginDTO) {
        return Ok(await _authService.LoginAsync(loginDTO));
    }



    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RegisterResponseDTO>> RegisterAsync([FromForm] RegisterRequestDTO registerDTO) {
        return Ok(await _authService.RegisterAsync(registerDTO));
    }


    [Authorize]
    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO) {
        var ClaimUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(ClaimUserId, out int userId))
            return Unauthorized(Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

        var isChanged = await _authService.ChangePasswordAsync(userId, changePasswordDTO);
        if (!isChanged)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return NoContent();
    }

}
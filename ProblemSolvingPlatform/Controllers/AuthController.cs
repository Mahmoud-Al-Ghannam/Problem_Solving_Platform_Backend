using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Request;
using ProblemSolvingPlatform.BLL.Services.Auth;

namespace ProblemSolvingPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDTO registerDTO)
    {
        var result = await _authService.RegisterAsync(registerDTO);
        if (result.IsSuccess)
            return StatusCode(result.statusCode, new { result.message });
        else
            return StatusCode(result.statusCode, new { result.message });
    }
}
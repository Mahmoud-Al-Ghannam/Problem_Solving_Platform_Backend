using AuthHelper;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Submissions;
using ProblemSolvingPlatform.BLL.Services.Submissions;
using System.Security.Claims;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubmissionsController : ControllerBase
{
    private ISubmissionsService _submissionService { get; }
    public SubmissionsController(ISubmissionsService submissionsService)
    {
        _submissionService = submissionsService;
    }


    [HttpPost("/submit")]
    [Authorize]
    public async Task<IActionResult> Submit(SubmitDTO submitDTO)
    {
        var userId = AuthUtils.GetUserId(User);
        if(userId == null)
            return Unauthorized("User ID not found");

        var response = await _submissionService.Submit(submitDTO, userId.Value);
        if (response.isSuccess)
            return Ok(new { submissionId = response.submissionId, message = response.msg }); 

        return BadRequest(new {message =  response.msg});
    }

    [HttpPut("change-visionScope")]
    [Authorize]
    public async Task<IActionResult> ChangeVisionScope(int submissionId, int visionScopeId)
    {
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return Unauthorized("User ID not found");

        var isUpdated = await _submissionService.ChangeVisionScope(submissionId, visionScopeId, userId.Value);
        if (isUpdated)
            return Ok("DONE");
        return BadRequest("Failed");
    }

    [HttpGet("visionScopes")]
    public IActionResult GetAllVisionScopes()
    {
        var result = new List<object>();

        foreach (VisionScope visionScope in Enum.GetValues(typeof(VisionScope)))
        {
            result.Add(new
            {
                name = visionScope.ToString(),
                value = (int)visionScope
            }
            );
        }

        return Ok(result);
    }


}

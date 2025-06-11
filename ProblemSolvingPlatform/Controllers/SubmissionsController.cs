using AuthHelper;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submit;
using ProblemSolvingPlatform.BLL.Services.Submissions;
using System.Security.Claims;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.Controllers;

[ApiController]
[Route("api/submissions")]
public class SubmissionsController : ControllerBase
{
    private ISubmissionService _submissionService { get; }
    public SubmissionsController(ISubmissionService submissionsService)
    {
        _submissionService = submissionsService;
    }


    [HttpPost("submit")]
    [Authorize]
    public async Task<IActionResult> Submit(SubmitDTO submitDTO)
    {
        var userId = AuthUtils.GetUserId(User);
        if(userId == null)
            return Unauthorized("User ID not found");
        
        var response = await _submissionService.Submit(submitDTO, userId.Value);
        if (response.isSuccess)
            return Ok(new { message = response.msg }); 

        return BadRequest(new {message =  response.msg});
    }

    [HttpPut("change-vision-scope")]
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


    [HttpGet("vision-scopes")]
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



    [HttpGet("")]
    [Authorize]
    public async Task<IActionResult> GetSubmissions(int problemId, int page = 1, int limit = 10,VisionScope scope = VisionScope.all) 
    {
        if (page <= 0 || limit <= 0 || limit > 100)   
            return BadRequest("Page must be ≥ 1 and limit between 1–100");
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return BadRequest("no user found");

        var submissions = await _submissionService.GetAllSubmissions(userId.Value, page, limit, problemId, scope);
        if (submissions == null)
            return NotFound(new { message = "No submissions Exist" });
        return Ok(submissions);
    }
    
    
    
    [HttpGet("{submissionId}")]
    public async Task<IActionResult> GetSubmissionDetails(int submissionId)
    {
        var userId = AuthUtils.GetUserId(User);
        var subDetails = await _submissionService.GetSubmissionDetails(submissionId, userId);
        if (subDetails == null)
            return BadRequest(new { message = "Failed to view submission :)" });
        return Ok(subDetails);
    }


}

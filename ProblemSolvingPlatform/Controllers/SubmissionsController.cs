using AuthHelper;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.VisualBasic;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submit;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.VisionScope;
using ProblemSolvingPlatform.BLL.Services.Submissions;
using ProblemSolvingPlatform.Responses;
using System.Security.Claims;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.Controllers;

[ApiController]
[Route("api/submissions")]
public class SubmissionsController : GeneralController {
    private ISubmissionService _submissionService { get; }
    public SubmissionsController(ISubmissionService submissionsService)
    {
        _submissionService = submissionsService;
    }


    [HttpPost("submit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<int?>> AddNewSubmission(SubmitDTO submitDTO)
    {
        var userId = AuthUtils.GetUserId(User);
        if(userId == null)
            return Unauthorized(BLL.Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);
        
        int? submissionID = await _submissionService.AddNewSubmission(submitDTO, userId.Value);
        if (submissionID == null)
            return StatusCode(StatusCodes.Status500InternalServerError,new { error = BLL.Constants.ErrorMessages.General }); 

        return Ok(submissionID.Value);
    }

    [HttpPut("change-vision-scope")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> ChangeVisionScope(int submissionId, int visionScopeId)
    {
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return Unauthorized(BLL.Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

        var isUpdated = await _submissionService.ChangeVisionScope(submissionId, visionScopeId, userId.Value);
        if (!isUpdated) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
        return NoContent();
    }


    [HttpGet("vision-scopes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<VisionScopesDTO> GetAllVisionScopes()
    {
        var result = new List<VisionScopesDTO>();

        foreach (VisionScope visionScope in Enum.GetValues(typeof(VisionScope)))
        {
            result.Add(new VisionScopesDTO {
                VisionScope = visionScope.ToString(),
                Id = (int)visionScope
            }
            );
        }

        return Ok(result);
    }



    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<IEnumerable<SubmissionDTO>>> GetAllSubmissions(int page= BLL.Constants.PaginationDefaultValues.Page, int limit= BLL.Constants.PaginationDefaultValues.Limit, int? userId = null,int? problemId = null, VisionScope? scope = null) 
    {
        var submissions = await _submissionService.GetAllSubmissions(page, limit,AuthUtils.GetUserId(User),userId, problemId, scope);
        if (submissions == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
        return Ok(submissions);
    }


    [HttpGet("own")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<IEnumerable<SubmissionDTO>>> GetAllOwnSubmissions(int page = BLL.Constants.PaginationDefaultValues.Page, int limit = BLL.Constants.PaginationDefaultValues.Limit, int? problemId = null, VisionScope? scope = null) {
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return Unauthorized(BLL.Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

        var submissions = await _submissionService.GetAllSubmissions(page, limit, AuthUtils.GetUserId(User), userId.Value, problemId, scope);
        if (submissions == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
        return Ok(submissions);
    }



    [HttpGet("details/{submissionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DetailedSubmissionDTO>> GetSubmissionDetails(int submissionId)
    {
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return Unauthorized(BLL.Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

        var subDetails = await _submissionService.GetDetailedSubmissionByID(submissionId, userId);
        if (subDetails == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
        return Ok(subDetails);
    }

    [HttpGet("{submissionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SubmissionDTO>> GetSubmissionByID(int submissionId) {
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return Unauthorized(BLL.Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

        var submission = await _submissionService.GetSubmissionByID(submissionId, userId);
        if (submission == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
        return Ok(submission);
    }


}

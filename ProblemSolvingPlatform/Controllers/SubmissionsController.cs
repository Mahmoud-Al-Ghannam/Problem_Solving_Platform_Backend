using AuthHelper;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submit;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.VisionScope;
using ProblemSolvingPlatform.BLL.Services.Submissions;
using ProblemSolvingPlatform.Responses;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.Controllers;

[ApiController]
[Route($"/{Constants.Api.PrefixApi}/submissions")]
public class SubmissionsController : GeneralController
{
    private ISubmissionService _submissionService { get; }
    public SubmissionsController(ISubmissionService submissionsService)
    {
        _submissionService = submissionsService;
    }


    /// <summary>
    /// JWt Bearer Auth
    /// </summary>
    /// <param name="submitDTO"></param>
    /// <returns></returns>
    [HttpPost("submit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<int?>> AddNewSubmission(SubmitDTO submitDTO)
    {
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return Unauthorized(Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

        int? submissionID = await _submissionService.AddNewSubmission(submitDTO, userId.Value);
        if (submissionID == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = Constants.ErrorMessages.General });

        return Ok(submissionID.Value);
    }


    /// <summary>
    /// JWt Bearer Auth
    /// </summary>
    /// <param name="submissionId"></param>
    /// <param name="visionScopeId"></param>
    /// <returns></returns>
    [HttpPut("change-vision-scope")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> ChangeVisionScope([FromQuery][Required] int submissionId, [FromQuery][Required] int visionScopeId)
    {
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return Unauthorized(Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

        var isUpdated = await _submissionService.ChangeVisionScope(submissionId, visionScopeId, userId.Value);
        if (!isUpdated) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return NoContent();
    }


    /// <summary>
    /// No Auth
    /// </summary>
    /// <returns></returns>
    [HttpGet("vision-scopes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<VisionScopesDTO> GetAllVisionScopes()
    {
        var result = new List<VisionScopesDTO>();

        foreach (VisionScope visionScope in Enum.GetValues(typeof(VisionScope)))
        {
            result.Add(new VisionScopesDTO
            {
                VisionScope = visionScope.ToString(),
                ID = (int)visionScope
            }
            );
        }

        return Ok(result);
    }


    /// <summary>
    /// JWt Bearer Auth
    /// </summary>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <param name="userId"></param>
    /// <param name="problemId"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<PageDTO<SubmissionDTO>>> GetAllSubmissions([FromQuery] int page = Constants.PaginationDefaultValues.Page, [FromQuery] int limit = Constants.PaginationDefaultValues.Limit, [FromQuery] int? userId = null, [FromQuery] int? problemId = null, [FromQuery] VisionScope? scope = null)
    {
        var submissions = await _submissionService.GetAllSubmissions(page, limit, AuthUtils.GetUserId(User), userId, problemId, scope);
        if (submissions == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return Ok(submissions);
    }


    /// <summary>
    /// JWt Bearer Auth
    /// </summary>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <param name="problemId"></param>
    /// <param name="scope"></param>
    /// <returns></returns>
    [HttpGet("own")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<PageDTO<SubmissionDTO>>> GetAllOwnSubmissions([FromQuery] int page = Constants.PaginationDefaultValues.Page, [FromQuery] int limit = Constants.PaginationDefaultValues.Limit, [FromQuery] int? problemId = null, [FromQuery] VisionScope? scope = null)
    {
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return Unauthorized(Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

        var submissions = await _submissionService.GetAllSubmissions(page, limit, AuthUtils.GetUserId(User), userId.Value, problemId, scope);
        if (submissions == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return Ok(submissions);
    }


    /// <summary>
    /// JWt Bearer Auth
    /// </summary>
    /// <param name="submissionId"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("details/{submissionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DetailedSubmissionDTO>> GetSubmissionDetails(int submissionId)
    {
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return Unauthorized(Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

        var subDetails = await _submissionService.GetDetailedSubmissionByID(submissionId, userId);
        if (subDetails == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return Ok(subDetails);
    }


    /// <summary>
    /// JWt Bearer Auth
    /// </summary>
    /// <param name="submissionId"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("{submissionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SubmissionDTO>> GetSubmissionByID(int submissionId)
    {
        var userId = AuthUtils.GetUserId(User);
        if (userId == null)
            return Unauthorized(Constants.ErrorMessages.JwtDoesnotIncludeSomeFields);

        var submission = await _submissionService.GetSubmissionByID(submissionId, userId);
        if (submission == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return Ok(submission);
    }


}

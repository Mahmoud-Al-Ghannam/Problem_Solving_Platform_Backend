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
public class SubmissionsController : GeneralController {
    private ISubmissionService _submissionService { get; }
    public SubmissionsController(ISubmissionService submissionsService) {
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
    public async Task<ActionResult<int?>> AddNewSubmission(SubmitDTO submitDTO) {
        int userId = AuthUtils.GetUserId(User)!.Value;
        int? submissionID = await _submissionService.AddNewSubmission(submitDTO, userId);
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
    public async Task<IActionResult> ChangeVisionScope([FromQuery][Required] int submissionId, [FromQuery][Required] int visionScopeId) {
        int userId = AuthUtils.GetUserId(User)!.Value;
        var isUpdated = await _submissionService.ChangeVisionScope(submissionId, visionScopeId, userId);
        if (!isUpdated) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return NoContent();
    }


    /// <summary>
    /// No Auth
    /// </summary>
    /// <returns></returns>
    [HttpGet("vision-scopes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<VisionScopesDTO> GetAllVisionScopes() {
        var result = new List<VisionScopesDTO>();

        foreach (VisionScope visionScope in Enum.GetValues(typeof(VisionScope))) {
            result.Add(new VisionScopesDTO {
                VisionScope = visionScope.ToString(),
                ID = (int)visionScope
            }
            );
        }

        return Ok(result);
    }


    /// <summary>
    /// No Auth , JWT Bearer Auth , JWT Bearer Auth With System Role
    /// </summary>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <param name="userId"></param>
    /// <param name="problemId"></param>
    /// <param name="scope">
    /// When request this api without token , we will replace scope with 'all'.
    /// 
    /// When request this api with token for user is not owner of submissions , we will replace scope with 'all' for only submissions that don't belong to user (owner of token).
    ///
    /// Otherwise , we don't change scope which you put
    /// </param>
    /// <returns></returns>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageDTO<ShortSubmissionDTO>>> GetAllSubmissions([FromQuery] int page = Constants.PaginationDefaultValues.Page, [FromQuery] int limit = Constants.PaginationDefaultValues.Limit, [FromQuery] int? userId = null, [FromQuery] int? problemId = null, [FromQuery] VisionScope? scope = null) {
        PageDTO<ShortSubmissionDTO>? pageDTO;
        if (User.Identity != null && User.Identity.IsAuthenticated) {
            if (User.IsInRole(Constants.Roles.System))
                pageDTO = await _submissionService.GetAllSubmissions(page, limit, userId, problemId, scope);
            else if (userId != null) {
                if (AuthUtils.GetUserId(User) == userId.Value)
                    pageDTO = await _submissionService.GetAllSubmissions(page, limit, userId, problemId, scope);
                else 
                    pageDTO = await _submissionService.GetAllSubmissions(page, limit, userId, problemId, Enums.VisionScope.all);
            } else {
                pageDTO = await _submissionService.GetAllSubmissions(page, limit, userId, problemId, scope);
                if (pageDTO != null) pageDTO.Items = pageDTO.Items.Where(s => s.UserID == AuthUtils.GetUserId(User) || (s.UserID != AuthUtils.GetUserId(User) && s.VisionScope == VisionScope.all)).ToList(); 
            }
        }
        else {
            pageDTO = await _submissionService.GetAllSubmissions(page, limit, userId, problemId, Enums.VisionScope.all);
        }
        if (pageDTO == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return Ok(pageDTO);
    }


    /// <summary>
    /// JWt Bearer Auth
    /// </summary>
    /// <param name="submissionId"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("details/{submissionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DetailedSubmissionDTO>> GetSubmissionDetails(int submissionId) {
        int userId = AuthUtils.GetUserId(User)!.Value; 

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
    public async Task<ActionResult<SubmissionDTO>> GetSubmissionByID(int submissionId) {
        int userId = AuthUtils.GetUserId(User)!.Value;
        var submission = await _submissionService.GetSubmissionByID(submissionId, userId);
        if (submission == null)
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
        return Ok(submission);
    }


}

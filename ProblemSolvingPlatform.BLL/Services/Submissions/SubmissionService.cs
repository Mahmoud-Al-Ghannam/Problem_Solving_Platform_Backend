using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.API.Compiler.Utils;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submit;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.VisionScope;
using ProblemSolvingPlatform.BLL.Services.Submissions.Handling_Submission;
using ProblemSolvingPlatform.DAL.Models.Submissions;
using ProblemSolvingPlatform.DAL.Repos;
using ProblemSolvingPlatform.DAL.Repos.Problems;
using ProblemSolvingPlatform.DAL.Repos.Submissions;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.BLL.Services.Submissions;

public class SubmissionService : ISubmissionService
{
    private ISubmissionRepo _submissionsRepo { get; }
    private IProblemRepo _problemRepo { get; }
    private SubmissionHandler _submissionHandler { get; }
    private ISubmissionTestRepo _submissionTestRepo { get; }
    public SubmissionService(ISubmissionRepo submissionsRepo, IProblemRepo problemRepo, SubmissionHandler submissionHandler,
        ISubmissionTestRepo submissionTestRepo)
    {
        _submissionsRepo = submissionsRepo;
        _problemRepo = problemRepo;
        _submissionHandler = submissionHandler;
        _submissionTestRepo = submissionTestRepo;
    }

    // general submission 
    public async Task<SubmitResponseDTO> Submit(SubmitDTO submitDTO, int userId)
    {
        if (!await _problemRepo.DoesProblemExistByIDAsync(submitDTO.ProblemId))
            return new SubmitResponseDTO()
            {
                isSuccess = false,
                msg = "The problem not found :)"
            };

        
        var submission = new Submission()
        {
            UserID = userId,
            ProblemID = submitDTO.ProblemId,
            CompilerName = submitDTO.CompilerName,
            Code = submitDTO.Code,
            VisionScope = submitDTO.VisionScope
        };
        var submissionId = await _submissionsRepo.AddNewSubmission(submission);
        if (submissionId == null)
            return new SubmitResponseDTO()
            {
                isSuccess = true,
                msg = "Failed to submit a solution",
            };
        var resultMessage = await _submissionHandler.ExecuteSubmission(submissionId.Value, submitDTO.Code, submitDTO.ProblemId, submitDTO.CompilerName);

        return new SubmitResponseDTO()
        {
            isSuccess = true,
            msg = resultMessage,
        };

    }

    public List<VisionScopesDTO> GetAllVisionScopes()
    {
        var result = new List<VisionScopesDTO>();

        foreach (VisionScope visionScope in Enum.GetValues(typeof(VisionScope)))
        {
            result.Add(new VisionScopesDTO()
            {
                VisionScope = visionScope.ToString(),
                Id = (int)visionScope
            }
            );
        }
        return result;
    }

    
    public async Task<bool> ChangeVisionScope(int submissionId, int visionScopeId, int userId)
       => await _submissionsRepo.ChangeVisionScope(submissionId, visionScopeId, userId);



    private async Task<bool> UserCanViewSubmission(int submissionId, int? userId)
    {
        var submissionAccessInfo = await _submissionsRepo.GetSubmissionAccessInfo(submissionId);

        if (submissionAccessInfo == null) return false;

        return (userId != null && submissionAccessInfo.Value.userId == userId)
               || (submissionAccessInfo.Value.visionScope == (byte)Enums.VisionScope.all);
    }
  
    public async Task<SubmissionDetailsDTO?> GetSubmissionDetails(int submissionId, int? userId)
    {
        if (!await UserCanViewSubmission(submissionId, userId))
               return null;


        var submissionDetails = new SubmissionDetailsDTO();
        
        var code = await _submissionsRepo.GetSubmissionCode(submissionId);
        var submissionsTestCases = await _submissionTestRepo.GetAllSubmissionTestCases(submissionId);

        List<SubmissionTestCaseDTO> submissionTests = new();
        if(submissionsTestCases != null)
        {
            foreach(var test in submissionsTestCases)
            {
                submissionTests.Add(new SubmissionTestCaseDTO()
                {
                    TestCaseID = test.TestCaseID,
                    ExecutionTimeMilliseconds = test.ExecutionTimeMilliseconds,
                    Status = ((Enums.SubmissionStatus)(test.Status)).ToString()
                });
            }
        }

        return new SubmissionDetailsDTO()
        {
            Code = code ?? "",
            SubmissionsTestCases = submissionTests
        };
    }


    public async Task<List<SubmissionDTO>?> GetAllSubmissions(int userId, int page, int limit, int? problemId, Enums.VisionScope scope)
    {
        var submissions = await _submissionsRepo.GetSubmissions(userId,  page, limit, problemId, (byte)scope);
        if (submissions == null)
            return null;

        var submissionsLST = new List<SubmissionDTO>();
        foreach (var submission in submissions)
        {
            submissionsLST.Add(new SubmissionDTO()
            {
                CompilerName = submission.CompilerName,
                ExecutionTimeMilliseconds = submission.ExecutionTimeMilliseconds,
                Status = ((Enums.SubmissionStatus)(submission.Status)).ToString(),
                SubmissionId = submission.SubmissionId,
                SubmittedDate = submission.SubmittedAt,
                UserID = userId,
                VisionScope = ((Enums.VisionScope)(submission.VisionScope)).ToString()
            });
        }
        return submissionsLST;
    }

}


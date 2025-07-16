using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.API.Compiler.Services;
using ProblemSolvingPlatform.API.Compiler.Utils;
using ProblemSolvingPlatform.API.DTOs;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submit;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.VisionScope;
using ProblemSolvingPlatform.BLL.Helpers;
using ProblemSolvingPlatform.DAL.Models.Submissions;
using ProblemSolvingPlatform.DAL.Models.SubmissionTestCase;
using ProblemSolvingPlatform.DAL.Repos;
using ProblemSolvingPlatform.DAL.Repos.Problems;
using ProblemSolvingPlatform.DAL.Repos.Submissions;
using ProblemSolvingPlatform.DAL.Repos.Tests;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.BLL.Services.Submissions;

public class SubmissionService : ISubmissionService {
    private ISubmissionRepo _submissionsRepo { get; }
    private IProblemRepo _problemRepo { get; }

    private ICompilerApiService _compilerApiService { get; }
    private ITestCaseRepo _testCaseRepo { get; }
    private ISubmissionTestRepo _submissionTestRepo { get; }
    public SubmissionService(ISubmissionRepo submissionsRepo, IProblemRepo problemRepo,
        ISubmissionTestRepo submissionTestRepo, ITestCaseRepo testCaseRepo, ICompilerApiService compilerApiService) {
        _submissionsRepo = submissionsRepo;
        _problemRepo = problemRepo;
        _submissionTestRepo = submissionTestRepo;
        _testCaseRepo = testCaseRepo;
        _compilerApiService = compilerApiService;
    }

    // general submission 
    public async Task<SubmitResponseDTO> Submit(SubmitDTO submitDTO, int userId) {
        if (!await _problemRepo.DoesProblemExistByIDAsync(submitDTO.ProblemId))
            return new SubmitResponseDTO() {
                isSuccess = false,
                msg = "The problem not found :)"
            };


        var testCases = (await _testCaseRepo.GetTestCasesByProblemIdAsync(submitDTO.ProblemId))?.ToList();
        var problem = await _problemRepo.GetProblemByIDAsync(submitDTO.ProblemId);

        if (testCases == null || problem == null) return new SubmitResponseDTO() {
            isSuccess = false,
            msg = "There is an error"
        };

        var compileResults = await _compilerApiService.CompileAsync(new CompileRequestDTO() {
            Compiler = submitDTO.CompilerName,
            Inputs = testCases.Select(t => t.Input).ToList(),
            Source = submitDTO.Code,
            TimeoutMs = problem.TimeLimitMilliseconds
        });

        List<NewSubmissionTestCaseModel> submissionTestCases = new List<NewSubmissionTestCaseModel>();
        
        for (int i = 0; i < compileResults.Count; i++) {
            var compileResult = compileResults[i];
            var submissionTestCase = new NewSubmissionTestCaseModel();

            submissionTestCase.TestCaseID = testCases[i].TestCaseID;
            submissionTestCase.ExecutionTimeMilliseconds = (int) (compileResult.ExecutionTimeMs ?? 0);

            if (!compileResult.CompilationSuccess) {
                submissionTestCase.Status = DAL.Models.Enums.SubmissionStatus.CompilationError;
            }
            else if (compileResult.Timeout) {
                submissionTestCase.Status = DAL.Models.Enums.SubmissionStatus.TimeLimitExceeded;
            }
            else if (!compileResult.ExecutionSuccess) {
                submissionTestCase.Status = DAL.Models.Enums.SubmissionStatus.RunTimeError;
            }
            else {
                if (StringHelper.EqualEgnoreWhiteSpaces(testCases[i].Output,compileResult.Output??""))
                    submissionTestCase.Status = DAL.Models.Enums.SubmissionStatus.Accepted;
                else
                    submissionTestCase.Status = DAL.Models.Enums.SubmissionStatus.WrongAnswer;
            }

            submissionTestCases.Add(submissionTestCase);
            if (submissionTestCase.Status != DAL.Models.Enums.SubmissionStatus.Accepted)
                break;
        }

        var submission = new NewSubmissionModel() {
            UserID = userId,
            ProblemID = submitDTO.ProblemId,
            CompilerName = submitDTO.CompilerName,
            Code = submitDTO.Code,
            VisionScope = (byte)submitDTO.VisionScope,
            SubmissionTestCases = submissionTestCases
        };
        var submissionId = await _submissionsRepo.AddNewSubmission(submission);
        if (submissionId == null)
            return new SubmitResponseDTO() {
                isSuccess = true,
                msg = "Failed to submit a solution",
            };

        return new SubmitResponseDTO() {
            isSuccess = true,
            msg = "The submission was added successfully",
        };

    }

    public List<VisionScopesDTO> GetAllVisionScopes() {
        var result = new List<VisionScopesDTO>();

        foreach (VisionScope visionScope in Enum.GetValues(typeof(VisionScope))) {
            result.Add(new VisionScopesDTO() {
                VisionScope = visionScope.ToString(),
                Id = (int)visionScope
            }
            );
        }
        return result;
    }

    public async Task<bool> ChangeVisionScope(int submissionId, int visionScopeId, int userId)
       => await _submissionsRepo.ChangeVisionScope(submissionId, visionScopeId, userId);

    private async Task<bool> UserCanViewSubmission(int submissionId, int? userId) {
        var submissionAccessInfo = await _submissionsRepo.GetSubmissionAccessInfo(submissionId);

        if (submissionAccessInfo == null) return false;

        return (userId != null && submissionAccessInfo.Value.userId == userId)
               || (submissionAccessInfo.Value.visionScope == (byte)Enums.VisionScope.all);
    }

    public async Task<SubmissionDetailsDTO?> GetSubmissionDetails(int submissionId, int? userId) {
        if (!await UserCanViewSubmission(submissionId, userId))
            return null;


        var submissionDetails = new SubmissionDetailsDTO();

        var code = await _submissionsRepo.GetSubmissionCode(submissionId);
        var submissionsTestCases = await _submissionTestRepo.GetAllSubmissionTestCasesAsync(submissionId);

        List<SubmissionTestCaseDTO> submissionTests = new();
        if (submissionsTestCases != null) {
            foreach (var test in submissionsTestCases) {
                submissionTests.Add(new SubmissionTestCaseDTO() {
                    TestCaseID = test.TestCaseID,
                    ExecutionTimeMilliseconds = test.ExecutionTimeMilliseconds,
                    Status = ((Enums.SubmissionStatus)(test.Status)).ToString()
                });
            }
        }

        return new SubmissionDetailsDTO() {
            Code = code ?? "",
            SubmissionsTestCases = submissionTests
        };
    }

    public async Task<List<SubmissionDTO>?> GetAllSubmissions(int page, int limit, int? userId = null, int? problemId = null, Enums.VisionScope? scope = null) {
        var submissions = await _submissionsRepo.GetSubmissions(page, limit, userId, problemId, (scope == null ? null : (byte)scope.Value));
        if (submissions == null)
            return null;

        var submissionsLST = new List<SubmissionDTO>();
        foreach (var submission in submissions) {
            submissionsLST.Add(new SubmissionDTO() {
                CompilerName = submission.CompilerName,
                ExecutionTimeMilliseconds = submission.ExecutionTimeMilliseconds,
                Status = ((Enums.SubmissionStatus)(submission.Status)).ToString(),
                SubmissionId = submission.SubmissionId,
                SubmittedDate = submission.SubmittedAt,
                UserID = submission.UserID,
                ProblemID = submission.ProblemID,
                VisionScope = ((Enums.VisionScope)(submission.VisionScope)).ToString()
            });
        }
        return submissionsLST;
    }

}


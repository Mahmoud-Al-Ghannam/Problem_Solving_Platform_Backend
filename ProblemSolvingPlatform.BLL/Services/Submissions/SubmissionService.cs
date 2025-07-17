using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProblemSolvingPlatform.API.Compiler.Services;
using ProblemSolvingPlatform.API.Compiler.Utils;
using ProblemSolvingPlatform.API.DTOs;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submit;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.VisionScope;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Helpers;
using ProblemSolvingPlatform.BLL.Options.Constraint;
using ProblemSolvingPlatform.BLL.Services.Compiler;
using ProblemSolvingPlatform.DAL.Models.Problems;
using ProblemSolvingPlatform.DAL.Models.Submissions;
using ProblemSolvingPlatform.DAL.Models.SubmissionTestCase;
using ProblemSolvingPlatform.DAL.Models.TestCases;
using ProblemSolvingPlatform.DAL.Repos;
using ProblemSolvingPlatform.DAL.Repos.Problems;
using ProblemSolvingPlatform.DAL.Repos.Submissions;
using ProblemSolvingPlatform.DAL.Repos.Tests;
using ProblemSolvingPlatform.DAL.Repos.Users;
using System.Collections.Generic;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.BLL.Services.Submissions;

public class SubmissionService : ISubmissionService {
    private ISubmissionRepo _submissionsRepo { get; }
    private IProblemRepo _problemRepo { get; }

    private ConstraintsOption _constraintsOption { get; }   
    private IUserRepo _userRepo { get; }
    private ICompilerService _compilerService { get; }
    private ITestCaseRepo _testCaseRepo { get; }
    private ISubmissionTestRepo _submissionTestRepo { get; }
    public SubmissionService(ISubmissionRepo submissionsRepo, IProblemRepo problemRepo,
        ISubmissionTestRepo submissionTestRepo, ITestCaseRepo testCaseRepo, ICompilerService compilerService, IUserRepo userRepo, ConstraintsOption constraintsOption) {
        _submissionsRepo = submissionsRepo;
        _problemRepo = problemRepo;
        _submissionTestRepo = submissionTestRepo;
        _testCaseRepo = testCaseRepo;
        _compilerService = compilerService;
        _userRepo = userRepo;
        _constraintsOption = constraintsOption;
    }

    // general submission 
    public async Task<int?> AddNewSubmission(SubmitDTO submitDTO, int userId) {
        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        errors["ProblemID"] = [];
        errors["UserID"] = [];
        errors["Code"] = [];

        if (!await _problemRepo.DoesProblemExistByIDAsync(submitDTO.ProblemId))
            errors["ProblemID"].Add($"The problem with id = {submitDTO.ProblemId} was not found");

        if (!await _userRepo.DoesUserExistByIDAsync(userId))
            errors["UserID"].Add($"The user with id = {userId} was not found");


        if (string.IsNullOrEmpty(submitDTO.Code)) {
            if (_constraintsOption.Submission.CodeLength.Start.Value > 0)
                errors["Code"].Add($"The submission's code is required");
        }
        else {
            if (submitDTO.Code.Length > _constraintsOption.Submission.CodeLength.End.Value || submitDTO.Code.Length < _constraintsOption.Submission.CodeLength.Start.Value)
                errors["Code"].Add($"The length of submission's code must to be in range [{_constraintsOption.Submission.CodeLength.Start.Value},{_constraintsOption.Submission.CodeLength.End.Value}]");
        }


        List<TestCaseModel>? testCases = (await _testCaseRepo.GetTestCasesByProblemIdAsync(submitDTO.ProblemId))?.ToList();
        ProblemModel? problem = await _problemRepo.GetProblemByIDAsync(submitDTO.ProblemId);


        List<CompileResponseDTO> compileResults = new();
        try {
            compileResults = await _compilerService.CompileAsync(new CompileRequestDTO() {
                Compiler = submitDTO.CompilerName,
                Inputs = testCases?.Select(t => t.Input).ToList() ?? [],
                Source = submitDTO.Code,
                TimeoutMs = problem?.TimeLimitMilliseconds ?? 0
            });
        }
        catch (CustomValidationException ex) {
            foreach (var (k, v) in ex.errors) {
                if (errors.ContainsKey(k))
                    errors[k].AddRange(v);
                else
                    errors[k] = v;
            }
        }


        errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
        if (errors.Count > 0) throw new CustomValidationException(errors);


        if (testCases == null || problem == null) throw new Exception(Constants.ErrorMessages.General);

        List<NewSubmissionTestCaseModel> submissionTestCases = new List<NewSubmissionTestCaseModel>();

        for (int i = 0; i < compileResults.Count; i++) {
            var compileResult = compileResults[i];
            var submissionTestCase = new NewSubmissionTestCaseModel();

            submissionTestCase.TestCaseID = testCases[i].TestCaseID;
            submissionTestCase.ExecutionTimeMilliseconds = (int)(compileResult.ExecutionTimeMs ?? 0);
            submissionTestCase.Output = StringHelper.RemoveWhiteSpaces(compileResult.Output??"");

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
                if (StringHelper.EqualEgnoreWhiteSpaces(testCases[i].Output, compileResult.Output ?? ""))
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

        return await _submissionsRepo.AddNewSubmission(submission);
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

    public async Task<bool> ChangeVisionScope(int submissionId, int visionScopeId, int userId) {
        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        errors["SubmissionID"] = [];
        errors["VisionScopeID"] = [];
        errors["UserID"] = [];

        if (!await _submissionsRepo.DoesSubmissionExistByID(submissionId))
            errors["SubmissionID"].Add($"The submission with id = {submissionId} was not found");
        else {
            var existingSubmission = await _submissionsRepo.GetSubmissionByID(submissionId);
            if (existingSubmission == null) throw new Exception(Constants.ErrorMessages.General);

            if (existingSubmission.UserID != userId)
                errors["UserID"].Add($"The submission with id = {submissionId} does not belong to user with id = {userId}");
        }

        if (!GetAllVisionScopes().Any(v => v.Id == visionScopeId))
            errors["VisionScopeID"].Add($"The vision scopeID with id = {visionScopeId} was not found");

        if (!await _userRepo.DoesUserExistByIDAsync(userId))
            errors["UserID"].Add($"The user with id = {userId} was not found");


        errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
        if (errors.Count > 0) throw new CustomValidationException(errors);

        return await _submissionsRepo.ChangeVisionScope(submissionId, visionScopeId, userId);
    }

    public async Task<DetailedSubmissionDTO?> GetDetailedSubmissionByID(int submissionId, int? requestedBy) {
        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        errors["SubmissionID"] = [];
        errors["UserID"] = [];

        if (!await _submissionsRepo.DoesSubmissionExistByID(submissionId))
            errors["SubmissionID"].Add($"The submission with id = {submissionId} was not found");

        if (requestedBy.HasValue && !await _userRepo.DoesUserExistByIDAsync(requestedBy.Value))
            errors["UserID"].Add($"The user with id = {requestedBy} was not found");

        var submissionDetails = new DetailedSubmissionDTO();

        var submission = await _submissionsRepo.GetSubmissionByID(submissionId);
        if (submission == null) throw new Exception(Constants.ErrorMessages.General);

        if (((requestedBy.HasValue && submission.UserID == requestedBy.Value)
               || (submission.VisionScope == DAL.Models.Enums.VisionScope.all)) == false)
            errors["UserID"].Add($"The user with id = {requestedBy} cannot see the submission with id = {submissionId}");

        errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
        if (errors.Count > 0) throw new CustomValidationException(errors);


        var submissionsTestCases = await _submissionTestRepo.GetAllDetailedSubmissionTestCasesAsync(submissionId);

        List<DetailedSubmissionTestCaseDTO> submissionTests = new();
        if (submissionsTestCases != null) {
            foreach (var test in submissionsTestCases) {
                submissionTests.Add(new DetailedSubmissionTestCaseDTO() {
                    SubmissionTestCaseID = test.SubmissionTestCaseID,
                    SubmissionID = test.SubmissionID,
                    TestCaseID = test.TestCaseID,
                    ExecutionTimeMilliseconds = test.ExecutionTimeMilliseconds,
                    Status = ((Enums.SubmissionStatus)(test.Status)).ToString(),
                    Output = test.Output,
                    Input = test.Input,
                    ExpectedOutput = test.ExpectedOutput
                });
            }
        }

        return new DetailedSubmissionDTO() {
            SubmissionInfo = new SubmissionDTO() { 
                SubmissionID = submission.SubmissionID,
                UserID = submission.UserID,
                ProblemID = submission.ProblemID,
                CompilerName = submission.CompilerName,
                Status = submission.Status.ToString(),
                ExecutionTimeMilliseconds = submission.ExecutionTimeMilliseconds,
                Code = submission.Code,
                SubmittedAt = submission.SubmittedAt,
                VisionScope = submission.VisionScope.ToString(),
            },
            SubmissionsTestCases = submissionTests
        };
    }

    public async Task<List<SubmissionDTO>?> GetAllSubmissions(int page, int limit,int? requestedBy = null, int? userId = null, int? problemId = null, Enums.VisionScope? scope = null) {
        Dictionary<string, List<string>> errors = new();
        errors["Page"] = [];
        errors["Limit"] = [];

        if (page < _constraintsOption.MinPageNumber)
            errors["Page"].Add($"The page must to be greater than {_constraintsOption.MinPageNumber}");

        if (limit < _constraintsOption.PageSize.Start.Value || limit > _constraintsOption.PageSize.End.Value)
            errors["Limit"].Add($"The limit must to be in range [{_constraintsOption.PageSize.Start.Value},{_constraintsOption.PageSize.End.Value}]");

        errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
        if (errors.Count > 0) throw new CustomValidationException(errors);

        var submissions = await _submissionsRepo.GetAllSubmissions(page, limit, userId, problemId, (scope == null ? null : (byte)scope.Value));
        if (submissions == null)
            return null;

        if (requestedBy == null)
            submissions = submissions.Where(s => s.VisionScope == DAL.Models.Enums.VisionScope.all).ToList();
        else
            submissions = submissions.Where(s => s.UserID == requestedBy.Value || s.VisionScope == DAL.Models.Enums.VisionScope.all).ToList();


        var submissionsLST = new List<SubmissionDTO>();
        foreach (var submission in submissions) {
            submissionsLST.Add(new SubmissionDTO() {
                CompilerName = submission.CompilerName,
                ExecutionTimeMilliseconds = submission.ExecutionTimeMilliseconds,
                Status = ((Enums.SubmissionStatus)(submission.Status)).ToString(),
                SubmissionID = submission.SubmissionID,
                SubmittedAt = submission.SubmittedAt,
                UserID = submission.UserID,
                Code = submission.Code,
                ProblemID = submission.ProblemID,
                VisionScope = ((Enums.VisionScope)(submission.VisionScope)).ToString()
            });
        }
        return submissionsLST;
    }

    public async Task<SubmissionDTO?> GetSubmissionByID(int submissionId, int? userId) {
        return (await GetDetailedSubmissionByID(submissionId, userId))?.SubmissionInfo;
    }
}


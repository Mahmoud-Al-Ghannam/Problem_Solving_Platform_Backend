using ProblemSolvingPlatform.API.Compiler.Utils;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Submissions;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.VisionScope;
using ProblemSolvingPlatform.BLL.Services.Submissions.Handling_Submission;
using ProblemSolvingPlatform.DAL.Models.Submissions;
using ProblemSolvingPlatform.DAL.Repos.Problem;
using ProblemSolvingPlatform.DAL.Repos.Submissions;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.BLL.Services.Submissions;

public class SubmissionsService : ISubmissionsService
{
    private ISubmissionsRepo _submissionsRepo { get; }
    private IProblemRepo _problemRepo { get; }
    private SubmissionHandler _submissionHandler { get; }
    public SubmissionsService(ISubmissionsRepo submissionsRepo, IProblemRepo problemRepo, SubmissionHandler submissionHandler)
    {
        _submissionsRepo = submissionsRepo;
        _problemRepo = problemRepo;
        _submissionHandler =    submissionHandler;
    }

    // general submission 
    public async Task<SubmitResponseDTO> Submit(SubmitDTO submitDTO, int userId)
    {
        // if prolem not exist => return bad request, problem not exist 
        if (!await _problemRepo.ProblemExists(submitDTO.ProblemId))
            return new SubmitResponseDTO()
            {
                isSuccess = false,
                msg = "The problem not found :)"
            };

        // save in db 
        
        // take the language name (C++, ... ) :)
        string language = CompilerUtils.GetCompiler(submitDTO.compilerName)?.Language ?? "";

        // convert from name into value from Enum :)
        Enums.ProgLanguages parsedLanguage = Enums.ProgLanguages.Unknown;
        if (Enum.TryParse(language, true, out Enums.ProgLanguages result)) {
            parsedLanguage = result;
        }
        byte languageNum = (byte)parsedLanguage;
        // 


        var submission = new Submission()
        {
            UserID = userId,
            ProgrammingLanguage = languageNum,
            Code = submitDTO.Code,
            VisionScope = submitDTO.VisionScope
        };
        var submissionId = await _submissionsRepo.AddGeneralProblemSubmission(submitDTO.ProblemId, submission);
        if (submissionId == null)
            return new SubmitResponseDTO()
            {
                isSuccess = true,
                msg = "Failed to submit a solution",
            };

        //  send submission for compiler
        // await _submissionHandler.ExecuteSubmission(submissionId.Value, submitDTO.Code);


        return new SubmitResponseDTO()
        {
            isSuccess = true,
            msg = "Submission sent successfully, but this is only for testing so the submission on pending:) ",
            submissionId = submissionId.Value,
            Status = Enums.SubmissionStatus.Pending.ToString()
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
    
    
}


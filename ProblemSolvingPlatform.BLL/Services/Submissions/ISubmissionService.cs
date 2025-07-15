using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submit;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.VisionScope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Submissions;

public interface ISubmissionService
{ 
    public Task<SubmitResponseDTO> Submit(SubmitDTO submitDTO, int userId);

    public List<VisionScopesDTO> GetAllVisionScopes();

    public Task<bool> ChangeVisionScope(int submissionId, int visionScopeId, int userId);


    public Task<List<SubmissionDTO>?> GetAllSubmissions(int page, int limit,int? userId = null, int? problemId = null,  Enums.VisionScope? scope = null);

    public Task<SubmissionDetailsDTO?> GetSubmissionDetails(int submissionId, int? userId);

}
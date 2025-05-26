using ProblemSolvingPlatform.BLL.DTOs.Submissions;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.VisionScope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Submissions;

public interface ISubmissionsService
{ 
    public Task<SubmitResponseDTO> Submit(SubmitDTO submitDTO, int userId);

    public List<VisionScopesDTO> GetAllVisionScopes();

    public Task<bool> ChangeVisionScope(int submissionId, int visionScopeId, int userId);

}

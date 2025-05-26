using ProblemSolvingPlatform.DAL.Models.Submissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos.Submissions;

public interface ISubmissionsRepo
{
     public Task<int?> AddGeneralProblemSubmission(int problemId, Submission submission);
    // AddSubmission(groupId, submission, problemGroupId)

    public Task<bool> ChangeVisionScope(int submissionId, int visionScopeId, int userId);
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using ProblemSolvingPlatform.DAL.Models.Submissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos.Submissions;

public interface ISubmissionRepo {
    public Task<int?> AddNewSubmission(Models.Submissions.Submission submission);

    public Task<bool> ChangeVisionScope(int submissionId, int visionScopeId, int userId);

    public Task<bool> UpdateSubmissionStatusAndExecTime(int submissionId, byte status, int execTimeMS);

    public Task<(int userId, byte visionScope)?> GetSubmissionAccessInfo(int submissionId);

    public Task<string?> GetSubmissionCode(int submissionId);

    public Task<List<Submission>?> GetSubmissions(int page, int limit, int? userId = null, int? problemId = null, byte? visionScope = null);

}
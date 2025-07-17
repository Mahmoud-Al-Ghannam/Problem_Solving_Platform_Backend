using Microsoft.AspNetCore.Mvc.RazorPages;
using ProblemSolvingPlatform.DAL.Models.Submissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos.Submissions;

public interface ISubmissionRepo {
    public Task<int?> AddNewSubmission(NewSubmissionModel submission);

    public Task<bool> ChangeVisionScope(int submissionId, int visionScopeId, int userId);

    public Task<SubmissionModel?> GetSubmissionByID(int submissionID);
    public Task<List<SubmissionModel>?> GetAllSubmissions(int page, int limit, int? userId = null, int? problemId = null, byte? visionScope = null);

    public Task<bool> DoesSubmissionExistByID(int submissionID);
}
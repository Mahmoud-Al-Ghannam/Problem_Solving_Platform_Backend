using Microsoft.AspNetCore.Mvc.RazorPages;
using ProblemSolvingPlatform.DAL.Models;
using ProblemSolvingPlatform.DAL.Models.Submissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.DAL.Models.Enums;

namespace ProblemSolvingPlatform.DAL.Repos.Submissions;

public interface ISubmissionRepo {
    public Task<int?> AddNewSubmission(NewSubmissionModel submission);

    public Task<bool> ChangeVisionScope(int submissionId, int visionScopeId, int userId);

    public Task<SubmissionModel?> GetSubmissionByID(int submissionID);
    public Task<PageModel<SubmissionModel>?> GetAllSubmissions(int page, int limit, int? userId = null, int? problemId = null, VisionScope? visionScope = null);
    public Task<(int totalPages, int totalItems)?> GetTotalPagesAndItemsCountAsync(int limit, int? userId = null, int? problemId = null, VisionScope? visionScope = null);

    public Task<bool> DoesSubmissionExistByID(int submissionID);
}
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.DAL.Models;
using ProblemSolvingPlatform.DAL.Models.Problems;
using ProblemSolvingPlatform.DAL.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos.Problems {
    public interface IProblemRepo {
        public Task<int?> AddProblemAsync(NewProblemModel newProblem);
        public Task<bool> UpdateProblemAsync(UpdateProblemModel newProblem);
        public Task<bool> DeleteProblemByIDAsync(int problemID,int deletedBy);

        public Task<ProblemModel?> GetProblemByIDAsync(int problemID);
        public Task<IEnumerable<TagModel>?> GetProblemTagsAsync(int problemID);

        public Task<bool> DoesProblemExistByIDAsync(int problemId);
        public Task<PageModel<ShortProblemModel>?> GetAllProblemsAsync(int page,int limit,string? title = null,byte? difficulty = null,int? createdBy = null,bool? isSystemProblem = null,DateTime? createdAt = null,bool? isDeleted = null,IEnumerable<int>? tagIDs = null,int? tryingStatusForUser = null);
        public Task<(int totalPages,int totalItems)?> GetTotalPagesAndItemsCountAsync(int limit,string? title = null,byte? difficulty = null,int? createdBy = null,bool? isSystemProblem = null,DateTime? createdAt = null,bool? isDeleted = null,IEnumerable<int>? tagIDs = null);
    }
}

using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.DAL.Models.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.BLL.Services.Problems {
    public interface IProblemService {
        public Task<int?> AddProblemAsync(NewProblemDTO newProblem,int createdBy,bool isSystemProblem);
        public Task<bool> UpdateProblemAsync(UpdateProblemDTO updateProblem,int userID);
        public Task<bool> DeleteProblemByIDAsync(int problemID,int deletedBy);
        public Task<ProblemDTO?> GetProblemByIDAsync(int problemID);
        public Task<PageDTO<ShortProblemDTO>?> GetAllProblemsAsync(int page, int limit, string? title = null, Difficulty? difficulty = null, int? createdBy = null,bool? IsSystemProblem = null, DateTime? createdAt = null,bool? isDeleted = null, string? tagIDs = null,int? tryingStatusForUser=null,TryingStatusOfProblem? tryingStatus = null);
    }
}

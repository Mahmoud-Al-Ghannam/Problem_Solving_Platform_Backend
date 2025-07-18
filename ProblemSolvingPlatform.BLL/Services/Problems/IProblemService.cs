using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.DAL.Models.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Problems {
    public interface IProblemService {
        public Task<int?> AddProblemAsync(NewProblemDTO newProblem,int createdBy,bool isSystemProblem);
        public Task<bool> UpdateProblemAsync(UpdateProblemDTO updateProblem,int userID);
        public Task<bool> DeleteProblemByIDAsync(int problemID,int deletedBy);
        public Task<ProblemDTO?> GetProblemByIDAsync(int problemID);
        public Task<IEnumerable<ShortProblemDTO>?> GetAllProblemsAsync(int page, int limit, string? title = null, byte? difficulty = null, int? createdBy = null,bool? IsSystemProblem = null, DateTime? createdAt = null,bool? isDeleted = null, string? tagIDs = null);
    }
}

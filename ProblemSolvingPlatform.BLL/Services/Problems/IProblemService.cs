using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.DAL.Models.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Problems {
    public interface IProblemService {
        public Task<int?> AddProblemAsync(NewProblemDTO newProblem,int createdBy);
        public Task<bool> UpdateProblemAsync(UpdateProblemDTO updateProblem,int userID);
        public Task<bool> DeleteProblemByIDAsync(int problemID,int deletedBy);
        public Task<ProblemDTO?> GetProblemByIDAsync(int problemID);
    }
}

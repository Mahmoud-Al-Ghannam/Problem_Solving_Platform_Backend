using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.DAL.Models.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Problems {
    public interface IProblemService {
        public Task<int?> AddProblemAsync(NewProblemDTO newProblem);
        public Task<ProblemDTO?> GetProblemByIDAsync(int problemID);
    }
}

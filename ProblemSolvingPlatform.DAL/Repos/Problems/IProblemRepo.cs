using ProblemSolvingPlatform.DAL.Models.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos.Problems {
    public interface IProblemRepo {
        public Task<int?> AddProblemAsync(NewProblemModel newProblem);

        public Task<ProblemModel> GetProblemByIDAsync(int problemID);

        public Task<bool> ProblemExistsAsync(int problemId);
    }
}

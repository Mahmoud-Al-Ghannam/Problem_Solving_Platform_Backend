using ProblemSolvingPlatform.DAL.Models.Problem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos.Problem {
    public interface IProblemRepo {
        public Task<int?> AddProblemAsync(NewProblemModel newProblem);
    }
}

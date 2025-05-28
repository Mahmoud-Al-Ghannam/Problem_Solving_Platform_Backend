using ProblemSolvingPlatform.BLL.DTOs.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Problem {
    public interface IProblemService {
        public Task<int?> AddProblemAsync(NewProblemDTO newProblem);
    }
}

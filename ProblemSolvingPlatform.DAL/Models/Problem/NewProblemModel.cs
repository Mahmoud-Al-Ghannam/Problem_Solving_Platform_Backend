using ProblemSolvingPlatform.DAL.Models.TestCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.DAL.Models.Enums;

namespace ProblemSolvingPlatform.DAL.Models.Problem {
    public class NewProblemModel {
        public string CompilerID {  get; set; }
        public int CreatedBy {  get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public string InputDescription { get; set; }
        public string OutputDescription { get; set; }
        public string Note { get; set; }
        public string Tutorial { get; set; }
        public Difficulty Difficulty { get; set; }
        public string SolutionCode { get; set; }
        public int TimeLimitMilliseconds { get; set; }
        public List<NewTestCaseModel> TestCases { get; set; }
        public List<int> TagIDs { get; set; }
    }
}

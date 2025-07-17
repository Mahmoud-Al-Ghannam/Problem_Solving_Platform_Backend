using ProblemSolvingPlatform.DAL.Models.Tags;
using ProblemSolvingPlatform.DAL.Models.TestCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.DAL.Models.Enums;

namespace ProblemSolvingPlatform.DAL.Models.Problems {
    public class ProblemModel {
        public int ProblemID { get; set; }
        public string CompilerName { get; set; }
        public int CreatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public string InputDescription { get; set; }
        public string OutputDescription { get; set; }
        public string? Note { get; set; }
        public string? Tutorial { get; set; }
        public Difficulty Difficulty { get; set; }
        public string SolutionCode { get; set; }
        public int TimeLimitMilliseconds { get; set; }
        public bool IsSystemProblem { get; set; }
        public IEnumerable<TestCaseModel> SampleTestCases { get; set; }

        public IEnumerable<TagModel> Tags { get; set; }
    }
}

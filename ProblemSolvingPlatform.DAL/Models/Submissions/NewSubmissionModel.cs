using ProblemSolvingPlatform.DAL.Models.SubmissionTestCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Models.Submissions {
    public class NewSubmissionModel {
        public int UserID { get; set; }
        public int ProblemID { get; set; }
        public string CompilerName { get; set; }
        public string Code { get; set; }
        public byte VisionScope { get; set; }

        public List<NewSubmissionTestCaseModel> SubmissionTestCases { get; set; }

    }
}

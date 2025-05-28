using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Models.TestCases {
    public class TestCaseModel {
        public int TestCaseID { get; set; }
        public int ProblemID { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public bool IsPublic { get; set; }
        public bool IsSample { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Models.TestCases {
    public class NewTestCaseModel {
		public string Input { get; set; }
		public string Output { get; set; }
		public bool IsPublic { get; set; }
		public bool IsSample { get; set; }
    }
}

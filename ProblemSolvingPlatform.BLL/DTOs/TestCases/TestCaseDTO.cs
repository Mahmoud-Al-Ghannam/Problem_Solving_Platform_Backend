using ProblemSolvingPlatform.BLL.Validation.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.TestCases {
    public class TestCaseDTO {
        public int TestCaseID { get; set; }
        public int ProblemID { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public bool IsPublic { get; set; }
        public bool IsSample { get; set; }
    }
}

using ProblemSolvingPlatform.BLL.DTOs.TestCases;
using ProblemSolvingPlatform.BLL.Validation.ValidationAttributes;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.BLL.DTOs.Problems {
    public class NewProblemDTO {
        public string CompilerName { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public string InputDescription { get; set; }
        public string OutputDescription { get; set; }
        public string? Note { get; set; }
        public string? Tutorial { get; set; }
        public Difficulty Difficulty { get; set; }
        public string SolutionCode { get; set; }
        public int TimeLimitMilliseconds { get; set; }
        public List<NewTestCaseDTO> TestCases { get; set; }
        public List<int> TagIDs { get; set; }
    }
}

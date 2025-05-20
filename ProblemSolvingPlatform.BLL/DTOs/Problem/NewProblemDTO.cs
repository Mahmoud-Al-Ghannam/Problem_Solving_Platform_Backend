using ProblemSolvingPlatform.BLL.DTOs.TestCase;
using ProblemSolvingPlatform.BLL.Validation.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.BLL.DTOs.Problem {
    public class NewProblemDTO {
        [Required]
        [MinLengthAfterTrim(1)]
        public string CompilerName { get; set; }

        [Required]
        [Range(1,int.MaxValue)]
        public int CreatedBy { get; set; }
        [Required]
        [MinLengthAfterTrim(1)]
        public string Title { get; set; }
        [Required]
        [MinLengthAfterTrim(1)]
        public string GeneralDescription { get; set; }
        [Required]
        [MinLengthAfterTrim(1)]
        public string InputDescription { get; set; }
        [Required]
        [MinLengthAfterTrim(1)]
        public string OutputDescription { get; set; }
        [Required]
        [MinLengthAfterTrim(1)]
        public string Note { get; set; }
        [Required]
        [MinLengthAfterTrim(1)]
        public string Tutorial { get; set; }
        [Required]
        public Difficulty Difficulty { get; set; }
        [Required]
        [MinLengthAfterTrim(1)]
        public string SolutionCode { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int TimeLimitMilliseconds { get; set; }
        [Required]
        [MinLength(1)]
        [ValidationCollection]
        public List<NewTestCaseDTO> TestCases { get; set; }

        [Required]
        [MinLength(1)]
        //[ValidationCollection]
        public List<int> TagIDs { get; set; }
    }
}

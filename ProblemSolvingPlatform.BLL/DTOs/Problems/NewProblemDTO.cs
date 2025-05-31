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
        [Required]
        [MinLengthAfterTrim(1)]
        public string CompilerName { get; set; }

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
        [MinLengthAfterTrim(1)]
        public string? Note { get; set; }
        [MinLengthAfterTrim(1)]
        public string? Tutorial { get; set; }
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
        public IEnumerable<NewTestCaseDTO> TestCases { get; set; }

        [Required]
        [MinLength(1)]
        //[ValidationCollection]
        public IEnumerable<int> TagIDs { get; set; }
    }
}

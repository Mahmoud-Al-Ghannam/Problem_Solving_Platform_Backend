using ProblemSolvingPlatform.BLL.Validation.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.TestCase {
    public class NewTestCaseDTO {

        [Required]
        [MinLengthAfterTrim(1)]
        public string Input { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        [Required]
        public bool IsSample { get; set; }
    }
}

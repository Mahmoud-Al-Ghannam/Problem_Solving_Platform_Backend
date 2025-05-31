using ProblemSolvingPlatform.DAL.Models.TestCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.DAL.Models.Enums;

namespace ProblemSolvingPlatform.DAL.Models.Problems {
    public class UpdateProblemModel {
        public int ProblemID { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public string InputDescription { get; set; }
        public string OutputDescription { get; set; }
        public string? Note { get; set; }
        public string? Tutorial { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}

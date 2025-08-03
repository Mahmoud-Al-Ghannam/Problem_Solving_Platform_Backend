using ProblemSolvingPlatform.DAL.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.DAL.Models.Enums;

namespace ProblemSolvingPlatform.DAL.Models.Problems
{
    public class ShortProblemModel
    {
        public int ProblemID { get; set; }
        public int CreatedByID { get; set; }
        public string CreatedByUsername { get; set; }
        public int? DeletedByID { get; set; }
        public string? DeletedByUsername { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }

        public bool IsSystemProblem { get; set; }
        public TryingStatusOfProblem? TryingStatus { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<TagModel> Tags { get; set; }
        public int SolutionsCount { get; set; }
        public int AttemptsCount { get; set; }
    }
}

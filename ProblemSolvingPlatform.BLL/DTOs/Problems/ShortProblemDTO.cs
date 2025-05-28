using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.BLL.DTOs.TestCases;
using ProblemSolvingPlatform.BLL.Validation.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.BLL.DTOs.Problems {
    public class ShortProblemDTO {
        public int ProblemID { get; set; }

        /// <summary>
        /// The username of creator this problem
        /// </summary>
        public string Username { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<TagDTO> Tags { get; set; }

        /// <summary>
        /// The number of users who solved the problem
        /// </summary>
        public int SolutionsCount { get; set; }

        /// <summary>
        /// The number of users who tried to solve the problem
        /// </summary>
        public int AttemptsCount { get; set; }
    }
}

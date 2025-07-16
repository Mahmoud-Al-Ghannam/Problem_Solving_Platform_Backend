using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Models {
    public class Enums {
        public enum Difficulty { Easy = 0, Medium = 1, Hard = 2 }

        public enum SubmissionStatus {
            Pending = 0, Accepted = 1, WrongAnswer = 2,
            CompilationError = 3,
            RunTimeError = 4, TimeLimitExceeded = 5, MemoryLimitExceeded = 6
        }
    }
}

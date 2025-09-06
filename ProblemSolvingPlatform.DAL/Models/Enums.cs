using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Models {
    public class Enums {
        public enum Difficulty { Easy = 0, Medium = 1, Hard = 2 }
        public enum TryingStatusOfProblem { NotTry = 0, Tried = 1, Solved = 2 }

        public enum VisionScope { onlyme = 0, all = 1 }

        public enum Role { System = 0, User = 1 }
        public enum SubmissionStatus {
            Accepted = 1, WrongAnswer = 2,
            CompilationError = 3,
            RunTimeError = 4, TimeLimitExceeded = 5
        }

        // pending, memoryLimitExceeded 
    }
}

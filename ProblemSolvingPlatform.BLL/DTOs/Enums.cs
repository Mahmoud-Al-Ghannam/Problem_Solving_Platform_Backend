using Microsoft.OpenApi.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs {
    public class Enums {
        public enum Difficulty {eEasy = 0,eMedium = 1, eHard = 2 }

        public enum ProgLanguages { Cpp = 0, Csharp = 1, C = 2 , Unknown = 99}

        public enum VisionScope { onlyme = 0, all = 1 }

        public enum SubmissionStatus
        {
            Pending = 0, Accepted = 1, WrongAnswer = 2,
            CompilationError = 3,
            RunTimeError = 4, TimeLimitExceeded = 5, MemoryLimitExceeded = 6
        }


        public enum SubTypeSubmissions { General = 0, Group = 1 }
    }
}

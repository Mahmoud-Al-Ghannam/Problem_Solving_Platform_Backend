using Microsoft.Identity.Client;
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
        public enum Role { System = 0, User = 1 }
        public enum Difficulty { Easy = 0, Medium = 1, Hard = 2 }

        public enum VisionScope { onlyme = 0, all = 1 }

        public enum SubmissionStatus {
            Pending = 0, Accepted = 1, WrongAnswer = 2,
            CompilationError = 3,
            RunTimeError = 4, TimeLimitExceeded = 5, MemoryLimitExceeded = 6
        }


        public enum SubTypeSubmissions { General = 0, Group = 1 }
    }
}

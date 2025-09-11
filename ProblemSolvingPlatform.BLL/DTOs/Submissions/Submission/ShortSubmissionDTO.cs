using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission {
    public class ShortSubmissionDTO {
        public int SubmissionID { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public int ProblemID { get; set; }
        public string ProblemTitle { get; set; }
        public bool IsProblemDeleted { get; set; }
        public string CompilerName { get; set; }
        public Enums.SubmissionStatus Status { get; set; }
        public int ExecutionTimeMilliseconds { get; set; }
        public DateTime SubmittedAt { get; set; }
        public Enums.VisionScope VisionScope { get; set; }
    }
}

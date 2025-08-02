using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;

public class SubmissionDTO
{
    public int SubmissionID { get; set; }
    public int UserID { get; set; }
    public int ProblemID { get; set; }
    public string CompilerName { get; set; }
    public string Status { get; set; }
    public int ExecutionTimeMilliseconds { get; set; }
    public string Code { get; set; }
    public DateTime SubmittedAt { get; set; }
    public Enums.VisionScope VisionScope { get; set; }
}

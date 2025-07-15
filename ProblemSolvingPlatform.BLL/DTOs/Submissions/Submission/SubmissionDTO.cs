using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;

public class SubmissionDTO
{
    public int SubmissionId { get; set; }
    public int UserID { get; set; }
    public int ProblemID { get; set; }
    public string CompilerName { get; set; }
    public string Status { get; set; }
    public int ExecutionTimeMilliseconds { get; set; }
    public DateTime SubmittedDate { get; set; }
    public string VisionScope { get; set; }
}

using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Models.Submissions;

public class Submission
{
    public int SubmissionId { get; set; }
    public int UserID { get; set; }
    public int ProblemID { get; set; }
    public string CompilerName { get; set; }
    public byte Status { get; set; }
    public int ExecutionTimeMilliseconds { get; set; }
    public string Code { get; set; }
    public byte VisionScope { get; set; }
    public byte SubType { get; set; }
    public DateTime SubmittedAt { get; set; }
}

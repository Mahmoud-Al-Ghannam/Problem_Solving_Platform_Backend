using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;

public class SubmissionTestCaseDTO
{
    public int TestCaseID { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ExecutionTimeMilliseconds { get; set; }
}

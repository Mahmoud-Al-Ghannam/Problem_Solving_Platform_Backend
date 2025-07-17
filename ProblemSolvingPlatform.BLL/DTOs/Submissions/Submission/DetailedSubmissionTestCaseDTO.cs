using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;

public class DetailedSubmissionTestCaseDTO {

    public int SubmissionTestCaseID { get; set; }
    public int SubmissionID { get; set; }
    public int TestCaseID { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ExecutionTimeMilliseconds { get; set; }
    public string Input { get; set; }
    public string ExpectedOutput { get; set; }
    public string Output { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Models.SubmissionTestCase;

public class SubmissionTestCaseModel
{
    public int SubmissionTestCaseID { get; set; }
    public int TestCaseID { get; set; }
    public int SubmissionID { get; set; }
    public byte Status { get; set; }
    public int ExecutionTimeMilliseconds { get; set; }
}

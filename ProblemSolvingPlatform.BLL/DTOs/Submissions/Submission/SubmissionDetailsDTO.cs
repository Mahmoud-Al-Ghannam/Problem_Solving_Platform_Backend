using ProblemSolvingPlatform.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;

public class SubmissionDetailsDTO
{
     public SubmissionDTO SubmissionInfo { get; set; } = new();
    public List<SubmissionTestCaseDTO> SubmissionsTestCases { get; set; } = new();
}

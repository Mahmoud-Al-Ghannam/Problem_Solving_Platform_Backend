using ProblemSolvingPlatform.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;

public class DetailedSubmissionDTO
{
     public SubmissionDTO SubmissionInfo { get; set; } = new();
    public List<DetailedSubmissionTestCaseDTO> SubmissionsTestCases { get; set; } = new();
}

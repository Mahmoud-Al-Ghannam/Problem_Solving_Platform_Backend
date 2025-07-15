using ProblemSolvingPlatform.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos;

public interface ISubmissionTestRepo
{
    public Task<int?> AddNewSubmissionTestCase(SubmissionTestCase submissionTestCase);

    public Task<List<SubmissionTestCase>?> GetAllSubmissionTestCases(int? submissionId = null);
}

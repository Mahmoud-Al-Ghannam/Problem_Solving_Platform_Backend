using Microsoft.Data.SqlClient;
using ProblemSolvingPlatform.DAL.Models;
using ProblemSolvingPlatform.DAL.Models.SubmissionTestCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos;

public interface ISubmissionTestRepo
{
    public Task<int?> AddNewSubmissionTestCaseAsync(NewSubmissionTestCaseModel submissionTestCase, SqlConnection connection, SqlTransaction transaction);

    public Task<List<SubmissionTestCaseModel>?> GetAllSubmissionTestCasesAsync(int? submissionId = null);
    public Task<List<DetailedSubmissionTestCaseModel>?> GetAllDetailedSubmissionTestCasesAsync(int? submissionId = null);
}

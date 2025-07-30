using ProblemSolvingPlatform.DAL.Models.TestCases;

namespace ProblemSolvingPlatform.DAL.Repos.Tests
{
    public interface ITestCaseRepo
    {
        public Task<IEnumerable<TestCaseModel>?> GetAllTestCasesAsync(int? ProblemID = null, bool? IsSample = null, bool? IsPublic = null);
    }
}
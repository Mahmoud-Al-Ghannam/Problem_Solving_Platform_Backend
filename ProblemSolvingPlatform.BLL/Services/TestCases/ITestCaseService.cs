using ProblemSolvingPlatform.BLL.DTOs.TestCases;

namespace ProblemSolvingPlatform.BLL.Services.TestCases {
    public interface ITestCaseService {
        public Task<IEnumerable<TestCaseDTO>?> GetAllTestCasesAsync(int Page, int Limit, int? ProblemID = null, bool? IsSample = null, bool? IsPublic = null);
    }
}
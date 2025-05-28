using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.BLL.DTOs.TestCases;
using ProblemSolvingPlatform.DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.TestCases {
    public class TestCaseService : ITestCaseService {

        private readonly ITestCaseRepo _testCaseRepo;

        public TestCaseService(ITestCaseRepo testCaseRepo) {
            _testCaseRepo = testCaseRepo;
        }

        public async Task<IEnumerable<TestCaseDTO>?> GetAllTestCasesAsync(int Page, int Limit, int? ProblemID = null, bool? IsSample = null, bool? IsPublic = null) {
            var testCasesModel = await _testCaseRepo.GetAllTestCasesAsync(Page,Limit,ProblemID,IsSample,IsPublic);

            var testCasesDTO = testCasesModel?.Select(tcm => new TestCaseDTO() {
                TestCaseID = tcm.TestCaseID,
                ProblemID = tcm.ProblemID,
                Input = tcm.Input,
                Output = tcm.Output,
                IsPublic = tcm.IsPublic,
                IsSample = tcm.IsSample
            }).ToList();

            return testCasesDTO;
        }
    }
}

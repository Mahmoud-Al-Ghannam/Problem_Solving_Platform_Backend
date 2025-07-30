using Azure;
using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.BLL.DTOs.TestCases;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Options.Constraint;
using ProblemSolvingPlatform.DAL.Repos.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.TestCases
{
    public class TestCaseService : ITestCaseService {

        private readonly ITestCaseRepo _testCaseRepo;
        private readonly ConstraintsOption _constraintsOption;

        public TestCaseService(ITestCaseRepo testCaseRepo, ConstraintsOption constraintsOption) {
            _testCaseRepo = testCaseRepo;
            _constraintsOption = constraintsOption;
        }

        public async Task<IEnumerable<TestCaseDTO>?> GetAllTestCasesAsync(int? ProblemID = null, bool? IsSample = null, bool? IsPublic = null) {
            
            var testCasesModel = await _testCaseRepo.GetAllTestCasesAsync(ProblemID,IsSample,IsPublic);

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

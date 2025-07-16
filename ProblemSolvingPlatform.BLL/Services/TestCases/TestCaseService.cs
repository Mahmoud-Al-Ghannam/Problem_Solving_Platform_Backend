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

        public async Task<IEnumerable<TestCaseDTO>?> GetAllTestCasesAsync(int Page, int Limit, int? ProblemID = null, bool? IsSample = null, bool? IsPublic = null) {
            Dictionary<string, List<string>> errors = new();
            errors["Page"] = [];
            errors["Limit"] = [];

            if (Page < _constraintsOption.MinPageNumber)
                errors["Page"].Add($"The page must to be greater than {_constraintsOption.MinPageNumber}");
            if (Limit < _constraintsOption.PageSize.Start.Value || Limit > _constraintsOption.PageSize.End.Value)
                errors["Limit"].Add($"The limit must to be in range [{_constraintsOption.PageSize.Start.Value},{_constraintsOption.PageSize.End.Value}]");

            errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
            if (errors.Count > 0) throw new CustomValidationException(errors); 
            
            
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

using Azure;
using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.BLL.DTOs.TestCases;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Options.Constraint;
using ProblemSolvingPlatform.DAL.Models.TestCases;
using ProblemSolvingPlatform.DAL.Repos.Problems;
using ProblemSolvingPlatform.DAL.Repos.Tests;
using ProblemSolvingPlatform.DAL.Repos.Users;
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
        private IUserRepo _userRepo;
        private IProblemRepo _problemRepo;
        public TestCaseService(ITestCaseRepo testCaseRepo, ConstraintsOption constraintsOption, IUserRepo userRepo, IProblemRepo problemRepo) {
            _testCaseRepo = testCaseRepo;
            _constraintsOption = constraintsOption;
            _userRepo = userRepo;
            _problemRepo = problemRepo;
        }

        public async Task<IEnumerable<TestCaseDTO>?> GetAllTestCasesAsync(int userId, int? ProblemID = null, bool? IsSample = null, bool? IsPublic = null) {
            
            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null) throw new CustomValidationException("user not found");
            if (ProblemID == null) throw new CustomValidationException("you must to send problemId");

            var problemCreatorId = (await _problemRepo.GetProblemByIDAsync(ProblemID.Value))?.CreatedByID;
            if (problemCreatorId == null) throw new CustomValidationException("this problem doesn't created by user");

            var testCasesModel = new List<TestCaseModel>();
            if (user.Role == DAL.Models.Enums.Role.System || userId == problemCreatorId.Value)
                testCasesModel = (await _testCaseRepo.GetAllTestCasesAsync(ProblemID, IsSample, IsPublic))?.ToList();
            else
                testCasesModel = (await _testCaseRepo.GetAllTestCasesAsync(ProblemID, IsSample, true))?.ToList();


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

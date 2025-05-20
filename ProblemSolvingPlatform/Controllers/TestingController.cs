using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Request;
using ProblemSolvingPlatform.BLL.Services.Auth;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models.Problem;
using ProblemSolvingPlatform.DAL.Models.TestCase;
using ProblemSolvingPlatform.DAL.Repos.Problem;

namespace ProblemSolvingPlatform.Controllers
{
    [ApiController]
    [Route("api/testing")]
    public class TestingController : Controller
    {
        private readonly DbContext _db;
        public TestingController(DbContext dbContext) {
            _db = dbContext;
        }

        [HttpGet("test")]
        public async Task<ActionResult<int?>> TestAsync() {
            NewProblemModel problem = new NewProblemModel() {
                CompilerName = "cc",
                CreatedBy = 1,
                Title = "Test",
                GeneralDescription = "gdTest",
                InputDescription = "iTest",
                OutputDescription = "oTest",
                Note = "noteTest",
                Tutorial = "tutTest",
                Difficulty = ProblemSolvingPlatform.DAL.Models.Enums.Difficulty.eHard,
                SolutionCode = "codeTest",
                TimeLimitMilliseconds = 1000,
                TestCases = new List<NewTestCaseModel>() {

                },
                TagIDs = new List<int>() {

                }
            };

            ProblemRepo repo = new ProblemRepo(_db);
            int? id = repo.AddProblemAsync(problem).Result;
            return Ok(id);
        }
    }
}

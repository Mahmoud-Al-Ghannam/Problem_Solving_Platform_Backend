using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Request;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Services.Auth;
using ProblemSolvingPlatform.BLL.Services.Submissions.Handling_Submission;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models.Problems;
using ProblemSolvingPlatform.DAL.Models.TestCases;
using ProblemSolvingPlatform.DAL.Repos.Problems;

namespace ProblemSolvingPlatform.Controllers
{
    [ApiController]
    [Route("api/testing")]
    public class TestingController : Controller
    {
        private readonly DbContext _db;
        private SubmissionHandler _submissionHandler { get; set; }
        public TestingController(DbContext dbContext, SubmissionHandler submissionHandler) {
            _db = dbContext;
            _submissionHandler = submissionHandler;
        }

        [HttpGet("compiletest")]
        public async Task<ActionResult> Compile()
        {
            var res = await _submissionHandler.ExecuteSubmission(1, "");
            return Ok(res);
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

        [HttpGet("exception")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> exception(int x) {
            if (x == 0) throw new Exception("Hello World!");
            else if (x == 1) {
                throw new CustomValidationException("Name", ["Was not found"]);
            }

            return Ok(x*x);
        }
    }
}

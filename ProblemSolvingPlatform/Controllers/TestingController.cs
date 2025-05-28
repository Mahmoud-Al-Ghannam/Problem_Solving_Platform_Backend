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

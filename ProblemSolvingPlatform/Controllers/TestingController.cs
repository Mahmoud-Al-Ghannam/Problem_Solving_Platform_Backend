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
using Swashbuckle.AspNetCore.Annotations;


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


        [SwaggerResponse(200,"Returns sum of two numbers")]
        [SwaggerOperation("This is summary","This is description",OperationId = "This is Operation ID")]
        [HttpGet("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> sum([FromBody] List<int> n1, [FromQuery] int x) {
            return Ok(n1.Sum() * x);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL.DTOs.TestCases;
using ProblemSolvingPlatform.BLL.Services.TestCases;

namespace ProblemSolvingPlatform.Controllers {

    [ApiController]
    [Route("/api/testcases")]
    public class TestCasesController : Controller {
        
        private readonly ITestCaseService _testCaseService;

        public TestCasesController(ITestCaseService testCaseService) {
            _testCaseService = testCaseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestCaseDTO>?>> GetAllTestCases (int Page, int Limit, int? ProblemID = null, bool? IsSample = null, bool? IsPublic = null) {
            var testcases = await _testCaseService.GetAllTestCasesAsync(Page,Limit,ProblemID,IsSample,IsPublic);
            if (testcases == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(testcases);
        }
    }
}

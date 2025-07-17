using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using ProblemSolvingPlatform.BLL.DTOs.TestCases;
using ProblemSolvingPlatform.BLL.Services.TestCases;
using ProblemSolvingPlatform.Responses;

namespace ProblemSolvingPlatform.Controllers {

    [ApiController]
    [Route("/api/test-cases")]
    public class TestCasesController : GeneralController {
        
        private readonly ITestCaseService _testCaseService;

        public TestCasesController(ITestCaseService testCaseService) {
            _testCaseService = testCaseService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TestCaseDTO>?>> GetAllTestCases (int Page = BLL.Constants.PaginationDefaultValues.Page, int Limit = BLL.Constants.PaginationDefaultValues.Limit, int? ProblemID = null, bool? IsSample = null, bool? IsPublic = null) {
            var testcases = await _testCaseService.GetAllTestCasesAsync(Page,Limit,ProblemID,IsSample,IsPublic);
            if (testcases == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
            return Ok(testcases);
        }
    }
}

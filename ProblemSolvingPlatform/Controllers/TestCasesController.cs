using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs.TestCases;
using ProblemSolvingPlatform.BLL.Services.TestCases;
using ProblemSolvingPlatform.Responses;

namespace ProblemSolvingPlatform.Controllers
{

    [ApiController]
    [Route($"/{Constants.Api.PrefixApi}/test-cases")]
    public class TestCasesController : GeneralController
    {

        private readonly ITestCaseService _testCaseService;

        public TestCasesController(ITestCaseService testCaseService)
        {
            _testCaseService = testCaseService;
        }

        /// <summary>
        /// JWt Bearer Auth
        /// </summary>
        /// <param name="ProblemID"></param>
        /// <param name="IsSample"></param>
        /// <param name="IsPublic"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TestCaseDTO>?>> GetAllTestCases([FromQuery] int? ProblemID = null, [FromQuery] bool? IsSample = null, [FromQuery] bool? IsPublic = null)
        {
            var testcases = await _testCaseService.GetAllTestCasesAsync(ProblemID, IsSample, IsPublic);
            if (testcases == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return Ok(testcases);
        }
    }
}

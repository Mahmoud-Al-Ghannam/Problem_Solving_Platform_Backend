using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs.TestCases;
using ProblemSolvingPlatform.BLL.Services.TestCases;
using ProblemSolvingPlatform.Responses;

namespace ProblemSolvingPlatform.Controllers.Public
{

    [ApiController]
    [Route($"/{Constants.Api.PrefixPublicApi}/test-cases")]
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
        /// <param name="Page"></param>
        /// <param name="Limit"></param>
        /// <param name="ProblemID"></param>
        /// <param name="IsSample"></param>
        /// <param name="IsPublic"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TestCaseDTO>?>> GetAllTestCases(int Page = Constants.PaginationDefaultValues.Page, int Limit = Constants.PaginationDefaultValues.Limit, int? ProblemID = null, bool? IsSample = null, bool? IsPublic = null)
        {
            var testcases = await _testCaseService.GetAllTestCasesAsync(Page, Limit, ProblemID, IsSample, IsPublic);
            if (testcases == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return Ok(testcases);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Request;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submission;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Services.Auth;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models.Problems;
using ProblemSolvingPlatform.DAL.Models.TestCases;
using ProblemSolvingPlatform.DAL.Repos.Problems;
using ProblemSolvingPlatform.Responses;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection.Metadata;


namespace ProblemSolvingPlatform.Controllers
{
    [ApiController]
    [Route($"{Constants.Api.PrefixPublicApi}/testing")]
    public class TestingController : GeneralController {
        private readonly DbContext _db;
        public TestingController(DbContext dbContext) {
            _db = dbContext;
        }


        /// <summary>
        /// Summary two
        /// </summary>
        /// <remarks>
        ///     Hello Hello
        /// </remarks>
        /// <response code="200">
        ///     {
        ///         hello hello hello man hi
        ///     }
        /// </response>
        /// <param name="n1">this is first parm</param>
        /// <param name="x">this is second parm</param>
        /// <returns>returns some thing </returns>
        [HttpGet("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> sum([FromBody] List<int> n1, [FromQuery] int x) {
            return Ok(n1.Sum() * x);
        }
    }
}

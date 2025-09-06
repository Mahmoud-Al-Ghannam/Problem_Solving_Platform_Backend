using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.Services.Statistics;
using ProblemSolvingPlatform.DAL.DTOs;

namespace ProblemSolvingPlatform.Controllers;



[ApiController]
[Route($"/{Constants.Api.PrefixApi}/[controller]")]
public class StatisticsController : ControllerBase
{
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public ActionResult<GeneralStatistics> GetGeneralStatistics([FromQuery] bool? DeletedFilter)
        {
            var result = _statisticsService.GetGeneralStatistics(DeletedFilter);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("problems/{problemId}")]
        public ActionResult<ProblemStatsDto> GetProblemStatistics(int problemId)
        {
            var result = _statisticsService.GetProblemStats(problemId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("problems/tags")]
        public ActionResult<List<ProblemTagStatsDto>> GetProblemTagStats(int? tagId = null, bool? DeletedFilter = null)
        {
            var result = _statisticsService.GetProblemTagStats(tagId, DeletedFilter);
            return Ok(result);
        }

        [HttpGet("users/{userId}")]
        public ActionResult<UserStatsDto> GetUserStats(int userId, bool? DeletedFilter)
        {
            var result = _statisticsService.GetUserStats(userId, DeletedFilter);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("users/{userId}/activity")]
        public ActionResult<UserActivityStatsDto> GetUserActivityStats(int userId, bool? DeletedFilter)
        {
            var result = _statisticsService.GetUserActivityStats(userId, DeletedFilter);
            return Ok(result);
        }

  

}

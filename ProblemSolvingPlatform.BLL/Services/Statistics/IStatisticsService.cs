using ProblemSolvingPlatform.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Statistics;

public interface IStatisticsService
{
    GeneralStatistics GetGeneralStatistics(bool? DeletedFilter);
    ProblemStatsDto GetProblemStats(int problemId);
    List<ProblemTagStatsDto> GetProblemTagStats(int? tagId, bool? DeletedFilter);
    UserStatsDto GetUserStats(int userId, bool? DeletedFilter);
    UserActivityStatsDto GetUserActivityStats(int userId, bool? DeletedFilter);
}

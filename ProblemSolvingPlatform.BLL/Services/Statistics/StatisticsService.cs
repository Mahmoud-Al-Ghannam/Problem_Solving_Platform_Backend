using Microsoft.AspNetCore.Razor.TagHelpers;
using ProblemSolvingPlatform.DAL.DTOs;
using ProblemSolvingPlatform.DAL.Repos.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.DAL.Models.Enums;

namespace ProblemSolvingPlatform.BLL.Services.Statistics;

public class StatisticsService : IStatisticsService
{
    private readonly IStatisticsRepo _repo;

    public StatisticsService(IStatisticsRepo repo)
    {
        _repo = repo;
    }

    public GeneralStatistics GetGeneralStatistics(bool? DeletedFilter)
    {
        var result = _repo.GetGeneralStatistics(DeletedFilter);

        foreach (SubmissionStatus status in Enum.GetValues(typeof(SubmissionStatus)))
        {
            var key = status.ToString();
            if(!result.NumberOfSubmissionsByStatus.ContainsKey(key))
            {
                result.NumberOfSubmissionsByStatus[key] = 0;
            }
        }

        foreach(Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
        {
            var key = difficulty.ToString();
            if(!result.NumberOfProblemsByDifficutly.ContainsKey(key))
            {
                result.NumberOfProblemsByDifficutly[key] = 0;
            }
        }

        return result;  
    }

    public ProblemStatsDto GetProblemStats(int problemId)
    {
        var result = _repo.GetProblemStats(problemId);

        foreach(SubmissionStatus status in Enum.GetValues(typeof(SubmissionStatus)))
        {
            var key = status.ToString();
            if (!result.NumberOfSubmissionsByStatus.ContainsKey(key))
                result.NumberOfSubmissionsByStatus[key] = 0;
        }
        return result;
    }


    public List<ProblemTagStatsDto> GetProblemTagStats(int? tagId, bool? DeletedFilter)
        => _repo.GetProblemTagStats(tagId, DeletedFilter);

    public UserStatsDto GetUserStats(int userId, bool? DeletedFilter)
        => _repo.GetUserStats(userId, DeletedFilter);

    public UserActivityStatsDto GetUserActivityStats(int userId, bool? DeletedFilter)
        => _repo.GetUserActivityStats(userId, DeletedFilter);
}

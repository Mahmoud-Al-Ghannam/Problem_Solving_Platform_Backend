using Microsoft.Data.SqlClient;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.DTOs;
using ProblemSolvingPlatform.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.DAL.Models.Enums;

namespace ProblemSolvingPlatform.DAL.Repos.Statistics;

public class StatisticsRepo : IStatisticsRepo
{
    private readonly DbContext _db;

    public StatisticsRepo(DbContext dbContext)
    {
        _db = dbContext;
    }

    public GeneralStatistics GetGeneralStatistics(bool? DeletedFilter)
    {
        var result = new GeneralStatistics();

        using var connection = _db.GetConnection();
        using var cmd = new SqlCommand("SP_Statistics_GetGeneral", connection);
        cmd.CommandType = CommandType.StoredProcedure;

        int filterVal = DeletedFilter switch
        {
            true => 1, // deleted
            false => 0, // non
            null => 2 // all
        };

        cmd.Parameters.AddWithValue("@DeletedFilter", filterVal);

        connection.Open();
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
            result.NumberOfActiveUsers = reader.GetInt32(0);

        if (reader.NextResult() && reader.Read())
            result.NumberOfInActiveUsers = reader.GetInt32(0);

        if (reader.NextResult() && reader.Read())
            result.NumberOfProblems = reader.GetInt32(0);

        if (reader.NextResult() && reader.Read())
            result.NumberOfSubmissions = reader.GetInt32(0);

        if (reader.NextResult() && reader.Read())
            result.NumberOfTags = reader.GetInt32(0);

        // Problems by Tag
        result.NumberOfProblemsByTag = new Dictionary<string, int>();
        if (reader.NextResult())
        {
            while (reader.Read())
                result.NumberOfProblemsByTag.Add(reader.GetString(0), reader.GetInt32(1));
        }

        // Submissions by Compiler
        result.NumberOfSubmissionsByCompiler = new Dictionary<string, int>();
        if (reader.NextResult())
        {
            while (reader.Read())
                result.NumberOfSubmissionsByCompiler.Add(reader.GetString(0), reader.GetInt32(1));
        }

        // Submissions by Status
        result.NumberOfSubmissionsByStatus = new Dictionary<string, int>();
        if (reader.NextResult())
        {
            while (reader.Read())
                result.NumberOfSubmissionsByStatus.Add(((SubmissionStatus)reader.GetByte(0)).ToString(), reader.GetInt32(1));
        }

        // Problems by Difficulty
        result.NumberOfProblemsByDifficulty = new Dictionary<string, int>();
        if (reader.NextResult())
        {
            while (reader.Read())
                result.NumberOfProblemsByDifficulty.Add(((Difficulty)reader.GetByte(0)).ToString(), reader.GetInt32(1));
        }

        return result;
    }

    public ProblemStatsDto GetProblemStats(int problemId)
    {
        var result = new ProblemStatsDto
        {
            Tags = new List<string>(),
            NumberOfSubmissionsByStatus = new Dictionary<string, int>()
        };

        using var connection = _db.GetConnection();
        using var cmd = new SqlCommand("SP_Statistics_GetProblemStats", connection);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ProblemId", problemId);

        connection.Open();
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result.ProblemId = reader.GetInt32(reader.GetOrdinal("ProblemID"));
            result.Title = reader.GetString(reader.GetOrdinal("Title"));
            result.Difficulty = ((Difficulty)reader.GetByte(reader.GetOrdinal("Difficulty"))).ToString();
            result.TotalSubmissions = reader.GetInt32(reader.GetOrdinal("TotalSubmissions"));
            result.TotalAccepted = reader.GetInt32(reader.GetOrdinal("TotalAccepted"));
            result.AcceptanceRate = reader.GetDouble(reader.GetOrdinal("AcceptanceRate"));
            result.UniqueUsersSolved = reader.GetInt32(reader.GetOrdinal("UniqueUsersSolved"));
            result.NumberOfTestCases = reader.GetInt32(reader.GetOrdinal("NumberOfTestCases"));
        }

        if (reader.NextResult())
        {
            while (reader.Read())
                result.Tags.Add(reader.GetString(0));
        }

        if (reader.NextResult())
        {
            while (reader.Read())
                result.NumberOfSubmissionsByStatus.Add(((SubmissionStatus)reader.GetByte(0)).ToString(), reader.GetInt32(1));
        }

        return result;
    }

    public List<ProblemTagStatsDto> GetProblemTagStats(int? tagId, bool? DeletedFilter)
    {
        var list = new List<ProblemTagStatsDto>();

        using var connection = _db.GetConnection();
        using var cmd = new SqlCommand("SP_Statistics_GetProblemTagStats", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        int filterVal = DeletedFilter switch
        {
            true => 1, // deleted
            false => 0, // non
            null => 2 // all
        };

        cmd.Parameters.AddWithValue("@DeletedFilter", filterVal);
        cmd.Parameters.AddWithValue("@TagId", tagId.HasValue ? tagId.Value : (object)DBNull.Value);

        connection.Open();
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new ProblemTagStatsDto
            {
                TagId = reader.GetInt32(reader.GetOrdinal("TagID")),
                TagName = reader.GetString(reader.GetOrdinal("TagName")),
                NumberOfProblems = reader.GetInt32(reader.GetOrdinal("NumberOfProblems")), 
                UsersSolved = reader.GetInt32(reader.GetOrdinal("UsersSolved")),
                TotalAttempts = reader.GetInt32(reader.GetOrdinal("TotalAttempts")),
                SuccessRate = reader.GetDouble(reader.GetOrdinal("SuccessRate"))
            });
        }

        return list;
    }

    public UserStatsDto GetUserStats(int userId, bool? DeletedFilter)
    {
        var result = new UserStatsDto
        {
            SolvedByTag = new Dictionary<string, int>(),
            NumberOfSubmissionsByStatus = new Dictionary<string, int>()
        };

        using var connection = _db.GetConnection();
        using var cmd = new SqlCommand("SP_Statistics_GetUserStats", connection);
        cmd.CommandType = CommandType.StoredProcedure;

        int filterVal = DeletedFilter switch
        {
            true => 1, // deleted
            false => 0, // non
            null => 2 // all
        };

        cmd.Parameters.AddWithValue("@DeletedFilter", filterVal);
        cmd.Parameters.AddWithValue("@UserId", userId);

        connection.Open();
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            result.UserId = reader.GetInt32(reader.GetOrdinal("UserID"));
            result.Username = reader.GetString(reader.GetOrdinal("Username"));
            result.TotalSolved = reader.GetInt32(reader.GetOrdinal("TotalSolved"));
            result.TotalAttempts = reader.GetInt32(reader.GetOrdinal("TotalAttempts"));
            result.AcceptanceRate = reader.GetDouble(reader.GetOrdinal("AcceptanceRate"));
            result.EasySolved = reader.GetInt32(reader.GetOrdinal("EasySolved"));
            result.MediumSolved = reader.GetInt32(reader.GetOrdinal("MediumSolved"));
            result.HardSolved = reader.GetInt32(reader.GetOrdinal("HardSolved"));
            result.NumberOfCreatedProblem = reader.GetInt32(reader.GetOrdinal("NumberOfCreatedProblem"));
        }

        if (reader.NextResult())
        {
            while (reader.Read())
                result.NumberOfSubmissionsByStatus.Add(((SubmissionStatus)reader.GetByte(0)).ToString(), reader.GetInt32(1));
        }

        if (reader.NextResult())
        {
            while (reader.Read())
                result.SolvedByTag[reader.GetString(0)] = reader.GetInt32(1);
        }

        return result;
    }

    public UserActivityStatsDto GetUserActivityStats(int userId, bool? DeletedFilter)
    {
        var result = new UserActivityStatsDto
        {
            SubmissionsPerDay = new Dictionary<DateTime, DailyActivityDto>()
        };

        using var connection = _db.GetConnection();
        using var cmd = new SqlCommand("SP_Statistics_GetUserActivity", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        int filterVal = DeletedFilter switch
        {
            true => 1, // deleted
            false => 0, // non
            null => 2 // all
        };

        cmd.Parameters.AddWithValue("@DeletedFilter", filterVal);
        cmd.Parameters.AddWithValue("@UserId", userId);

        connection.Open();
        using var reader = cmd.ExecuteReader();

        DateTime? prevDate = null;
        int currentStreak = 0, longestStreak = 0;

        while (reader.Read())
        {
            var date = reader.GetDateTime(reader.GetOrdinal("SubmissionDate"));
            var submissionCount = reader.GetInt32(reader.GetOrdinal("SubmissionCount"));
            var uniqueSolved = reader.GetInt32(reader.GetOrdinal("UniqueSolved"));

            int level = uniqueSolved switch
            {
                0 => (submissionCount == 0? 0 : 1),
                1 => 2,
                2 => 3,
                3 => 4,
                _ => 5
            };

            result.SubmissionsPerDay[date] = new DailyActivityDto
            {
                SubmissionCount = submissionCount,
                Level = level
            };

            // Handle streaks
            if (prevDate != null && (date - prevDate.Value).Days == 1)
                currentStreak++;
            else
                currentStreak = 1;

            longestStreak = Math.Max(longestStreak, currentStreak);
            prevDate = date;
        }

        result.CurrentStreak = currentStreak;
        result.LongestStreak = longestStreak;

        return result;
    }

}

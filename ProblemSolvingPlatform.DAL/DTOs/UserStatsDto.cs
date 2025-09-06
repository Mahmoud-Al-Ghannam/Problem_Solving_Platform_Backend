using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.DTOs;

public class UserStatsDto
{
    public string Username { get; set; }
    public int UserId { get; set; }
    public int TotalSolved { get; set; }
    public int TotalAttempts { get; set; }
    public double AcceptanceRate { get; set; }
    public int EasySolved { get; set; }
    public int MediumSolved { get; set; }
    public int HardSolved { get; set; }
    public Dictionary<string, int> SolvedByTag { get; set; }
    public Dictionary<string, int> NumberOfSubmissionsByStatus { get; set; } // for each status, number of submissions 
    public int NumberOfCreatedProblem { get; set; }
}

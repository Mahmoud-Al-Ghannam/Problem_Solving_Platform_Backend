using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.DAL.Models.Enums;

namespace ProblemSolvingPlatform.DAL.DTOs;

public class ProblemStatsDto
{
    public int ProblemId { get; set; }
    public string Title { get; set; }
    public string Difficulty { get; set; }
    public int TotalSubmissions { get; set; }
    public int TotalAccepted { get; set; }
    public double AcceptanceRate { get; set; }
    public int UniqueUsersSolved { get; set; }
    public int NumberOfTestCases { get; set; }
    public List<string> Tags { get; set; }
    public Dictionary<string, int> NumberOfSubmissionsByStatus { get; set; }  // num, wronganser, ... 
}

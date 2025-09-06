using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.DTOs;

public class GeneralStatistics
{
    public int NumberOfProblems { get; set; }
    public int NumberOfSubmissions { get; set; }
    public int NumberOfTags { get; set; }
    public int NumberOfActiveUsers { get; set; }
    public int NumberOfInActiveUsers { get; set; }
    public Dictionary<string, int> NumberOfProblemsByTag { get; set; }
    public Dictionary<string, int> NumberOfSubmissionsByCompiler { get; set; }
    public Dictionary<string, int> NumberOfSubmissionsByStatus { get; set; }
    public Dictionary<string, int> NumberOfProblemsByDifficutly { get; set; }
}

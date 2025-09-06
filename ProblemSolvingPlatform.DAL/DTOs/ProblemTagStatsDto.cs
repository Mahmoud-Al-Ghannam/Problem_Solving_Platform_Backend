using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.DTOs;

public class ProblemTagStatsDto
{
    public int TagId { get; set; }
    public string TagName { get; set; }
    public int NumberOfProblems { get; set; } 
    public int UsersSolved { get; set; }
    public int TotalAttempts { get; set; }
    public double SuccessRate { get; set; }
}


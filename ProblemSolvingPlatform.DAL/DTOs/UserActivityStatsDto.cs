using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.DTOs;

public class UserActivityStatsDto
{
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public Dictionary<DateTime, DailyActivityDto> SubmissionsPerDay { get; set; }
    
}

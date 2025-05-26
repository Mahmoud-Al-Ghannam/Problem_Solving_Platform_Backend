using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Compiler.DTOs;

public class ExecuteResponseDTO
{
    public string? StandardOut { get; set; }
    public string? StandardError { get; set; }
    public double? ExecutionTimeMs { get; set; }
    public int? MemoryKB { get; set; }
    public int? ExitCode { get; set; }

    public bool Success => ExitCode == 0 && string.IsNullOrWhiteSpace(StandardError);
}

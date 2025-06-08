using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.Submissions.Submit;

public class SubmitDTO
{
    [Required]
    public int ProblemId { get; set; }
    [Required]
    public string CompilerName { get; set; }
    [Required]
    public required string Code { get; set; }

    public byte VisionScope { get; set; } = (byte)Enums.VisionScope.all;
}

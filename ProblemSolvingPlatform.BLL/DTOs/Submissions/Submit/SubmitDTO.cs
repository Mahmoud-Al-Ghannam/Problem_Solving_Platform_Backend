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
    public int ProblemId { get; set; }
    public string CompilerName { get; set; }
    public required string Code { get; set; }

    public Enums.VisionScope VisionScope { get; set; } = Enums.VisionScope.all;
}

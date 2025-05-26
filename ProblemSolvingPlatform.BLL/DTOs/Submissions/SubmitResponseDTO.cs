using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.Submissions;

public class SubmitResponseDTO
{
    public bool isSuccess { get; set; }  

    public string msg { get; set; }
    public int submissionId {  get; set; }
    public string Status { get; set; }
}

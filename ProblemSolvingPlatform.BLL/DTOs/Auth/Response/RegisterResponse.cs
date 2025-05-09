using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.Auth.Response;

public class RegisterResponse
{
    [Required]
    public string message { get; set; }
    [Required]
    public bool IsSuccess { get; set; }
    [Required]
    public int statusCode { get; set; }
}

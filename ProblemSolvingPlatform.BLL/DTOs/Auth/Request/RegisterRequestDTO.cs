using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProblemSolvingPlatform.BLL.DTOs.Auth.Request;

public class RegisterRequestDTO
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    
    public IFormFile? ProfileImage { get; set; }
}

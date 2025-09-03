using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.Auth.Response;

public class LoginResponseDTO
{
    public string Token { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
}

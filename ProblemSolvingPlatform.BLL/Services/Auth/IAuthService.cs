using ProblemSolvingPlatform.BLL.DTOs.Auth.Request;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Auth;

public interface IAuthService
{
    public Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO registerDTO);
    public Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginDTO);
}

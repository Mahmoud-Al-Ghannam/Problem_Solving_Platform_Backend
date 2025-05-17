using ProblemSolvingPlatform.BLL.DTOs.Auth.Request;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Response;
using ProblemSolvingPlatform.BLL.DTOs.UserProfile;
using ProblemSolvingPlatform.DAL.DTOs.Auth.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Auth;

public interface IAuthService
{
    public Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO registerDTO);
    public Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginDTO);

    public Task<bool> ChangePasswordAsync(int userId , ChangePasswordDTO changePasswordDTO);

}

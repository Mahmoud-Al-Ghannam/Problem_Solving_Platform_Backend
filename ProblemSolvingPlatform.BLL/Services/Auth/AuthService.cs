using ProblemSolvingPlatform.BLL.DTOs.Auth.Request;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Response;
using ProblemSolvingPlatform.BLL.Services.JWT;
using ProblemSolvingPlatform.DAL.DTOs.Auth.Request;
using ProblemSolvingPlatform.DAL.DTOs.UserProfile;
using ProblemSolvingPlatform.DAL.Repos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Auth;

public class AuthService : IAuthService
{
    private IUserRepo _userRepo {  get; set; }
    private TokenService _tokenService { get; set; }
    public AuthService(IUserRepo userRepo, TokenService tokenService)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
    }


    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginDTO)
    {
        var user = await _userRepo.GetUserByUsernameAndPassword(loginDTO.Username, loginDTO.Password);
        if (user == null)
        {
            return new LoginResponseDTO()
            {
                Success = false,
                StatusCode = 401,
                Message = "wrong username/password :)"
            };
        }

        // make a token and return the response
        return new LoginResponseDTO()
        {
            Success = true,
            StatusCode = 200,
            Message = "Success",
            Token = _tokenService.GenerateToken(user)
        };
    }

    public async Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO registerDTO)
    {
        if (await _userRepo.DoesUserExistByUsername(registerDTO.Username))
            return new RegisterResponseDTO() { IsSuccess = false, statusCode = 400, message = "Username is already exist" };

        UserDTO user = new()
        {
            Username = registerDTO.Username,
            Password = registerDTO.Password,
            ImagePath = "KOKO"
        };
        var res = await _userRepo.AddUser(user);
        if (!res.HasValue)
            return new RegisterResponseDTO()
            {
                IsSuccess = false,
                statusCode = 500,
                message = "Invalid, try again :)"
            };

        return new RegisterResponseDTO()
        {
            IsSuccess = true,
            statusCode = 200,
            message = "User Registered Successfully"
        };

    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO changePasswordDTO)
                               => await _userRepo.ChangePasswordAsync(userId, changePasswordDTO);
    

}

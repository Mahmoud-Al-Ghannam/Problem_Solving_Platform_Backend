using ProblemSolvingPlatform.BLL.DTOs.Auth.Request;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Response;
using ProblemSolvingPlatform.BLL.Services.JWT;
using ProblemSolvingPlatform.DAL.Interfaces;
using ProblemSolvingPlatform.DAL.Models;
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


    public async Task<LoginResult> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _userRepo.GetUserByUsernameAndPassword(loginDTO.Username, loginDTO.Password);
        if (user == null)
        {
            return new LoginResult()
            {
                Success = false,
                StatusCode = 401,
                Message = "wrong username/password :)"
            };
        }

        // make a token and return the response
        return new LoginResult()
        {
            Success = true,
            StatusCode = 200,
            Message = "Success",
            Token = _tokenService.GenerateToken(user)
        };
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterDTO registerDTO)
    {
        if (await _userRepo.IsUsernameExist(registerDTO.Username))
            return new RegisterResponse() { IsSuccess = false, statusCode = 400, message = "Username is already exist" };

        User user = new()
        {
            Username = registerDTO.Username,
            Password = registerDTO.Password,
            ImagePath = "KOKO"
        };
        var res = await _userRepo.AddUser(user);
        if (!res)
            return new RegisterResponse()
            {
                IsSuccess = false,
                statusCode = 500,
                message = "Invalid, try again :)"
            };

        return new RegisterResponse()
        {
            IsSuccess = true,
            statusCode = 200,
            message = "User Registered Successfully"
        };

    }
}

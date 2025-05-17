using ProblemSolvingPlatform.BLL.DTOs.Auth.Request;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Response;
using ProblemSolvingPlatform.BLL.Services.JWT;
using ProblemSolvingPlatform.DAL.DTOs.Auth.Request;
using ProblemSolvingPlatform.DAL.Repos.User;

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



    public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginDTO)
    {
        var user = await _userRepo.GetUserByUsernameAndPassword(loginDTO.Username, loginDTO.Password);
        if (user == null)
        {
            return new AuthResponseDTO()
            {
                Success = false,
                StatusCode = 401,
                Message = "wrong username/password :)"
            };
        }

        // make a token and return the response
        return new AuthResponseDTO()
        {
            Success = true,
            StatusCode = 200,
            Message = "Login successful :)",
            Token = _tokenService.GenerateToken(user)
        };
    }

    public async Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO registerDTO)
    {
        if (await _userRepo.DoesUserExistByUsername(registerDTO.Username))
            return new AuthResponseDTO() { Success = false, StatusCode = 400, Message = "Username is already exist" };

        
        string? imagePath = await FileService.SaveImageAndGetURL(registerDTO.ProfileImage);

        ProblemSolvingPlatform.DAL.Models.User user = new()
        {
            Username = registerDTO.Username,
            Password = registerDTO.Password,
            ImagePath = imagePath
        };
        var userId = await _userRepo.AddUser(user);
        if (!userId.HasValue)
            return new AuthResponseDTO() { Success = false, StatusCode = 500, Message = "Invalid, try again :)" };

        
        user.UserId = userId.Value;
        return new AuthResponseDTO()
        {
            Success = true,
            StatusCode = 200,
            Message = "User Registered Successfully",
            Token = _tokenService.GenerateToken(user)
        };

    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO changePasswordDTO)
                               => await _userRepo.ChangePasswordAsync(userId, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
    

}

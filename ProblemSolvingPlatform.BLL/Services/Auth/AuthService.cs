using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Request;
using ProblemSolvingPlatform.BLL.DTOs.Auth.Response;
using ProblemSolvingPlatform.BLL.DTOs.Submissions.Submit;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Options.Constraint;
using ProblemSolvingPlatform.BLL.Services.JWT;
using ProblemSolvingPlatform.DAL.DTOs.Auth.Request;
using ProblemSolvingPlatform.DAL.Models.Users;
using ProblemSolvingPlatform.DAL.Repos.Users;

namespace ProblemSolvingPlatform.BLL.Services.Auth;

public class AuthService : IAuthService {
    private readonly IUserRepo _userRepo;
    private readonly ITokenService _tokenService;
    private readonly ConstraintsOption _constraintsOption;
    public AuthService(IUserRepo userRepo, ITokenService tokenService, ConstraintsOption constraintsOption) {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _constraintsOption = constraintsOption;
    }

    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginDTO,Enums.Role? role) {
        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        errors["Username"] = [];
        errors["Password"] = [];
        errors["$"] = [];

        if (string.IsNullOrEmpty(loginDTO.Username))
            errors["Username"].Add("The username is required");

        if (string.IsNullOrEmpty(loginDTO.Password))
            errors["Password"].Add("The password is required");

        if (loginDTO.Username.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
            errors["Username"].Add("The username should only consists of letters, digits and '_'");

        var user = await _userRepo.GetUserByUsernameAndPasswordAsync(loginDTO.Username, loginDTO.Password);
        if (user == null) {
            errors["$"].Add("The username or password is wrong");
        } 
        else if (!user.IsActive) {
            errors["$"].Add($"The user with username = {loginDTO.Username} and password = {loginDTO.Password} was not active");
        }
        else if (role != null && (Enums.Role)(byte)user.Role != role) {
            errors["$"].Add($"The username or password is wrong");
        }


        errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
        if (errors.Count > 0) throw new CustomValidationException(errors);

        var token = _tokenService.GenerateToken(user.UserId, (Enums.Role)user.Role);
        return new LoginResponseDTO()
        {
            Token = token,
            UserId = user.UserId,
            UserName = user.Username
        };
    }

    public async Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO registerDTO) {
        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        errors["Username"] = [];
        errors["Password"] = [];

        if (string.IsNullOrEmpty(registerDTO.Username))
            errors["Username"].Add("The username is required");
        else {
            if (registerDTO.Username.Length > _constraintsOption.User.UsernameLength.End.Value || registerDTO.Username.Length < _constraintsOption.User.UsernameLength.Start.Value)
                errors["Username"].Add($"The length of username must to be in range [{_constraintsOption.User.UsernameLength.Start.Value},{_constraintsOption.User.UsernameLength.End.Value}]");
        }

        if (registerDTO.Username.Any(c => !char.IsLetterOrDigit(c) && c != '_'))
            errors["Username"].Add("The username should only consists of letters, digits and '_'");

        if (string.IsNullOrEmpty(registerDTO.Password))
            errors["Password"].Add("The password is required");
        else {
            if (registerDTO.Password.Length > _constraintsOption.User.PasswordLength.End.Value || registerDTO.Password.Length < _constraintsOption.User.PasswordLength.Start.Value)
                errors["Password"].Add($"The length of password must to be in range [{_constraintsOption.User.PasswordLength.Start.Value},{_constraintsOption.User.PasswordLength.End.Value}]");
        }

        if (await _userRepo.DoesUserExistByUsernameAsync(registerDTO.Username))
            errors["Username"].Add($"The user with username = {registerDTO.Username} is already exists");


        errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
        if (errors.Count > 0) throw new CustomValidationException(errors);


        string? imagePath = await FileService.SaveImageAndGetURL(registerDTO.ProfileImage);

        DAL.Models.Users.UserModel user = new() {
            Username = registerDTO.Username,
            Password = registerDTO.Password,
            ImagePath = imagePath,
            Role = DAL.Models.Enums.Role.User
        };
        var userId = await _userRepo.AddUserAsync(user);
        if (!userId.HasValue)
            throw new Exception(Constants.ErrorMessages.General);

        user.UserId = userId.Value;
        string token = _tokenService.GenerateToken(user.UserId, (BLL.DTOs.Enums.Role)(byte)user.Role);

        user.UserId = userId.Value;
        return new RegisterResponseDTO() {
            UserID = user.UserId,
            Username = user.Username,
            Token = token
        };
    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO changePasswordDTO) {
        Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        errors["OldPassword"] = [];
        errors["NewPassword"] = [];
        errors["UserID"] = [];

        if (!await _userRepo.DoesUserExistByIDAsync(userId))
            errors["UserID"].Add($"The user with id = {userId} was not found");
        else {
            UserModel? user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null) throw new Exception(Constants.ErrorMessages.General);

            UserModel? user1 = await _userRepo.GetUserByUsernameAndPasswordAsync(user.Username,changePasswordDTO.OldPassword);
            if (user1 == null) errors["OldPassword"].Add("The old password is wrong");
        }

        if (string.IsNullOrEmpty(changePasswordDTO.NewPassword))
            errors["NewPassword"].Add("The new password is required");
        else {
            if (changePasswordDTO.NewPassword.Length > _constraintsOption.User.PasswordLength.End.Value || changePasswordDTO.NewPassword.Length < _constraintsOption.User.PasswordLength.Start.Value)
                errors["NewPassword"].Add($"The length of new password must to be in range [{_constraintsOption.User.PasswordLength.Start.Value},{_constraintsOption.User.PasswordLength.End.Value}]");
        }

        errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
        if (errors.Count > 0) throw new CustomValidationException(errors);

        return await _userRepo.ChangePasswordAsync(userId, changePasswordDTO.OldPassword, changePasswordDTO.NewPassword);
    }

}

using Microsoft.AspNetCore.Http;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Users;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Options.Constraint;
using ProblemSolvingPlatform.DAL.Repos.Users;
using System.Collections.Immutable;
using System.Security.Claims;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;
using static System.Net.Mime.MediaTypeNames;


namespace ProblemSolvingPlatform.BLL.Services.Users;

public class UserService : IUserService {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ConstraintsOption _constraintsOption;
    private IUserRepo _userRepo { get; }
    public UserService(IUserRepo userRepo, IHttpContextAccessor httpContextAccessor, ConstraintsOption constraintsOption) {
        _userRepo = userRepo;
        _httpContextAccessor = httpContextAccessor;
        _constraintsOption = constraintsOption;
    }

    public async Task<UserDTO?> GetUserByIdAsync(int userId) {
        if (!await _userRepo.DoesUserExistByIDAsync(userId))
            throw new CustomValidationException("UserID", [$"The user with id = {userId} was not fount"]);

        var user = await _userRepo.GetUserByIdAsync(userId);
        if (user == null) throw new Exception(Constants.ErrorMessages.General);

        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request?.Host}";

        return new UserDTO() {
            UserID = user.UserId,
            Username = user.Username,
            CreatedAt = user.CreatedAt,
            Role = (Role)user.Role,
            ImagePath = user.ImagePath != null ? $"{baseUrl}/Images/{user.ImagePath}" : null,
            IsActive = user.IsActive
        };
    }

    public async Task<UserDTO?> GetUserByUsernameAsync(string username) {
        if (!await _userRepo.DoesUserExistByUsernameAsync(username))
            throw new CustomValidationException("Useranme", [$"The user with username = {username} was not fount"]);

        var user = await _userRepo.GetUserByUsernameAsync(username);
        if (user == null) throw new Exception(Constants.ErrorMessages.General);

        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request?.Host}";

        return new UserDTO() {
            UserID = user.UserId,
            Username = user.Username,
            CreatedAt = user.CreatedAt,
            Role = (Role)user.Role,
            ImagePath = user.ImagePath != null ? $"{baseUrl}/Images/{user.ImagePath}" : null,
            IsActive = user.IsActive
        };
    }

    public async Task<bool> UpdateUserInfoByIdAsync(int userId, UpdateUserDTO updateUser) {
        if (!await _userRepo.DoesUserExistByIDAsync(userId))
            throw new CustomValidationException("UserID", [$"The user with id = {userId} was not fount"]);

        if (updateUser == null)
            throw new CustomValidationException("UpdateUser", [$"This field is required"]);


        // 1 remove the image from the server 
        var user = await _userRepo.GetUserByIdAsync(userId);
        if (user == null) throw new Exception(Constants.ErrorMessages.General);


        if (updateUser.ProfileImage != null) {
            var oldPath = user.ImagePath ?? "";
            string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", oldPath);
            if (File.Exists(oldImagePath))
                File.Delete(oldImagePath);
        }
        // 2 
        string newpath = await FileService.SaveImageAndGetURL(updateUser.ProfileImage);
        return await _userRepo.UpdateUserInfoByIdAsync(userId, newpath);
    }

    public async Task<PageDTO<UserDTO>?> GetAllUsersAsync(int page, int limit, string? username = null, bool? isActive = null) {
        Dictionary<string, List<string>> errors = new();
        errors["Page"] = [];
        errors["Limit"] = [];

        if (page < _constraintsOption.MinPageNumber)
            errors["Page"].Add($"The page must to be greater than {_constraintsOption.MinPageNumber}");

        if (limit < _constraintsOption.PageSize.Start.Value || limit > _constraintsOption.PageSize.End.Value)
            errors["Limit"].Add($"The limit must to be in range [{_constraintsOption.PageSize.Start.Value},{_constraintsOption.PageSize.End.Value}]");


        errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
        if (errors.Count > 0) throw new CustomValidationException(errors);

        var pageModel = await _userRepo.GetAllUsersAsync(page, limit, username, isActive);
        if (pageModel == null) return null;

        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request?.Host}";

        return new PageDTO<UserDTO>() {
            Items = pageModel.Items.Select(x => new UserDTO() {
                UserID = x.UserId,
                Username = x.Username,
                CreatedAt = x.CreatedAt,
                ImagePath = x.ImagePath != null ? $"{baseUrl}/Images/{x.ImagePath}" : null,
                Role = (Role)x.Role,
                IsActive = x.IsActive,
            }).ToList(),

            TotalItems = pageModel.TotalItems,
            TotalPages = pageModel.TotalPages,
            CurrentPage = pageModel.CurrentPage
        };
    }

    public async Task<bool> DoesUserExistByUsernameAsync(string username) {
        return await _userRepo.DoesUserExistByUsernameAsync(username);
    }

    public async Task<bool> DoesUserExistByIDAsync(int userID) {
        return await _userRepo.DoesUserExistByIDAsync(userID);
    }

    public async Task<bool> UpdateUserActivationAsync(int userId, bool isActive,int updatedBy) {
        if (!await _userRepo.DoesUserExistByIDAsync(userId))
            throw new CustomValidationException("UserID", [$"The user with id = {userId} was not fount"]);
        else if (userId == updatedBy)
            throw new CustomValidationException("UserID", [$"The user cannot update his activation"]);

        return await _userRepo.UpdateUserActivationAsync(userId, isActive);
    }

    public async Task<bool> IsUserActiveByIDAsync(int userID) {
        if (!await _userRepo.DoesUserExistByIDAsync(userID))
            throw new CustomValidationException("UserID", [$"The user with id = {userID} was not fount"]);

        return await _userRepo.IsUserActiveByIDAsync(userID);
    }

    
}

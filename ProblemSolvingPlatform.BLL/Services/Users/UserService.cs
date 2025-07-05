using Microsoft.AspNetCore.Http;
using ProblemSolvingPlatform.BLL.DTOs.UserProfile;
using ProblemSolvingPlatform.DAL.Repos.Users;
using System.Collections.Immutable;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;
using static System.Net.Mime.MediaTypeNames;


namespace ProblemSolvingPlatform.BLL.Services.Users;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private IUserRepo _userRepo { get; }
    public UserService(IUserRepo userRepo, IHttpContextAccessor httpContextAccessor)
    {
        _userRepo = userRepo;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserDTO?> GetUserByIdAsync(int userId)
    {
        var user = await _userRepo.GetUserByIdAsync(userId);

        if (user == null) return null;



        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request?.Host}";


        return new UserDTO()
        {
            UserId = user.UserId,
            Username = user.Username,
            CreatedAt = user.CreatedAt,
            Role = (Role) user.Role,
            ImagePath = user.ImagePath != null ? $"{baseUrl}/Images/{user.ImagePath}" : null
        };
    }

    public async Task<bool> UpdateUserInfoByIdAsync(int userId, UpdateUserDTO updateUser)
    {
        if(updateUser == null) return false;


        // 1 remove the image from the server 
        var user = await _userRepo.GetUserByIdAsync(userId);
        if(user == null) return false;


        if (updateUser.profileImage != null)
        {
            var oldPath = user.ImagePath ?? "";
            string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", oldPath);
            if (File.Exists(oldImagePath))
                File.Delete(oldImagePath);
        }
        // 2 
        string newpath = await FileService.SaveImageAndGetURL(updateUser.profileImage);
        return await _userRepo.UpdateUserInfoByIdAsync(userId, newpath);
    }

    public async Task<List<UserDTO>?> GetAllUsersWithFiltersAsync(int page, int limit, string? username)
    {
        var users = await _userRepo.GetAllUsersByFiltersAsync(page, limit, username);
        if (users == null)
            return null;

        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request?.Host}";


        var usersInfoLST = users.Select(x => new UserDTO()
        {
            UserId = x.UserId,
            Username = x.Username,
            CreatedAt = x.CreatedAt,
            ImagePath = x.ImagePath != null ? $"{baseUrl}/Images/{x.ImagePath}" : null,
            Role = (Role) x.Role
        }).ToList();
        return usersInfoLST;
    }

    public async Task<bool> DoesUserExistByUsernameAsync(string Username) {
        return await _userRepo.DoesUserExistByUsernameAsync(Username);
    }

    public async Task<bool> DoesUserExistByIDAsync(int UserID) {
        return await _userRepo.DoesUserExistByIDAsync(UserID);
    }
}

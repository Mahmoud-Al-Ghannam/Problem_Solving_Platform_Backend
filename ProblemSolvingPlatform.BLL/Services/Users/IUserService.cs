using Microsoft.AspNetCore.Http;
using ProblemSolvingPlatform.BLL.DTOs.UserProfile;
using ProblemSolvingPlatform.DAL.DTOs.Auth.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Users;

public interface IUserService
{
    public Task<UserDTO?> GetUserByIdAsync(int userId);
    public Task<List<UserDTO>?> GetAllUsersAsync(int page, int limit, string? username,bool? isActive);

    public Task<bool> DoesUserExistByUsernameAsync(string username);
    public Task<bool> DoesUserExistByIDAsync(int userID);
    public Task<bool> IsUserActiveByIDAsync(int userID);
    public Task<bool> UpdateUserInfoByIdAsync(int userId, UpdateUserDTO updateUser);

    public Task<bool> UpdateUserActivationAsync(int userId, bool isActive);
}

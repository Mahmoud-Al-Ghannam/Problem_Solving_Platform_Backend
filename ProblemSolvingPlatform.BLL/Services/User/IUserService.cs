using Microsoft.AspNetCore.Http;
using ProblemSolvingPlatform.BLL.DTOs.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.User;

public interface IUserService
{
    public Task<UserInfo?> GetUserByIdAsync(int userId);
    public Task<List<UserInfo>> GetAllUsersWithFiltersAsync(int page, int limit, string? username);

    public Task<bool> UpdateUserInfoByIdAsync(int userId, UpdateUserInfo updateUser);
}

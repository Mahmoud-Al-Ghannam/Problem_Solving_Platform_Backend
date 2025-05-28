using Microsoft.AspNetCore.Http;
using ProblemSolvingPlatform.BLL.DTOs.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Users;

public interface IUserService
{
    public Task<UserDTO?> GetUserByIdAsync(int userId);
    public Task<List<UserDTO>> GetAllUsersWithFiltersAsync(int page, int limit, string? username);

    public Task<bool> UpdateUserInfoByIdAsync(int userId, UpdateUserDTO updateUser);
}

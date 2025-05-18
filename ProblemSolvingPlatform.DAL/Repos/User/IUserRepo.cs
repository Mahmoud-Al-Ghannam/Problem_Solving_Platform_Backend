using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProblemSolvingPlatform.DAL.Repos.User;

public interface IUserRepo
{
    public Task<int?> AddUser(Models.User.User user);
    public Task<bool> UpdateUserInfoByIdAsync(int userId, string ImagePath);

    public Task<Models.User.User> GetUserByUsernameAndPassword(string Username, string Password);
    public Task<Models.User.User> GetUserByIdAsync(int userId);

    public Task<bool> DoesUserExistByUsername(string Username);

    public Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);

    public Task<List<Models.User.User>> GetAllUsersByFiltersAsync(int page, int limit, string username);
}

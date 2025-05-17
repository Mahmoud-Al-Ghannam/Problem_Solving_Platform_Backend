using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvingPlatform.DAL.Models;


namespace ProblemSolvingPlatform.DAL.Repos.User;

public interface IUserRepo
{
    public Task<int?> AddUser(ProblemSolvingPlatform.DAL.Models.User user);
    public Task<bool> UpdateUserInfoByIdAsync(int userId, string ImagePath);

    public Task<ProblemSolvingPlatform.DAL.Models.User> GetUserByUsernameAndPassword(string Username, string Password);
    public Task<ProblemSolvingPlatform.DAL.Models.User> GetUserByIdAsync(int userId);

    public Task<bool> DoesUserExistByUsername(string Username);

    public Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);

    public Task<List<ProblemSolvingPlatform.DAL.Models.User>> GetAllUsersByFiltersAsync(int page, int limit, string username);
}

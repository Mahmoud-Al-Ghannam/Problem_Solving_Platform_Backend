using ProblemSolvingPlatform.DAL.DTOs.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos.User;

public interface IUserRepo
{
    public Task<int?> AddUser(UserDTO user);
    public Task<UserDTO> GetUserByUsernameAndPassword(string Username, string Password);
    public Task<bool> DoesUserExistByUsername(string Username);
}

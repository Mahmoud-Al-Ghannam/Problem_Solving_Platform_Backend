using ProblemSolvingPlatform.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Interfaces;

public interface IUserRepo
{
    public Task<bool> AddUser(User user);
    public Task<User> GetUserByUsernameAndPassword(string Username, string Password);
    public Task<bool> DoesUserExistByUsername(string Username);
}

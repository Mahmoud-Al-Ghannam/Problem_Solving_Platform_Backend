using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Interfaces;
using ProblemSolvingPlatform.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos;


public class UserRepo : IUserRepo
{
    private readonly DbContext _db;
    public UserRepo(DbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<bool> AddUser(User user)
    {
        SqlConnection connection = _db.GetConnection();

        SqlCommand cmd = new("SP_User_AddNewUser", connection);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ImagePath", "C:/KOKO");
        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@Password", user.Password);

        // output 
        var userID = new SqlParameter("@UserID", System.Data.SqlDbType.Int) {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(userID);
        var IsAdded = new SqlParameter("@IsAdded", SqlDbType.Bit) {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(IsAdded);


        try
        {
            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return (bool)IsAdded.Value;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<bool> IsUsernameExist(string Username)
    {
        SqlConnection connection = _db.GetConnection();

        SqlCommand cmd = new("SP_User_DoesUserExistByUsername", connection);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Username", Username);

        try
        {
            await connection.OpenAsync();

            var res = await cmd.ExecuteScalarAsync();
            return (bool)res;
        }
        catch(Exception ex)
        {
            return false;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<User> GetUserByUsernameAndPassword(string Username, string Password)
    {
        SqlConnection connection = _db.GetConnection();

        SqlCommand cmd = new("SP_User_GetUserByUsernameAndPassword", connection);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Username", Username);
        cmd.Parameters.AddWithValue("@Password", Password);

        try
        {
            await connection.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();
            if(await reader.ReadAsync())
            {
                User user = new User()
                {
                    Username = (string)(reader["Username"] != DBNull.Value ? reader["Username"] : DBNull.Value),
                    UserId = (int)(reader["UserID"] != DBNull.Value ? reader["UserID"] : DBNull.Value),
                    ImagePath = "JOJO",
                    role = (byte)(reader["Role"] != DBNull.Value ? reader["Role"] : DBNull.Value),
                    CreatedAt = (DateTime)(reader["CreatedAt"] != DBNull.Value ? reader["CreatedAt"] : DBNull.Value)
                };
                return user;    
            }
            return null; 
        }
        catch(Exception ex)
        {
            return null;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

 
}

using ProblemSolvingPlatform.DAL.Context;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolvingPlatform.DAL.DTOs.UserProfile;

namespace ProblemSolvingPlatform.DAL.Repos.User;


public class UserRepo : IUserRepo
{
    private readonly DbContext _db;
    public UserRepo(DbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<int?> AddUser(UserDTO user)
    {
        using (SqlConnection connection = _db.GetConnection())
        {

            using (SqlCommand cmd = new("SP_User_AddNewUser", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ImagePath", "C:/KOKO");
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                // output 
                var userID = new SqlParameter("@UserID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(userID);
                var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(IsSuccess);


                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    if((bool)IsSuccess.Value)
                        return (int)userID.Value;
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }

    public async Task<bool> DoesUserExistByUsername(string Username)
    {
        using (SqlConnection connection = _db.GetConnection())
        {

            using (SqlCommand cmd = new("SP_User_DoesUserExistByUsername", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", Username);

                try
                {
                    await connection.OpenAsync();
                    var res = await cmd.ExecuteScalarAsync();
                    return (bool)res;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }

    public async Task<UserDTO> GetUserByUsernameAndPassword(string Username, string Password)
    {
        using (SqlConnection connection = _db.GetConnection())
        {

            using (SqlCommand cmd = new("SP_User_GetUserByUsernameAndPassword", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Password", Password);

                try
                {
                    await connection.OpenAsync();
                    var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        UserDTO user = new UserDTO()
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
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }


}

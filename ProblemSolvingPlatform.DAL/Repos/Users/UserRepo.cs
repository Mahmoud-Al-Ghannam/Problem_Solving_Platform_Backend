using ProblemSolvingPlatform.DAL.Context;
using System.Data;
using Microsoft.Data.SqlClient;
using ProblemSolvingPlatform.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using static ProblemSolvingPlatform.DAL.Models.Enums;
using ProblemSolvingPlatform.DAL.Models.Users;

namespace ProblemSolvingPlatform.DAL.Repos.Users;


public class UserRepo : IUserRepo {
    private readonly DbContext _db;
    public UserRepo(DbContext dbContext) {
        _db = dbContext;
    }

    public async Task<int?> AddUserAsync(Models.Users.UserModel user) {
        using (SqlConnection connection = _db.GetConnection()) {

            using (SqlCommand cmd = new("SP_User_AddNewUser", connection)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ImagePath", string.IsNullOrWhiteSpace(user.ImagePath) ? (object)DBNull.Value : user.ImagePath);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);

                // output 
                var userID = new SqlParameter("@UserID", SqlDbType.Int) {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(userID);
                var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(IsSuccess);


                try {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    if ((bool)IsSuccess.Value)
                        return (int)userID.Value;
                    return null;
                }
                catch (Exception ex) {
                    return null;
                }
            }
        }
    }

    public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword) {
        using (SqlConnection connection = _db.GetConnection()) {
            using (SqlCommand command = new("SP_User_UpdateUserPassword", connection)) {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OldPassword", oldPassword);
                command.Parameters.AddWithValue("@NewPassword", newPassword);
                command.Parameters.AddWithValue("@UserID", userId);

                // output 
                var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(IsSuccess);

                try {
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    return (bool)IsSuccess.Value;
                }
                catch (Exception ex) {
                    return false;
                }
                finally {
                    await connection.CloseAsync();
                }
            }
        }
    }

    public async Task<bool> DoesUserExistByUsernameAsync(string Username) {
        using (SqlConnection connection = _db.GetConnection()) {

            using (SqlCommand cmd = new("SP_User_DoesUserExistByUsername", connection)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", Username);

                try {
                    await connection.OpenAsync();
                    var res = await cmd.ExecuteScalarAsync();
                    return (bool)res;
                }
                catch (Exception ex) {
                    return false;
                }
            }
        }
    }

    public async Task<bool> DoesUserExistByIDAsync(int UserID) {
        using (SqlConnection connection = _db.GetConnection()) {

            using (SqlCommand cmd = new("SP_User_DoesUserExistByID", connection)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", UserID);

                try {
                    await connection.OpenAsync();
                    var res = await cmd.ExecuteScalarAsync();
                    return (bool)res;
                }
                catch (Exception ex) {
                    return false;
                }
            }
        }
    }

    public async Task<PageModel<Models.Users.UserModel>?> GetAllUsersAsync(int page, int limit, string? username = null, bool? isActive = null) {
        var pageModel = new PageModel<Models.Users.UserModel>();

        using (SqlConnection connection = _db.GetConnection()) {
            using (SqlCommand cmd = new("SP_User_GetAllUsers", connection)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Page", page);
                cmd.Parameters.AddWithValue("@Limit", limit);
                cmd.Parameters.AddWithValue("@Username", string.IsNullOrWhiteSpace(username) ? (object)DBNull.Value : username);
                cmd.Parameters.AddWithValue("@IsActive", isActive == null ? DBNull.Value : isActive.Value);

                try {
                    await connection.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync()) {
                        while (await reader.ReadAsync()) {
                            Models.Users.UserModel userInfo = new() {
                                UserId = Convert.ToInt32(reader["UserID"].ToString()),
                                Username = (string)reader["Username"],
                                ImagePath = (reader["ImagePath"] == DBNull.Value ? null : (string)reader["ImagePath"]),
                                Role = (DAL.Models.Enums.Role)(byte)reader["Role"],
                                CreatedAt = (DateTime)reader["CreatedAt"],
                                IsActive = (bool)reader["IsActive"]
                            };
                            pageModel.Items.Add(userInfo);
                        }
                    }

                    var temp = await GetTotalPagesAndItemsCountAsync(limit,username,isActive);
                    if (temp == null) return null;
                    pageModel.TotalItems = temp.Value.totalItems;
                    pageModel.TotalPages = temp.Value.totalPages;
                    pageModel.CurrentPage = page;

                    return pageModel;
                }
                catch (Exception ex) {
                    return null;
                }
            }
        }

    }
    
    public async Task<(int totalPages, int totalItems)?> GetTotalPagesAndItemsCountAsync(int limit, string? username = null, bool? isActive = null) {
        (int totalPages, int totalItems) result = (0, 0);

        using (SqlConnection connection = _db.GetConnection()) {
            using (SqlCommand cmd = new("SP_User_TotalPagesAndItemsCount", connection)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Limit", limit);
                cmd.Parameters.AddWithValue("@Username", string.IsNullOrWhiteSpace(username) ? (object)DBNull.Value : username);
                cmd.Parameters.AddWithValue("@IsActive", isActive == null ? DBNull.Value : isActive.Value);

                try {
                    await connection.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync()) {
                        if (await reader.ReadAsync()) {
                            result.totalItems = (int)reader["TotalItems"];
                            result.totalPages = (int)reader["TotalPages"];
                        }
                    }
                    return result;
                }
                catch (Exception ex) {
                    return null;
                }
            }
        }
    }
    

    public async Task<Models.Users.UserModel> GetUserByIdAsync(int userId) {
        using (SqlConnection connection = _db.GetConnection()) {
            using (SqlCommand command = new("SP_User_GetUserByID", connection)) {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", userId);

                try {
                    await connection.OpenAsync();
                    var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync()) {
                        Models.Users.UserModel userInfo = new() {
                            UserId = Convert.ToInt32(reader["UserID"].ToString()),
                            Username = (string)reader["Username"],
                            ImagePath = (reader["ImagePath"] == DBNull.Value ? null : (string)reader["ImagePath"]),
                            Role = (DAL.Models.Enums.Role)(byte)reader["Role"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            IsActive = (bool)reader["IsActive"]
                        };

                        return userInfo;
                    }
                    return null;
                }
                catch (Exception ex) {
                    return null;
                }
                finally {
                    await connection.CloseAsync();
                }
            }
        }
    }

    public async Task<UserModel> GetUserByUsernameAsync(string username) {
        using (SqlConnection connection = _db.GetConnection()) {
            using (SqlCommand command = new("SP_User_GetUserByUsername", connection)) {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", username);

                try {
                    await connection.OpenAsync();
                    var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync()) {
                        Models.Users.UserModel userInfo = new() {
                            UserId = Convert.ToInt32(reader["UserID"].ToString()),
                            Username = (string)reader["Username"],
                            ImagePath = (reader["ImagePath"] == DBNull.Value ? null : (string)reader["ImagePath"]),
                            Role = (DAL.Models.Enums.Role)(byte)reader["Role"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            IsActive = (bool)reader["IsActive"]
                        };

                        return userInfo;
                    }
                    return null;
                }
                catch (Exception ex) {
                    return null;
                }
                finally {
                    await connection.CloseAsync();
                }
            }
        }
    }

    public async Task<Models.Users.UserModel> GetUserByUsernameAndPasswordAsync(string Username, string Password) {
        using (SqlConnection connection = _db.GetConnection()) {

            using (SqlCommand cmd = new("SP_User_GetUserByUsernameAndPassword", connection)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", Username);
                cmd.Parameters.AddWithValue("@Password", Password);

                try {
                    await connection.OpenAsync();
                    var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync()) {
                        Models.Users.UserModel user = new Models.Users.UserModel() {
                            UserId = Convert.ToInt32(reader["UserID"].ToString()),
                            Username = (string)reader["Username"],
                            ImagePath = (reader["ImagePath"] == DBNull.Value ? null : (string)reader["ImagePath"]),
                            Role = (DAL.Models.Enums.Role)(byte)reader["Role"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            IsActive = (bool)reader["IsActive"]
                        };
                        return user;
                    }
                    return null;
                }
                catch (Exception ex) {
                    return null;
                }
            }
        }
    }

    public async Task<bool> UpdateUserInfoByIdAsync(int userId, string ImagePath) {

        using (SqlConnection connection = _db.GetConnection()) {

            using (SqlCommand cmd = new("SP_User_UpdateUser", connection)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ImagePath", ImagePath);

                // output 
                var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(IsSuccess);
                try {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return (bool)IsSuccess.Value;
                }
                catch (Exception ex) {
                    return false;
                }
            }
        }
    }

    public async Task<bool> UpdateUserActivationAsync(int userId, bool isActive) {
        using (SqlConnection connection = _db.GetConnection()) {

            using (SqlCommand cmd = new("SP_User_UpdateUserActivation", connection)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@IsActive", isActive);

                // output 
                var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(IsSuccess);
                try {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return (bool)IsSuccess.Value;
                }
                catch (Exception ex) {
                    return false;
                }
            }
        }
    }

    public async Task<bool> IsUserActiveByIDAsync(int UserID) {
        using (SqlConnection connection = _db.GetConnection()) {

            using (SqlCommand cmd = new("SP_User_IsUserActiveByID", connection)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", UserID);

                try {
                    await connection.OpenAsync();
                    var res = await cmd.ExecuteScalarAsync();
                    return (bool)res;
                }
                catch (Exception ex) {
                    return false;
                }
            }
        }
    }

    
}

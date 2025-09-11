using Microsoft.Data.SqlClient;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models;
using ProblemSolvingPlatform.DAL.Models.RequestApiLogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos.RequestApiLogs {
    public class RequestApiLogRepo : IRequestApiLogRepo {

        private readonly DbContext _db;
        public RequestApiLogRepo(DbContext dbContext) {
            _db = dbContext;
        }

        public async Task<int?> AddLogAsync(NewRequestApiLogModel newLog) {
            using (SqlConnection connection = _db.GetConnection()) {

                using (SqlCommand cmd = new("SP_RequestApiLog_AddNewLog", connection)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserID", newLog.UserID.HasValue ? newLog.UserID.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Endpoint", newLog.Endpoint);
                    cmd.Parameters.AddWithValue("@RequestType", newLog.RequestType);
                    cmd.Parameters.AddWithValue("@RequestBody", newLog.RequestBody == null ? newLog.RequestBody : DBNull.Value);
                    cmd.Parameters.AddWithValue("@RequestHeaders", newLog.RequestHeaders);
                    cmd.Parameters.AddWithValue("@ResponseHeaders", newLog.ResponseHeaders);
                    cmd.Parameters.AddWithValue("@ResponseBody", newLog.ResponseBody == null ? newLog.ResponseBody : DBNull.Value);
                    cmd.Parameters.AddWithValue("@StatusCode", newLog.StatusCode);
                    cmd.Parameters.AddWithValue("@ProcessingTimeMS", newLog.ProcessingTimeMS);

                    // output 
                    var logID = new SqlParameter("@RequestApiLogID", SqlDbType.Int) {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(logID);
                    var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(IsSuccess);


                    try {
                        await connection.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        if ((bool)IsSuccess.Value)
                            return (int)logID.Value;
                        return null;
                    }
                    catch (Exception ex) {
                        return null;
                    }
                }
            }
        }

        public async Task<PageModel<RequestApiLogModel>?> GetAllLogsAsync(int page, int limit, string? username = null, string? requestType = null, int? statusCode = null,string? endpoint = null) {
            var pageModel = new PageModel<RequestApiLogModel>();

            using (SqlConnection connection = _db.GetConnection()) {
                using (SqlCommand cmd = new("SP_RequestApiLog_GetAllLogs", connection)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Page", page);
                    cmd.Parameters.AddWithValue("@Limit", limit);
                    cmd.Parameters.AddWithValue("@Username", string.IsNullOrWhiteSpace(username) ? DBNull.Value : username);
                    cmd.Parameters.AddWithValue("@Endpoint", string.IsNullOrWhiteSpace(endpoint) ? DBNull.Value : endpoint);
                    cmd.Parameters.AddWithValue("@StatusCode", statusCode.HasValue ? statusCode.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@RequestType", string.IsNullOrWhiteSpace(requestType) ? DBNull.Value : requestType);

                    try {
                        await connection.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync()) {
                            while (await reader.ReadAsync()) {
                                RequestApiLogModel logInfo = new() {
                                    RequestApiLogID = Convert.ToInt32(reader["RequestApiLogID"].ToString()),
                                    UserID = (reader["UserID"] == DBNull.Value ? null : (int)reader["UserID"]),
                                    Username = (reader["Username"] == DBNull.Value ? null : (string)reader["Username"]),
                                    Endpoint = (string)reader["Endpoint"],
                                    RequestType = (string)reader["RequestType"],
                                    RequestHeaders = (string)reader["RequestHeaders"],
                                    RequestBody = (reader["RequestBody"] == DBNull.Value ? null : (string)reader["RequestBody"]),
                                    ResponseHeaders = (string)reader["ResponseHeaders"],
                                    ResponseBody = (reader["ResponseBody"] == DBNull.Value ? null : (string)reader["ResponseBody"]),
                                    StatusCode = (int)reader["StatusCode"],
                                    ProcessingTimeMS = (int)reader["ProcessingTimeMS"],
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                };
                                pageModel.Items.Add(logInfo);
                            }
                        }

                        var temp = await GetTotalPagesAndItemsCountAsync(limit,username,requestType,statusCode);
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

        public async Task<(int totalPages, int totalItems)?> GetTotalPagesAndItemsCountAsync(int limit, string? username = null, string? requestType = null, int? statusCode = null,string? endpoint= null) {
            (int totalPages, int totalItems) result = (0, 0);

            using (SqlConnection connection = _db.GetConnection()) {
                using (SqlCommand cmd = new("SP_RequestApiLog_TotalPagesAndItemsCount", connection)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Limit", limit);
                    cmd.Parameters.AddWithValue("@Username", string.IsNullOrWhiteSpace(username) ? DBNull.Value : username);
                    cmd.Parameters.AddWithValue("@Endpoint", string.IsNullOrWhiteSpace(endpoint) ? DBNull.Value : endpoint);
                    cmd.Parameters.AddWithValue("@StatusCode", statusCode.HasValue ? statusCode.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@RequestType", string.IsNullOrWhiteSpace(requestType) ? DBNull.Value : requestType);

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
    }
}

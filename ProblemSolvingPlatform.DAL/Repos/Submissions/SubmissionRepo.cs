using Microsoft.Data.SqlClient;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models.Submissions;
using ProblemSolvingPlatform.DAL.Models.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos.Submissions;

public class SubmissionRepo : ISubmissionRepo
{
    private DbContext _db { get; }
    public SubmissionRepo(DbContext dbContext)
    {
        _db = dbContext;
    }


    public async Task<int?> AddNewSubmission(Models.Submissions.Submission submission)
    {
        using (SqlConnection connection = _db.GetConnection())
        {
            using (SqlCommand cmd = new("SP_Submission_AddNewSubmission", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@UserID", submission.UserID);
                cmd.Parameters.AddWithValue("@CompilerName", submission.CompilerName);
                cmd.Parameters.AddWithValue("@Code", submission.Code);
                cmd.Parameters.AddWithValue("@VisionScope", submission.VisionScope);
                cmd.Parameters.AddWithValue("@ProblemID", submission.ProblemID);

                // output 
                var submissionID = new SqlParameter("@SubmissionID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(submissionID);

               
                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    if (submissionID.Value == DBNull.Value) return null;
                    return (int?)submissionID?.Value;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }

    public async Task<bool> ChangeVisionScope(int submissionId, int visionScopeId, int userId)
    {
        using (var connection = _db.GetConnection())
        using (var command = new SqlCommand("SP_Submission_ChangeVisionScope", connection))
        {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@submissionId", submissionId);
            command.Parameters.AddWithValue("@visionScopeId", visionScopeId);
            command.Parameters.AddWithValue("@userId", userId);

            try
            {
                await connection.OpenAsync();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public async Task<bool> UpdateSubmissionStatusAndExecTime(int submissionId, byte status, int execTimeMS)
    {
        using (var connection = _db.GetConnection())
        using (var command = new SqlCommand("SP_Submission_UpdateSubmissionStatusAndExecutionTime", connection))
        {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@SubmissionID", submissionId);
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("ExecTimeMS", execTimeMS);
            try
            {
                await connection.OpenAsync();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public async Task<string?> GetSubmissionCode(int submissionId)
    {
        using (SqlConnection connection = _db.GetConnection())
        using (SqlCommand command = new("SP_Submission_GetCode", connection))
        {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@SubmissionId", submissionId);
            try
            {
                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                if (result != null && result != DBNull.Value) 
                    return result.ToString();
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }

    public async Task<(int userId, byte visionScope)?> GetSubmissionAccessInfo(int submissionId)
    {
        using SqlConnection connection = _db.GetConnection();
        using SqlCommand command = new SqlCommand("SP_Submission_GetAccessInfo", connection) {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@SubmissionId", submissionId);

        try
        {
            await connection.OpenAsync();
            using SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                int userId = reader["UserID"] != DBNull.Value ? Convert.ToInt32(reader["UserID"]) : 0;
                byte visionScope = reader["VisionScope"] != DBNull.Value ? Convert.ToByte(reader["VisionScope"]) : (byte)0;
                return (userId, visionScope);
            }
            return null; 
        }
        catch
        {
            return null;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<List<Submission>?> GetSubmissions(int userId, int page, int limit, int? problemId, byte visionScope)
    {
        var results = new List<Submission>();

        using (var conn = _db.GetConnection())
        using (var cmd = new SqlCommand("SP_Submissions_GetSubmissions", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = userId });
            cmd.Parameters.Add(new SqlParameter("@Scope", SqlDbType.TinyInt) { Value = visionScope });
            cmd.Parameters.Add(new SqlParameter("@ProblemId", SqlDbType.Int) { Value = (problemId == null ? DBNull.Value : problemId.Value) });
            cmd.Parameters.Add(new SqlParameter("@Page", SqlDbType.Int) { Value = page });
            cmd.Parameters.Add(new SqlParameter("@Limit", SqlDbType.Int) { Value = limit });

            try
            {
                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var sub = new Submission
                        {
                            SubmissionId = reader.GetInt32(reader.GetOrdinal("SubmissionID")),
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            ProblemID = reader.GetInt32(reader.GetOrdinal("ProblemID")),
                            CompilerName = reader.GetString(reader.GetOrdinal("CompilerName")),
                            Status = reader.GetByte(reader.GetOrdinal("Status")),
                            ExecutionTimeMilliseconds = reader.GetInt32(reader.GetOrdinal("ExecutionTimeMilliseconds")),
                            VisionScope = reader.GetByte(reader.GetOrdinal("VisionScope")),
                            SubmittedAt = reader.GetDateTime(reader.GetOrdinal("SubmittedAt"))
                        };
                        results.Add(sub);
                    }
                }
            }
            catch 
            {
                return null;
            }
        }

        return results;
    
    }

}

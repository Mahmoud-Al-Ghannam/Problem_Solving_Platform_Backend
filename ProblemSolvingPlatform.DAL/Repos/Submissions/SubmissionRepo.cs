using Microsoft.Data.SqlClient;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models;
using ProblemSolvingPlatform.DAL.Models.Problems;
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

public class SubmissionRepo : ISubmissionRepo {
    private DbContext _db { get; }
    private readonly ISubmissionTestRepo _submissionTestRepo;
    public SubmissionRepo(DbContext dbContext, ISubmissionTestRepo submissionTestRepo) {
        _db = dbContext;
        _submissionTestRepo = submissionTestRepo;
    }


    public async Task<int?> AddNewSubmission(NewSubmissionModel submission) {
        SqlTransaction transaction = null;
        SqlConnection connection = _db.GetConnection();
        bool ok = true;
        int? SubmissionID = null;
        Enums.SubmissionStatus submissionStatus = submission.SubmissionTestCases.Where(stc => stc.Status != Enums.SubmissionStatus.Accepted)
                                                                                .Select(stc => stc.Status)
                                                                                .FirstOrDefault(Enums.SubmissionStatus.Accepted);

        int submissionExecutionTimeMS = submission.SubmissionTestCases.Max(stc => stc.ExecutionTimeMilliseconds);
        try {
            await connection.OpenAsync();
            transaction = connection.BeginTransaction();


            using (SqlCommand cmd = new("SP_Submission_AddNewSubmission", connection, transaction)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", submission.UserID);
                cmd.Parameters.AddWithValue("@ProblemID", submission.ProblemID);
                cmd.Parameters.AddWithValue("@CompilerName", submission.CompilerName);
                cmd.Parameters.AddWithValue("@Status", (byte)submissionStatus);
                cmd.Parameters.AddWithValue("@ExecutionTimeMilliseconds", submissionExecutionTimeMS);
                cmd.Parameters.AddWithValue("@Code", submission.Code);
                cmd.Parameters.AddWithValue("@VisionScope", submission.VisionScope);

                // output 
                var ParmSubmissionID = new SqlParameter("@SubmissionID", SqlDbType.Int) {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(ParmSubmissionID);

                var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(IsSuccess);

                await cmd.ExecuteNonQueryAsync();

                if ((bool)IsSuccess.Value)
                    SubmissionID = (int)ParmSubmissionID.Value;
                else {
                    ok = false;
                }


            }

            if (ok) {
                foreach (var submissionTestCase in submission.SubmissionTestCases) {
                    submissionTestCase.SubmissionID = SubmissionID.Value;
                    int? submissionTestCaseID = await _submissionTestRepo.AddNewSubmissionTestCaseAsync(submissionTestCase,connection,transaction);
                    if (submissionTestCaseID == null) {
                        ok = false;
                        break;
                    }
                }
            }
        }
        catch (Exception ex) {
            ok = false;
            SubmissionID = null;
        }

        if (ok) transaction.Commit();
        else transaction.Rollback();

        await connection.CloseAsync();
        return SubmissionID;
    }

    public async Task<bool> ChangeVisionScope(int submissionId, int visionScopeId, int userId) {
        using (var connection = _db.GetConnection())
        using (var command = new SqlCommand("SP_Submission_ChangeVisionScope", connection)) {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@submissionId", submissionId);
            command.Parameters.AddWithValue("@visionScopeId", visionScopeId);
            command.Parameters.AddWithValue("@userId", userId);

            try {
                await connection.OpenAsync();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                    return true;
                return false;
            }
            catch (Exception ex) {
                return false;
            }
        }
    }

    public async Task<List<SubmissionModel>?> GetAllSubmissions(int page, int limit, int? userId = null, int? problemId = null, byte? visionScope = null) {
        var results = new List<SubmissionModel>();

        using (var conn = _db.GetConnection())
        using (var cmd = new SqlCommand("SP_Submissions_GetAllSubmissions", conn)) {
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@Page", page);
            cmd.Parameters.AddWithValue("@Limit", limit);

            if (userId == null) cmd.Parameters.AddWithValue("@UserId", DBNull.Value);
            else cmd.Parameters.AddWithValue("@UserId", userId.Value);

            if (problemId == null) cmd.Parameters.AddWithValue("@ProblemId", DBNull.Value);
            else cmd.Parameters.AddWithValue("@ProblemId", problemId.Value);

            if (visionScope == null) cmd.Parameters.AddWithValue("@Scope", DBNull.Value);
            else cmd.Parameters.AddWithValue("@Scope", visionScope.Value);

            try {
                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        var sub = new SubmissionModel {
                            SubmissionID = reader.GetInt32(reader.GetOrdinal("SubmissionID")),
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            ProblemID = reader.GetInt32(reader.GetOrdinal("ProblemID")),
                            CompilerName = reader.GetString(reader.GetOrdinal("CompilerName")),
                            Status = (Enums.SubmissionStatus)reader.GetByte(reader.GetOrdinal("Status")),
                            ExecutionTimeMilliseconds = reader.GetInt32(reader.GetOrdinal("ExecutionTimeMilliseconds")),
                            VisionScope = (Enums.VisionScope) reader.GetByte(reader.GetOrdinal("VisionScope")),
                            SubmittedAt = reader.GetDateTime(reader.GetOrdinal("SubmittedAt"))
                        };
                        results.Add(sub);
                    }
                }
            }
            catch {
                return null;
            }
        }

        return results;

    }

    public async Task<SubmissionModel?> GetSubmissionByID (int submissionID) {
        SubmissionModel submissionModel = new SubmissionModel();

        try {
            using (SqlConnection connection = _db.GetConnection()) {

                await connection.OpenAsync();
                using (SqlCommand cmd = new("SP_Submission_GetSubmissionByID", connection)) {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubmissionID", submissionID);
                    using (var reader = await cmd.ExecuteReaderAsync()) {
                        if (await reader.ReadAsync()) {
                            submissionModel.SubmissionID = Convert.ToInt32(reader["SubmissionID"].ToString());
                            submissionModel.UserID = Convert.ToInt32(reader["UserID"].ToString());
                            submissionModel.ProblemID = Convert.ToInt32(reader["ProblemID"].ToString());
                            submissionModel.CompilerName = (string)reader["CompilerName"];
                            submissionModel.Status = (Enums.SubmissionStatus)(byte)reader["Status"];
                            submissionModel.ExecutionTimeMilliseconds = Convert.ToInt32(reader["ExecutionTimeMilliseconds"].ToString());
                            submissionModel.Code = (string)reader["Code"];
                            submissionModel.VisionScope = (Enums.VisionScope)(byte)reader["VisionScope"];
                            submissionModel.SubmittedAt = (DateTime)reader["SubmittedAt"];
                        }
                        else return null;
                    }
                }
            }
        }
        catch (Exception ex) {
            return null;
        }

        return submissionModel;
    }
    
    public async Task<bool> DoesSubmissionExistByID(int submissionID) {
        try {
            using (SqlConnection connection = _db.GetConnection()) {

                await connection.OpenAsync();

                using (SqlCommand cmd = new("SP_Submission_DoesSubmissionExistByID", connection)) {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SubmissionID", submissionID);


                    var res = await cmd.ExecuteScalarAsync();
                    if (res == null || Convert.ToInt32(res.ToString()) == 0) return false;
                    return true;
                }

            }
        }
        catch (Exception ex) {
            return false;
        }
    }
}

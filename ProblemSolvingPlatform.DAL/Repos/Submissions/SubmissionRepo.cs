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


    public async Task<int?> AddGeneralProblemSubmission(int problemId, Models.Submissions.Submission submission)
    {
        using (SqlConnection connection = _db.GetConnection())
        {
            using (SqlCommand cmd = new("SP_Submission_AddGeneralProblemSubmission", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@UserID", submission.UserID);
                cmd.Parameters.AddWithValue("@ProgrammingLanguage", submission.ProgrammingLanguage);
                cmd.Parameters.AddWithValue("@Code", submission.Code);
                cmd.Parameters.AddWithValue("@VisionScope", submission.VisionScope);
                cmd.Parameters.AddWithValue("@ProblemID", problemId);

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


}

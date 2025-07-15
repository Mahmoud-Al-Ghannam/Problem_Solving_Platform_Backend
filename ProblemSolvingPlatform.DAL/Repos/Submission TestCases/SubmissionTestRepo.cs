using Microsoft.Data.SqlClient;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models;
using ProblemSolvingPlatform.DAL.Models.Submissions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos;

public class SubmissionTestRepo : ISubmissionTestRepo
{

    private DbContext _db { get; }
    public SubmissionTestRepo(DbContext dbContext)
    {
        _db = dbContext;
    }


    public async Task<int?> AddNewSubmissionTestCase(SubmissionTestCase submissionTestCase)
    {
        //SP_SubmissionTestCase_AddNewSubmissionTestCase
        using (SqlConnection connection = _db.GetConnection())
        {
            using (SqlCommand cmd = new("SP_SubmissionTestCase_AddNewSubmissionTestCase", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TestCaseID", submissionTestCase.TestCaseID);
                cmd.Parameters.AddWithValue("@SubmissionID", submissionTestCase.SubmissionID);
                cmd.Parameters.AddWithValue("@Status", submissionTestCase.Status);
                cmd.Parameters.AddWithValue("@ExecutionTimeMilliseconds", submissionTestCase.ExecutionTimeMilliseconds);

                // output 
                var submissionTestCaseID = new SqlParameter("@SubmissionTestCaseID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(submissionTestCaseID);


                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    if (submissionTestCaseID.Value == DBNull.Value) return null;
                    return (int?)submissionTestCaseID?.Value;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }

    public async Task<List<SubmissionTestCase>?> GetAllSubmissionTestCases(int? submissionId = null)
    {
        List<SubmissionTestCase> testCases = new();
        using (SqlConnection connection = _db.GetConnection())
        using (SqlCommand command = new("SP_Submission_GetAllSubmissionTestCases", connection))
        {
            command.CommandType = CommandType.StoredProcedure;

            if (submissionId == null) command.Parameters.AddWithValue("@SubmissionId", DBNull.Value);
            else command.Parameters.AddWithValue("@SubmissionId", submissionId.Value);

            try
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        SubmissionTestCase testCase = new()
                        {
                            SubmissionTestCaseID = reader["SubmissionTestCaseID"] != DBNull.Value ? Convert.ToInt32(reader["SubmissionTestCaseID"]) : 0,
                            TestCaseID = reader["TestCaseID"] != DBNull.Value ? Convert.ToInt32(reader["TestCaseID"]) : 0,
                            SubmissionID = reader["SubmissionID"] != DBNull.Value ? Convert.ToInt32(reader["SubmissionID"]) : 0,
                            Status = reader["Status"] != DBNull.Value ? Convert.ToByte(reader["Status"]) : (byte)0,
                            ExecutionTimeMilliseconds = reader["ExecutionTimeMilliseconds"] != DBNull.Value ? Convert.ToInt32(reader["ExecutionTimeMilliseconds"]) : 0
                        };

                        testCases.Add(testCase);
                    }
                }

                return testCases;
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

}

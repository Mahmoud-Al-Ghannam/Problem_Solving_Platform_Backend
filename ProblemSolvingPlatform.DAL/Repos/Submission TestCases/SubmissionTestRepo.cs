using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models;
using ProblemSolvingPlatform.DAL.Models.Submissions;
using ProblemSolvingPlatform.DAL.Models.SubmissionTestCase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos;

public class SubmissionTestRepo : ISubmissionTestRepo {

    private DbContext _db { get; }
    public SubmissionTestRepo(DbContext dbContext) {
        _db = dbContext;
    }


    public async Task<int?> AddNewSubmissionTestCaseAsync(NewSubmissionTestCaseModel submissionTestCase, SqlConnection connection, SqlTransaction transaction) {
        //SP_SubmissionTestCase_AddNewSubmissionTestCase
        try {
            using (SqlCommand cmd = new("SP_SubmissionTestCase_AddNewSubmissionTestCase", connection, transaction)) {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TestCaseID", submissionTestCase.TestCaseID);
                cmd.Parameters.AddWithValue("@SubmissionID", submissionTestCase.SubmissionID);
                cmd.Parameters.AddWithValue("@Status", submissionTestCase.Status);
                cmd.Parameters.AddWithValue("@ExecutionTimeMilliseconds", submissionTestCase.ExecutionTimeMilliseconds);
                cmd.Parameters.AddWithValue("@Output", submissionTestCase.Output);

                // output 
                var ParmSubmissionTestCaseID = new SqlParameter("@SubmissionTestCaseID", SqlDbType.Int) {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(ParmSubmissionTestCaseID);

                var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(IsSuccess);

                await cmd.ExecuteNonQueryAsync();

                if ((bool)IsSuccess.Value)
                    return (int)ParmSubmissionTestCaseID.Value;


            }
        }
        catch (Exception ex) {
            return null;
        }

        return null;
    }

    public async Task<List<SubmissionTestCaseModel>?> GetAllSubmissionTestCasesAsync(int? submissionId = null) {
        List<SubmissionTestCaseModel> testCases = new();
        using (SqlConnection connection = _db.GetConnection())
        using (SqlCommand command = new("SP_Submission_GetAllSubmissionTestCases", connection)) {
            command.CommandType = CommandType.StoredProcedure;

            if (submissionId == null) command.Parameters.AddWithValue("@SubmissionId", DBNull.Value);
            else command.Parameters.AddWithValue("@SubmissionId", submissionId.Value);

            try {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        SubmissionTestCaseModel testCase = new() {
                            SubmissionTestCaseID = (int) reader["SubmissionTestCaseID"],
                            TestCaseID = (int) reader["TestCaseID"],
                            SubmissionID = (int) reader["SubmissionID"],
                            Status = (byte) reader["Status"],
                            ExecutionTimeMilliseconds = (int) reader["ExecutionTimeMilliseconds"],
                            Output = (string) reader["Output"]
                        };

                        testCases.Add(testCase);
                    }
                }

                return testCases;
            }
            catch (Exception ex) {
                return null;
            }
            finally {
                await connection.CloseAsync();
            }
        }
    }

    public async Task<List<DetailedSubmissionTestCaseModel>?> GetAllDetailedSubmissionTestCasesAsync(int? submissionId = null) {
        List<DetailedSubmissionTestCaseModel> testCases = new();
        using (SqlConnection connection = _db.GetConnection())
        using (SqlCommand command = new("SP_Submission_GetAllDetailedSubmissionTestCases", connection)) {
            command.CommandType = CommandType.StoredProcedure;

            if (submissionId == null) command.Parameters.AddWithValue("@SubmissionId", DBNull.Value);
            else command.Parameters.AddWithValue("@SubmissionId", submissionId.Value);

            try {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        DetailedSubmissionTestCaseModel testCase = new() {
                            SubmissionTestCaseID = (int)reader["SubmissionTestCaseID"],
                            TestCaseID = (int)reader["TestCaseID"],
                            SubmissionID = (int)reader["SubmissionID"],
                            Status = (byte)reader["Status"],
                            ExecutionTimeMilliseconds = (int)reader["ExecutionTimeMilliseconds"],
                            Output = (string)reader["Output"],
                            Input = (string)reader["Input"],
                            ExpectedOutput = (string)reader["ExpectedOutput"]
                        };

                        testCases.Add(testCase);
                    }
                }

                return testCases;
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

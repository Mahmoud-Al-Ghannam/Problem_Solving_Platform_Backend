using Microsoft.Data.SqlClient;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models.Problems;
using ProblemSolvingPlatform.DAL.Models.TestCases;
using ProblemSolvingPlatform.DAL.Models.Users;
using System.Data;

namespace ProblemSolvingPlatform.DAL.Repos.Problems {
    public class ProblemRepo : IProblemRepo {

        private readonly DbContext _db;
        public ProblemRepo(DbContext dbContext) {
            _db = dbContext;
        }

        public async Task<int?> AddProblemAsync(NewProblemModel newProblem) {
            SqlTransaction transaction = null;
            SqlConnection connection = _db.GetConnection();
            bool ok = true;
            int? ProblemID = null;
            try {
                await connection.OpenAsync();
                transaction = connection.BeginTransaction();

                // Exec SP_Problem_AddNewProblem
                using (SqlCommand cmd = new("SP_Problem_AddNewProblem", connection, transaction)) {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CompilerName", newProblem.CompilerName);
                    cmd.Parameters.AddWithValue("@CreatedBy", newProblem.CreatedBy);
                    cmd.Parameters.AddWithValue("@Title", newProblem.Title);
                    cmd.Parameters.AddWithValue("@GeneralDescription", newProblem.GeneralDescription);
                    cmd.Parameters.AddWithValue("@InputDescription", newProblem.InputDescription);
                    cmd.Parameters.AddWithValue("@OutputDescription", newProblem.OutputDescription);
                    cmd.Parameters.AddWithValue("@Note", newProblem.Note);
                    cmd.Parameters.AddWithValue("@Tutorial", newProblem.Tutorial);
                    cmd.Parameters.AddWithValue("@Difficulty", (byte)newProblem.Difficulty);
                    cmd.Parameters.AddWithValue("@SolutionCode", newProblem.SolutionCode);
                    cmd.Parameters.AddWithValue("@TimeLimitMilliseconds", newProblem.TimeLimitMilliseconds);

                    // output 
                    var ParmProblemID = new SqlParameter("@ProblemID", SqlDbType.Int) {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(ParmProblemID);

                    var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(IsSuccess);

                    await cmd.ExecuteNonQueryAsync();

                    if ((bool)IsSuccess.Value)
                        ProblemID = (int)ParmProblemID.Value;
                    else {
                        ok = false;
                    }
                }

                if (ok) {
                    foreach (NewTestCaseModel newTestCase in newProblem.TestCases) {
                        int? TestCaseID = null;
                        // Exec SP_TestCase_AddNewTestCase
                        using (SqlCommand cmd = new("SP_TestCase_AddNewTestCase", connection, transaction)) {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@ProblemID", ProblemID);
                            cmd.Parameters.AddWithValue("@Input", newTestCase.Input);
                            cmd.Parameters.AddWithValue("@Output", newTestCase.Output);
                            cmd.Parameters.AddWithValue("@IsPublic", newTestCase.IsPublic);
                            cmd.Parameters.AddWithValue("@IsSample", newTestCase.IsSample);

                            // output 
                            var ParmTestCaseID = new SqlParameter("@TestCaseID", SqlDbType.Int) {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(ParmTestCaseID);

                            var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(IsSuccess);

                            await cmd.ExecuteNonQueryAsync();

                            if ((bool)IsSuccess.Value)
                                TestCaseID = (int)ParmTestCaseID.Value;
                            else {
                                ok = false;
                            }
                        }
                    }
                }

                if (ok) {
                    foreach (int tagId in newProblem.TagIDs) {
                        int? ProblemTagID = null;
                        // Exec SP_Problem_AddNewTagToProblem
                        using (SqlCommand cmd = new("SP_Problem_AddNewTagToProblem", connection, transaction)) {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@ProblemID", ProblemID);
                            cmd.Parameters.AddWithValue("@TagID", tagId);

                            // output 
                            var ParmProblemTagID = new SqlParameter("@ProblemTagID", SqlDbType.Int) {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(ParmProblemTagID);

                            var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(IsSuccess);

                            await cmd.ExecuteNonQueryAsync();

                            if ((bool)IsSuccess.Value)
                                ProblemTagID = (int)ParmProblemTagID.Value;
                            else {
                                ok = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                ok = false;
                ProblemID = null;
            }

            if (ok) transaction.Commit();
            else transaction.Rollback();

            await connection.CloseAsync();
            return ProblemID;
        }


        public async Task<bool> ProblemExistsAsync(int problemId)
        {
            // run the code 
            using (SqlConnection connection = _db.GetConnection())
            {

                using (SqlCommand cmd = new("SP_Problem_DoesProblemExistByID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProblemID", problemId);

                    try
                    {
                        await connection.OpenAsync();
                        var res = await cmd.ExecuteScalarAsync();
                        if(res == null || Convert.ToInt32(res) == 0) return false;
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
        }
    }
}

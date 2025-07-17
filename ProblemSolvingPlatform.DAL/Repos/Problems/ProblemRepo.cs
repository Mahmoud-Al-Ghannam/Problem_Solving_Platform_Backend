using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Data.SqlClient;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models;
using ProblemSolvingPlatform.DAL.Models.Problems;
using ProblemSolvingPlatform.DAL.Models.Tags;
using ProblemSolvingPlatform.DAL.Models.TestCases;
using ProblemSolvingPlatform.DAL.Models.Users;
using ProblemSolvingPlatform.DAL.Repos.Tags;
using ProblemSolvingPlatform.DAL.Repos.Tests;
using System.Data;
using static ProblemSolvingPlatform.DAL.Models.Enums;

namespace ProblemSolvingPlatform.DAL.Repos.Problems {
    public class ProblemRepo : IProblemRepo {

        private readonly DbContext _db;
        private readonly ITestCaseRepo _testCaseRepo;
        public ProblemRepo(DbContext dbContext, ITestCaseRepo testCaseRepo) {
            _db = dbContext;
            _testCaseRepo = testCaseRepo;
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
                    cmd.Parameters.AddWithValue("@Note", newProblem.Note == null ? DBNull.Value : newProblem.Note);
                    cmd.Parameters.AddWithValue("@Tutorial", newProblem.Tutorial == null ? DBNull.Value : newProblem.Tutorial);
                    cmd.Parameters.AddWithValue("@Difficulty", (byte)newProblem.Difficulty);
                    cmd.Parameters.AddWithValue("@SolutionCode", newProblem.SolutionCode);
                    cmd.Parameters.AddWithValue("@TimeLimitMilliseconds", newProblem.TimeLimitMilliseconds);
                    cmd.Parameters.AddWithValue("@IsSystemProblem", newProblem.IsSystemProblem);

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
                    newProblem.TestCases = newProblem.TestCases.Select(tc => { tc.ProblemID = ProblemID.Value; return tc; }).ToList();
                    foreach (NewTestCaseModel newTestCase in newProblem.TestCases) {
                        int? TestCaseID = null;
                        // Exec SP_TestCase_AddNewTestCase
                        using (SqlCommand cmd = new("SP_TestCase_AddNewTestCase", connection, transaction)) {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@ProblemID", newTestCase.ProblemID);
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

        public async Task<ProblemModel?> GetProblemByIDAsync(int problemID) {
            ProblemModel problemModel = new ProblemModel();

            try {
                using (SqlConnection connection = _db.GetConnection()) {

                    await connection.OpenAsync();
                    using (SqlCommand cmd = new("SP_Problem_GetProblemByID", connection)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProblemID", problemID);
                        using (var reader = await cmd.ExecuteReaderAsync()) {
                            if (await reader.ReadAsync()) {
                                problemModel.ProblemID = Convert.ToInt32(reader["ProblemID"].ToString());
                                problemModel.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                                problemModel.CreatedAt = (DateTime)reader["CreatedAt"];
                                problemModel.DeletedBy = reader["DeletedBy"] == DBNull.Value ? null : (int)reader["DeletedBy"];
                                problemModel.DeletedAt = reader["DeletedAt"] == DBNull.Value ? null : (DateTime)reader["DeletedAt"];
                                problemModel.Title = (string)reader["Title"];
                                problemModel.GeneralDescription = (string)reader["GeneralDescription"];
                                problemModel.InputDescription = (string)reader["InputDescription"];
                                problemModel.OutputDescription = (string)reader["OutputDescription"];
                                problemModel.Note = reader["Note"] == DBNull.Value ? null : (string)reader["Note"];
                                problemModel.Tutorial = reader["Tutorial"] == DBNull.Value ? null : (string)reader["Tutorial"];
                                problemModel.Difficulty = (Enums.Difficulty)(byte)reader["Difficulty"];
                                problemModel.SolutionCode = (string)reader["SolutionCode"];
                                problemModel.CompilerName = (string)reader["CompilerName"];
                                problemModel.TimeLimitMilliseconds = (int)reader["TimeLimitMilliseconds"];
                                problemModel.IsSystemProblem = (bool)reader["IsSystemProblem"];
                            }
                            else return null;
                        }
                    }

                    problemModel.SampleTestCases = await _testCaseRepo.GetAllTestCasesAsync(1, 100, problemID, IsSample: true) ?? [];
                    problemModel.Tags = await GetProblemTagsAsync(problemID) ?? [];
                }
            }
            catch (Exception ex) {
                return null;
            }

            return problemModel;
        }

        public async Task<bool> DoesProblemExistByIDAsync(int problemId) {
            // run the code 
            try {
                using (SqlConnection connection = _db.GetConnection()) {

                    await connection.OpenAsync();

                    using (SqlCommand cmd = new("SP_Problem_DoesProblemExistByID", connection)) {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProblemID", problemId);


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

        public async Task<IEnumerable<TagModel>?> GetProblemTagsAsync(int problemID) {
            List<TagModel> tags = new List<TagModel>();

            try {
                using (SqlConnection connection = _db.GetConnection()) {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new("SP_ProblemTag_GetAllProblemTags", connection)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProblemID", problemID);

                        using (var reader = await cmd.ExecuteReaderAsync()) {
                            while (await reader.ReadAsync()) {
                                TagModel tag = new TagModel() {
                                    TagID = Convert.ToInt32(reader["TagID"].ToString()),
                                    Name = reader["Name"].ToString() ?? ""
                                };
                                tags.Add(tag);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                return null;
            }
            return tags;
        }

        public async Task<bool> UpdateProblemAsync(UpdateProblemModel updateProblem) {

            try {
                using (SqlConnection connection = _db.GetConnection()) {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new("SP_Problem_UpdateProblem", connection)) {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProblemID", updateProblem.ProblemID);
                        cmd.Parameters.AddWithValue("@Title", updateProblem.Title);
                        cmd.Parameters.AddWithValue("@GeneralDescription", updateProblem.GeneralDescription);
                        cmd.Parameters.AddWithValue("@InputDescription", updateProblem.InputDescription);
                        cmd.Parameters.AddWithValue("@OutputDescription", updateProblem.OutputDescription);
                        cmd.Parameters.AddWithValue("@Note", updateProblem.Note);
                        cmd.Parameters.AddWithValue("@Tutorial", updateProblem.Tutorial);
                        cmd.Parameters.AddWithValue("@Difficulty", (byte)updateProblem.Difficulty);

                        // output 
                        var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(IsSuccess);
                        await cmd.ExecuteNonQueryAsync();
                        return (bool)IsSuccess.Value;
                    }
                }
            }
            catch (Exception ex) {
                return false;
            }
        }

        public async Task<bool> DeleteProblemByIDAsync(int problemID, int deletedBy) {
            try {
                using (SqlConnection connection = _db.GetConnection()) {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new("SP_Problem_DeleteProblem", connection)) {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ProblemID", problemID);
                        cmd.Parameters.AddWithValue("@DeletedBy", deletedBy);

                        // output 
                        var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(IsSuccess);
                        await cmd.ExecuteNonQueryAsync();
                        return (bool)IsSuccess.Value;
                    }
                }
            }
            catch (Exception ex) {
                return false;
            }
        }

        public async Task<IEnumerable<ShortProblemModel>?> GetAllProblemsAsync(int page, int limit, string? title = null, byte? difficulty = null, int? createdBy = null,bool? isSystemProblem = null, DateTime? createdAt = null, IEnumerable<int>? tagIDs = null) {
            var problems = new List<ShortProblemModel>();

            try {
                using (SqlConnection connection = _db.GetConnection()) {
                    await connection.OpenAsync();
                    using (SqlCommand command = new("SP_Problem_GetAllProblems", connection)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Page", page);
                        command.Parameters.AddWithValue("@Limit", limit);
                        command.Parameters.AddWithValue("@Title", string.IsNullOrWhiteSpace(title) ? DBNull.Value : title);
                        command.Parameters.AddWithValue("@Difficulty", difficulty == null ? DBNull.Value : difficulty);
                        command.Parameters.AddWithValue("@CreatedBy", createdBy == null ? DBNull.Value : createdBy);
                        command.Parameters.AddWithValue("@IsSystemProblem", isSystemProblem == null ? DBNull.Value : isSystemProblem.Value);
                        command.Parameters.AddWithValue("@CreatedAt", createdAt == null ? DBNull.Value : createdAt);
                        SqlParameter TagIDsParm = new SqlParameter("@TagIDs", SqlDbType.Structured);
                        TagIDsParm.TypeName = "dbo.IntegersTableType";
                        DataTable dtTagIDs = new DataTable();
                        dtTagIDs.Columns.Add("val", typeof(int));
                        foreach (int x in tagIDs ?? [])
                            dtTagIDs.Rows.Add(x);
                        TagIDsParm.Value = dtTagIDs;
                        command.Parameters.Add(TagIDsParm);

                        using (var reader = await command.ExecuteReaderAsync()) {
                            while (await reader.ReadAsync()) {
                                int problemID = Convert.ToInt32(reader["ProblemID"]);
                                ShortProblemModel problem = new ShortProblemModel() {
                                    ProblemID = problemID,
                                    Title = reader["Title"].ToString() ?? "",
                                    GeneralDescription = reader["GeneralDescription"].ToString() ?? "",
                                    Difficulty = (Difficulty)Convert.ToInt32(reader["Difficulty"]),
                                    IsSystemProblem = Convert.ToBoolean(reader["IsSystemProblem"]),
                                    SolutionsCount = Convert.ToInt32(reader["SolutionsCount"]),
                                    AttemptsCount = Convert.ToInt32(reader["AttemptsCount"]),
                                    Tags = await GetProblemTagsAsync(problemID) ?? []
                                };
                                problems.Add(problem);
                            }
                        }

                        return problems;
                    }
                }
            }
            catch (Exception ex) {
                return null;
            }
        }
    }
}

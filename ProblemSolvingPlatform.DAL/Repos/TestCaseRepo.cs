using Microsoft.Data.SqlClient;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models.Tags;
using ProblemSolvingPlatform.DAL.Models.TestCases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos {
    public class TestCaseRepo : ITestCaseRepo {
        private readonly DbContext _db;

        public TestCaseRepo(DbContext db) {
            _db = db;
        }

        public async Task<IEnumerable<TestCaseModel>?> GetAllTestCasesAsync(int Page, int Limit, int? ProblemID = null, bool? IsSample = null, bool? IsPublic = null) {
            List<TestCaseModel> testCaseModels = new List<TestCaseModel>();

            try {
                using (SqlConnection connection = _db.GetConnection()) {
                    await connection.OpenAsync();
                    using (SqlCommand cmd = new("SP_TestCase_GetAllTestCases", connection)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Page", Page);
                        cmd.Parameters.AddWithValue("@Limit", Limit);
                        if (ProblemID != null) cmd.Parameters.AddWithValue("@ProblemID", ProblemID);
                        if (IsSample != null) cmd.Parameters.AddWithValue("@IsSample", IsSample);
                        if (IsPublic != null) cmd.Parameters.AddWithValue("@IsPublic", IsPublic);


                        using (var reader = await cmd.ExecuteReaderAsync()) {
                            while (await reader.ReadAsync()) {
                                TestCaseModel testcase = new TestCaseModel() {
                                    TestCaseID = (int)reader["TestCaseID"],
                                    ProblemID = (int)reader["ProblemID"],
                                    Input = (string)reader["Input"],
                                    Output = (string)reader["Output"],
                                    IsPublic = (bool)reader["IsPublic"],
                                    IsSample = (bool)reader["IsSample"]
                                };
                                testCaseModels.Add(testcase);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                return null;
            }
            return testCaseModels;
        }
    }
}

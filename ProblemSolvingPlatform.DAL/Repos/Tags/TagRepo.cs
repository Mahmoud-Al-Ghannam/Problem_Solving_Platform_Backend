using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models.Tags;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos.Tags {
    public class TagRepo : ITagRepo {
        private readonly DbContext _db;
        public TagRepo(DbContext dbContext) {
            _db = dbContext;
        }

        public async Task<int?> AddNewTagAsync(NewTagModel newTag) {

            int? tagID = null;
            try {
                using (SqlConnection connection = _db.GetConnection()) {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new("SP_Tag_AddNewTag", connection)) {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", newTag.Name);
                        // output 
                        var ParmTagID = new SqlParameter("@TagID", SqlDbType.Int) {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(ParmTagID);

                        var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(IsSuccess);

                        await cmd.ExecuteNonQueryAsync();

                        if ((bool)IsSuccess.Value)
                            tagID = (int)ParmTagID.Value;
                    }
                }
            }
            catch (Exception ex) {
                return null;
            }
            return tagID;
        }

        public async Task<bool> UpdateTagAsync(TagModel tag) {
            try {
                using (SqlConnection connection = _db.GetConnection()) {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new("SP_Tag_UpdateTag", connection)) {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TagID", tag.TagID);
                        cmd.Parameters.AddWithValue("@Name", tag.Name);
                        // output 
                        var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(IsSuccess);

                        await cmd.ExecuteNonQueryAsync();

                        if ((bool)IsSuccess.Value)
                            return true;
                    }
                }
            }
            catch (Exception ex) {}
            return false;
        }

        public async Task<bool> DeleteTagAsync(int tagID) {
            try {
                using (SqlConnection connection = _db.GetConnection()) {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new("SP_Tag_DeleteTag", connection)) {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TagID", tagID);
                        // output 
                        var IsSuccess = new SqlParameter("@IsSuccess", SqlDbType.Bit) {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(IsSuccess);

                        await cmd.ExecuteNonQueryAsync();

                        if ((bool)IsSuccess.Value)
                            return true;
                    }
                }
            }
            catch (Exception ex) { }
            return false;
        }

        public async Task<bool> DoesTagExistByIDAsync(int tagID) {
            try {
                using (SqlConnection connection = _db.GetConnection()) {

                    await connection.OpenAsync();

                    using (SqlCommand cmd = new("SP_Tag_DoesTagExistByID", connection)) {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TagID", tagID);


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

        public async Task<bool> DoesTagExistByNameAsync(string name) {
            try {
                using (SqlConnection connection = _db.GetConnection()) {

                    await connection.OpenAsync();

                    using (SqlCommand cmd = new("SP_Tag_DoesTagExistByName", connection)) {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", name);


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

        public async Task<IEnumerable<TagModel>?> GetAllTagsAsync() {
            List<TagModel> tags = new List<TagModel>();

            try {
                using (SqlConnection connection = _db.GetConnection()) {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new("SP_Tag_GetAllTags", connection)) {
                        cmd.CommandType = CommandType.StoredProcedure;

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

        
    }
}

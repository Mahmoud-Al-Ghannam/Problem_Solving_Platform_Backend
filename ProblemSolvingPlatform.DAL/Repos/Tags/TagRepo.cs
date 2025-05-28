using Azure;
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

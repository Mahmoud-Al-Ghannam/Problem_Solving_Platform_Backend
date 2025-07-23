using Microsoft.Identity.Client;
using ProblemSolvingPlatform.DAL.Models.Problems;
using ProblemSolvingPlatform.DAL.Models.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Repos.Tags {
    public interface ITagRepo {

        public Task<int?> AddNewTagAsync(NewTagModel newTag);
        public Task<bool> UpdateTagAsync(TagModel tag);
        public Task<bool> DeleteTagAsync(int tagID);
        public Task<TagModel?> GetTagByIDAsync(int tagID);
        public Task<TagModel?> GetTagByNameAsync(string name);
        public Task<bool> DoesTagExistByIDAsync(int tagID);
        public Task<bool> DoesTagExistByNameAsync(string name);
        public Task<IEnumerable<TagModel>?> GetAllTagsAsync ();
    }
}

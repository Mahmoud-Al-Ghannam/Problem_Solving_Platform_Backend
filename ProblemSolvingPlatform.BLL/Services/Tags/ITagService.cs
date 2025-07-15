using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.DAL.Models.Tags;

namespace ProblemSolvingPlatform.BLL.Services.Tags {
    public interface ITagService {
        public Task<int?> AddNewTagAsync(NewTagDTO newTag);
        public Task<bool> UpdateTagAsync(TagDTO tag);
        public Task<bool> DeleteTagAsync(int tagID);
        public Task<bool> DoesTagExistByIDAsync(int tagID);
        public Task<bool> DoesTagExistByNameAsync(string name);
        public Task<IEnumerable<TagDTO>?> GetAllTagsAsync();
    }
}
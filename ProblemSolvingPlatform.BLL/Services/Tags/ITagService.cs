using ProblemSolvingPlatform.BLL.DTOs.Tags;

namespace ProblemSolvingPlatform.BLL.Services.Tags {
    public interface ITagService {
        public Task<int?> AddNewTagAsync(NewTagDTO newTag);
        public Task<bool> UpdateTagAsync(TagDTO tag);
        public Task<bool> DeleteTagAsync(int tagID);
        public Task<TagDTO?> GetTagByIDAsync(int tagID);
        public Task<TagDTO?> GetTagByNameAsync(string name);
        public Task<bool> DoesTagExistByIDAsync(int tagID);
        public Task<bool> DoesTagExistByNameAsync(string name);
        public Task<IEnumerable<TagDTO>?> GetAllTagsAsync();
    }
}
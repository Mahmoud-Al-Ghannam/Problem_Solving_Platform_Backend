using ProblemSolvingPlatform.BLL.DTOs.Tags;

namespace ProblemSolvingPlatform.BLL.Services.Tags {
    public interface ITagService {
        public Task<IEnumerable<TagDTO>?> GetAllTagsAsync();
    }
}
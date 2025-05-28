using ProblemSolvingPlatform.BLL.DTOs.Tags;

namespace ProblemSolvingPlatform.BLL.Services.Tag {
    public interface ITagService {
        public Task<IEnumerable<TagDTO>> GetAllTagsAsync();
    }
}
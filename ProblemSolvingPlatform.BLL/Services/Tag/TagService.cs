using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.DAL.Repos.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Tag {
    public class TagService : ITagService {

        private readonly ITagRepo _tagRepo;
        public TagService(ITagRepo tagRepo) {
            _tagRepo = tagRepo;
        }

        public async Task<IEnumerable<TagDTO>> GetAllTagsAsync() {
            var tagsModel = await _tagRepo.GetAllTagsAsync();

            var tagsDTO = tagsModel.Select(tm => new  TagDTO () {
                TagID = tm.TagID,
                Name = tm.Name
            }).ToList();

            return tagsDTO;
        }

    }
}

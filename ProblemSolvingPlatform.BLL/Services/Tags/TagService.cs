using Azure;
using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.DAL.Models.Tags;
using ProblemSolvingPlatform.DAL.Repos.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Tags {
    public class TagService : ITagService {

        private readonly ITagRepo _tagRepo;
        public TagService(ITagRepo tagRepo) {
            _tagRepo = tagRepo;
        }

        public async Task<bool> DoesTagExistByIDAsync(int tagID) {
            return await _tagRepo.DoesTagExistByIDAsync(tagID);
        }

        public async Task<bool> DoesTagExistByNameAsync(string name) {
            return await _tagRepo.DoesTagExistByNameAsync(name);
        }

        public async Task<int?> AddNewTagAsync(NewTagDTO newTag) {

            if (await _tagRepo.DoesTagExistByNameAsync(newTag.Name))
                throw new CustomValidationException("Name", [$"The tag with name = {newTag.Name} is already exists"]);

            return await _tagRepo.AddNewTagAsync(new NewTagModel() { Name = newTag.Name });
        }
        public async Task<bool> UpdateTagAsync(TagDTO tag) {
            if (!await _tagRepo.DoesTagExistByIDAsync(tag.TagID))
                throw new CustomValidationException("TagID", [$"The tag with id = {tag.TagID} was not found"]);


            return await _tagRepo.UpdateTagAsync(
                new TagModel() { 
                    TagID = tag.TagID, 
                    Name = tag.Name 
            });
        }

        public async Task<bool> DeleteTagAsync(int tagID) {
            if (!await _tagRepo.DoesTagExistByIDAsync(tagID))
                throw new CustomValidationException("TagID", [$"The tag with id = {tagID} was not found"]);


            return await _tagRepo.DeleteTagAsync(tagID);
        }

        public async Task<IEnumerable<TagDTO>?> GetAllTagsAsync() {
            var tagsModel = await _tagRepo.GetAllTagsAsync();

            var tagsDTO = tagsModel?.Select(tm => new TagDTO() {
                TagID = tm.TagID,
                Name = tm.Name
            }).ToList();

            return tagsDTO;
        }

    }
}

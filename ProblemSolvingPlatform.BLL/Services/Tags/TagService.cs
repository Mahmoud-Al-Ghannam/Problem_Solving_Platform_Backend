using Azure;
using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Options.Constraint;
using ProblemSolvingPlatform.DAL.Models.Tags;
using ProblemSolvingPlatform.DAL.Repos.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProblemSolvingPlatform.BLL.Services.Tags {
    public class TagService : ITagService {

        private readonly ITagRepo _tagRepo;
        private readonly ConstraintsOption _constraintsOption;
        public TagService(ITagRepo tagRepo, ConstraintsOption constraintsOption) {
            _tagRepo = tagRepo;
            _constraintsOption = constraintsOption;
        }

        public async Task<bool> DoesTagExistByIDAsync(int tagID) {
            return await _tagRepo.DoesTagExistByIDAsync(tagID);
        }

        public async Task<bool> DoesTagExistByNameAsync(string name) {
            return await _tagRepo.DoesTagExistByNameAsync(name);
        }

        public async Task<int?> AddNewTagAsync(NewTagDTO newTag) {
            Dictionary<string, List<string>> errors = new();
            errors["Name"] = [];

            if (await _tagRepo.DoesTagExistByNameAsync(newTag.Name))
                errors["Name"].Add($"The tag with name = {newTag.Name} is already exists");

            if (newTag.Name.Length < _constraintsOption.Tag.NameLength.Start.Value ||
                newTag.Name.Length > _constraintsOption.Tag.NameLength.End.Value)
                errors["Name"].Add($"The length of tag's name must to be in range [{_constraintsOption.Tag.NameLength.Start.Value},{_constraintsOption.Tag.NameLength.End.Value}]");

            errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
            if (errors.Count > 0) throw new CustomValidationException(errors);

            return await _tagRepo.AddNewTagAsync(new NewTagModel() { Name = newTag.Name });
        }
        public async Task<bool> UpdateTagAsync(TagDTO tag) {
            Dictionary<string, List<string>> errors = new();
            errors["TagID"] = [];
            errors["Name"] = [];

            if (!await _tagRepo.DoesTagExistByIDAsync(tag.TagID))
                errors["TagID"].Add($"The tag with id = {tag.TagID} was not found");


            if (tag.Name.Length < _constraintsOption.Tag.NameLength.Start.Value ||
               tag.Name.Length > _constraintsOption.Tag.NameLength.End.Value)
                errors["Name"].Add($"The length of tag's name must to be in range [{_constraintsOption.Tag.NameLength.Start.Value},{_constraintsOption.Tag.NameLength.End.Value}]");


            errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
            if (errors.Count > 0) throw new CustomValidationException(errors);

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

using AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.BLL.Services.Tags;
using ProblemSolvingPlatform.Responses;

namespace ProblemSolvingPlatform.Controllers {

    [ApiController]
    [Route("/api/tags")]
    public class TagsController : GeneralController {

        private readonly ITagService _tagService;

        public TagsController(ITagService tagService) {
            _tagService = tagService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = BLL.Constants.Roles.System)]
        public async Task<ActionResult<int?>> AddNewTag(NewTagDTO newTag) {
            int? id = null;
            id = await _tagService.AddNewTagAsync(newTag);
            if (id == null)
                return StatusCode(StatusCodes.Status500InternalServerError,new ErrorResponseBody("Some error occurred"));
            return Ok(id);
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = BLL.Constants.Roles.System)]
        public async Task<ActionResult<int?>> UpdateTag(TagDTO tag) {
            bool ok = await _tagService.UpdateTagAsync(tag);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody("Some error occurred"));
            return Ok(ok);
        }

        [HttpDelete ("{tagID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = BLL.Constants.Roles.System)]
        public async Task<ActionResult<int?>> DeleteTag(int tagID) {
            bool ok = await _tagService.DeleteTagAsync(tagID);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody("Some error occurred"));
            return Ok(ok);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetAllTags() {
            var tags = await _tagService.GetAllTagsAsync();
            if (tags == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(tags);
        }
    }
}

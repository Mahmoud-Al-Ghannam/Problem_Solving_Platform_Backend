using AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.BLL.Services.Tags;
using ProblemSolvingPlatform.Responses;

namespace ProblemSolvingPlatform.Controllers
{

    [ApiController]
    [Route($"/{Constants.Api.PrefixApi}/tags")]
    public class TagsController : GeneralController
    {

        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }


        /// <summary>
        /// No Auth
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetAllTags()
        {
            var tags = await _tagService.GetAllTagsAsync();
            if (tags == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return Ok(tags);
        }

        /// <summary>
        /// JWt Bearer Auth With System Role
        /// </summary>
        /// <param name="newTag"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = Constants.Roles.System)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int?>> AddNewTag(NewTagDTO newTag)
        {
            int? id = null;
            id = await _tagService.AddNewTagAsync(newTag);
            if (id == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return Ok(id);
        }


        /// <summary>
        /// JWt Bearer Auth With System Role
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = Constants.Roles.System)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<int?>> UpdateTag(TagDTO tag)
        {
            bool ok = await _tagService.UpdateTagAsync(tag);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return NoContent();
        }

        /// <summary>
        /// JWt Bearer Auth With System Role
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns></returns>
        [HttpDelete("{tagID}")]
        [Authorize(Roles = Constants.Roles.System)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteTag(int tagID)
        {
            bool ok = await _tagService.DeleteTagAsync(tagID);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(Constants.ErrorMessages.General));
            return NoContent();
        }
    }
}

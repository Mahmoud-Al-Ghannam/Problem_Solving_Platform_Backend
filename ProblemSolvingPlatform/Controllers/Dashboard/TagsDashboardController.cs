using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.API.DTOs;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.BLL.Services.Tags;
using ProblemSolvingPlatform.Responses;

namespace ProblemSolvingPlatform.Controllers.Dashboard {

    [ApiController]
    [Route($"/{Constants.Api.PrefixDashboardApi}/tags")]
    public class TagsDashboardController : DashboardController {

        private readonly ITagService _tagService;

        public TagsDashboardController(ITagService tagService) {
            _tagService = tagService;
        }

        /// <summary>
        /// JWt Bearer Auth With System Role
        /// </summary>
        /// <param name="newTag"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int?>> AddNewTag(NewTagDTO newTag) {
            int? id = null;
            id = await _tagService.AddNewTagAsync(newTag);
            if (id == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
            return Ok(id);
        }


        /// <summary>
        /// JWt Bearer Auth With System Role
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<int?>> UpdateTag(TagDTO tag) {
            bool ok = await _tagService.UpdateTagAsync(tag);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
            return NoContent();
        }

        /// <summary>
        /// JWt Bearer Auth With System Role
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns></returns>
        [HttpDelete("{tagID}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteTag(int tagID) {
            bool ok = await _tagService.DeleteTagAsync(tagID);
            if (!ok) return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseBody(BLL.Constants.ErrorMessages.General));
            return NoContent();
        }

    }
}

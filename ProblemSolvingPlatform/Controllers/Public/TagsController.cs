using AuthHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.BLL.Services.Tags;
using ProblemSolvingPlatform.Responses;

namespace ProblemSolvingPlatform.Controllers.Public
{

    [ApiController]
    [Route($"/{Constants.Api.PrefixPublicApi}/tags")]
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
    }
}

using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetAllTags () {
            var tags = await _tagService.GetAllTagsAsync();
            if (tags == null)
                return StatusCode (StatusCodes.Status500InternalServerError);
            return Ok(tags);
        }
    }
}

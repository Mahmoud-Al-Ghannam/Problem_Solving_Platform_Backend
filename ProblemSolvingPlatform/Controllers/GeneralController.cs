using Microsoft.AspNetCore.Mvc;
using ProblemSolvingPlatform.Responses;

namespace ProblemSolvingPlatform.Controllers {

    [ProducesResponseType(typeof(BadRequestResponseBody), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseBody), StatusCodes.Status500InternalServerError)]
    public abstract class GeneralController : ControllerBase {
        
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProblemSolvingPlatform.Controllers.Dashboard {


    [Authorize(Roles = BLL.Constants.Roles.System)]
    public class DashboardController : GeneralController {
       
    }
}

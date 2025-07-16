using Microsoft.AspNetCore.Mvc.Filters;
using ProblemSolvingPlatform.BLL.Helpers;

namespace ProblemSolvingPlatform.ActionFilters {
    public class TrimStringsActionFilter : IActionFilter {

        public void OnActionExecuting(ActionExecutingContext context) {
            HashSet<object> hashSet = new HashSet<object>();
            foreach (var key in context.ActionArguments.Keys) {
                context.ActionArguments[key] = StringHelper.TrimStringsOfObject(context.ActionArguments[key], hashSet);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) {}
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using ProblemSolvingPlatform.BLL.Exceptions;
using System.Text;

namespace ProblemSolvingPlatform.Middlewares {
    public class ExceptionMiddleware {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            try {
                await _next(context);
            }
            catch (CustomValidationException ex) {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                var json = JsonConvert.SerializeObject(ex.errors);
                context.Response.ContentLength = Encoding.UTF8.GetBytes(json).Length;
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex) {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var json = JsonConvert.SerializeObject(new { error = ex.Message });
                context.Response.ContentLength = Encoding.UTF8.GetBytes(json).Length;
                await context.Response.WriteAsync(json);
            }
        }
    }
}

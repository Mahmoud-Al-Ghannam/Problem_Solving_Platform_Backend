using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.Responses;
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
                BadRequestResponseBody responseBody = new BadRequestResponseBody(ex.errors);    
                var json = JsonConvert.SerializeObject(responseBody);
                context.Response.ContentLength = Encoding.UTF8.GetBytes(json).Length;
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex) {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                ErrorResponseBody responseBody = new ErrorResponseBody(ex.Message);
                var json = JsonConvert.SerializeObject(responseBody);
                context.Response.ContentLength = Encoding.UTF8.GetBytes(json).Length;
                await context.Response.WriteAsync(json);
            }
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Services.JWT;
using ProblemSolvingPlatform.BLL.Services.Users;
using ProblemSolvingPlatform.Responses;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

namespace ProblemSolvingPlatform.Middlewares {
    public class VerifyUserActivationMiddleware : IMiddleware {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public VerifyUserActivationMiddleware(ITokenService tokenService, IUserService userService) {
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            var endpoint = context.GetEndpoint();
            try {
                if (endpoint != null) {
                    var authorizeAttribute = endpoint.Metadata.GetMetadata<AuthorizeAttribute>();

                    if (authorizeAttribute != null) {
                        string jwtToken = "";
                        if (context.Request.Headers["Authorization"].Count > 0)
                            jwtToken = await context.GetTokenAsync("Bearer", "access_token") ?? "";
                        ClaimsPrincipal? principal = _tokenService.GetPrincipalFromToken(jwtToken);

                        if (principal != null) {
                            string nameid = principal.Claims.First(c => c.Type == "nameid").Value;
                            int userID = int.Parse(nameid);

                            if (!await _userService.IsUserActiveByIDAsync(userID))
                                throw new CustomValidationException("UserID", [$"The user with id = {userID} was not active"]);
                        }
                    }
                }

                await next(context);
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

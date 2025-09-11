
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProblemSolvingPlatform.BLL;
using ProblemSolvingPlatform.BLL.DTOs.RequestApiLogs;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Services.JWT;
using ProblemSolvingPlatform.BLL.Services.RequestApiLogs;
using ProblemSolvingPlatform.BLL.Services.Users;
using ProblemSolvingPlatform.DAL.Repos.RequestApiLogs;
using ProblemSolvingPlatform.Responses;
using System.Security.Claims;
using System.Text;

namespace ProblemSolvingPlatform.Middlewares {
    public class RequestApiLogMiddleware : IMiddleware {

        private readonly ITokenService _tokenService;
        private readonly IRequestApiLogService _requestApiLogService;

        public RequestApiLogMiddleware(ITokenService tokenService, IRequestApiLogService requestApiLogService) {
            _tokenService = tokenService;
            _requestApiLogService = requestApiLogService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            string jwtToken = "";
            NewRequestApiLogDTO newLog = new NewRequestApiLogDTO() {
                UserID = null,
                RequestBody = null,
                ResponseBody = null,
                Endpoint = context.Request.Path,
                RequestType = context.Request.Method,
                ProcessingTimeMS = 0
            };

            if (context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/health") ||
            context.Request.Path.StartsWithSegments($"/{Constants.Api.PrefixApi}/request-api-logs/dashboard")) {
                await next(context);
                return;
            }


            var startTime = DateTime.UtcNow;

            if (context.Request.Method == HttpMethods.Post ||
                context.Request.Method == HttpMethods.Put ||
                context.Request.Method == HttpMethods.Patch) {
                context.Request.EnableBuffering();
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true)) {
                    newLog.RequestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
            }

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream()) {
                context.Response.Body = responseBody;

                // متابعة معالجة الطلب
                await next(context);

                // حساب وقت المعالجة
                newLog.ProcessingTimeMS = (int)(DateTime.UtcNow - startTime).TotalMilliseconds;
                newLog.RequestHeaders = GetHeadersAsString(context.Request.Headers);
                newLog.ResponseHeaders = GetHeadersAsString(context.Response.Headers);
                newLog.StatusCode = context.Response.StatusCode;
                // قراءة body الاستجابة
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                newLog.ResponseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                await responseBody.CopyToAsync(originalBodyStream);

                var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedUserId)) {
                    newLog.UserID = parsedUserId;
                }
            }


            await _requestApiLogService.AddLogAsync(newLog);
        }


        private string GetHeadersAsString(IHeaderDictionary headers) {
            var stringBuilder = new StringBuilder();
            foreach (var header in headers) {
                stringBuilder.AppendLine($"{header.Key}: {header.Value}");
            }
            return stringBuilder.ToString();
        }
    }


}

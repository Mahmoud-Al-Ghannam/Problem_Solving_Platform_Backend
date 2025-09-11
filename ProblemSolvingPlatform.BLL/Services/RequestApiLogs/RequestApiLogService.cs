using Azure;
using Microsoft.AspNetCore.Http;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.RequestApiLogs;
using ProblemSolvingPlatform.BLL.DTOs.Users;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Options.Constraint;
using ProblemSolvingPlatform.DAL.Models.RequestApiLogs;
using ProblemSolvingPlatform.DAL.Models.Tags;
using ProblemSolvingPlatform.DAL.Repos.RequestApiLogs;
using ProblemSolvingPlatform.DAL.Repos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.RequestApiLogs {
    public class RequestApiLogService : IRequestApiLogService {
        private IRequestApiLogRepo _logRepo { get; }
        private readonly ConstraintsOption _constraintsOption;
        public RequestApiLogService(IRequestApiLogRepo logRepo, ConstraintsOption constraintsOption) {
            _logRepo = logRepo;
            _constraintsOption = constraintsOption;
        }

        public async Task<int?> AddLogAsync(NewRequestApiLogDTO newLog) {
            Dictionary<string, List<string>> errors = new();

            if (new List<string>(["GET","PUT","POST","DELETE","PATCH"]).Contains(newLog.RequestType) == false) {
                errors["RequestType"] = [$"The request type should be one of these values: GET, PUT, POST, DELETE and PATCH"];
            }

            errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
            if (errors.Count > 0) throw new CustomValidationException(errors);

            return await _logRepo.AddLogAsync(new NewRequestApiLogModel() { 
                UserID = newLog.UserID,
                Endpoint = newLog.Endpoint,
                RequestType = newLog.RequestType,
                RequestBody = newLog.RequestBody,
                RequestHeaders = newLog.RequestHeaders,
                ResponseBody = newLog.ResponseBody,
                ResponseHeaders = newLog.ResponseHeaders,
                StatusCode = newLog.StatusCode,
                ProcessingTimeMS = newLog.ProcessingTimeMS,
            });
        }

        public async Task<PageDTO<RequestApiLogDTO>?> GetAllLogsAsync(int page, int limit, string? username = null, string? requestType = null, int? statusCode = null, string? endpoint = null) {
            Dictionary<string, List<string>> errors = new();
            errors["Page"] = [];
            errors["Limit"] = [];

            if (page < _constraintsOption.MinPageNumber)
                errors["Page"].Add($"The page must to be greater than {_constraintsOption.MinPageNumber}");
            if (limit < _constraintsOption.PageSize.Start.Value || limit > _constraintsOption.PageSize.End.Value)
                errors["Limit"].Add($"The limit must to be in range [{_constraintsOption.PageSize.Start.Value},{_constraintsOption.PageSize.End.Value}]");


            errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
            if (errors.Count > 0) throw new CustomValidationException(errors);

            var pageModel = await _logRepo.GetAllLogsAsync(page, limit,username,requestType,statusCode,endpoint);
            if (pageModel == null) return null;


            return new PageDTO<RequestApiLogDTO>() {
                Items = pageModel.Items.Select(x => new RequestApiLogDTO() {
                    RequestApiLogID = x.RequestApiLogID,
                    UserID = x.UserID,
                    Username = x.Username,
                    Endpoint = x.Endpoint,
                    RequestBody = x.RequestBody,
                    RequestHeaders = x.RequestHeaders,
                    RequestType = x.RequestType,
                    ResponseBody = x.ResponseBody,
                    ResponseHeaders = x.ResponseHeaders,
                    StatusCode = x.StatusCode,
                    ProcessingTimeMS = x.ProcessingTimeMS,
                    CreatedAt = x.CreatedAt,
                }).ToList(),

                TotalItems = pageModel.TotalItems,
                TotalPages = pageModel.TotalPages,
                CurrentPage = pageModel.CurrentPage
            };
        }
    }
}

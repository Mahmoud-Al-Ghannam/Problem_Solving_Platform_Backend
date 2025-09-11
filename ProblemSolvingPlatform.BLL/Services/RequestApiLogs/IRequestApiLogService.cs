using ProblemSolvingPlatform.DAL.Models.RequestApiLogs;
using ProblemSolvingPlatform.DAL.Models;
using ProblemSolvingPlatform.BLL.DTOs.RequestApiLogs;
using ProblemSolvingPlatform.BLL.DTOs;

namespace ProblemSolvingPlatform.BLL.Services.RequestApiLogs {
    public interface IRequestApiLogService {

        public Task<int?> AddLogAsync(NewRequestApiLogDTO newLog);

        public Task<PageDTO<RequestApiLogDTO>?> GetAllLogsAsync(int page, int limit, string? username = null, string? requestType = null, int? statusCode = null, string? endpoint = null);
        
    }
}

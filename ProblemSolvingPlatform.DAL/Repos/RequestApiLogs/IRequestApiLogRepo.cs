using ProblemSolvingPlatform.DAL.Models.Problems;
using ProblemSolvingPlatform.DAL.Models.Tags;
using ProblemSolvingPlatform.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.DAL.Models.Enums;
using ProblemSolvingPlatform.DAL.Models.RequestApiLogs;

namespace ProblemSolvingPlatform.DAL.Repos.RequestApiLogs {
    public interface IRequestApiLogRepo {
        public Task<int?> AddLogAsync(NewRequestApiLogModel newLog);
       
        public Task<PageModel<RequestApiLogModel>?> GetAllLogsAsync(int page, int limit, string? username = null, string? requestType = null, int? statusCode = null,string? endpoint = null);
        public Task<(int totalPages, int totalItems)?> GetTotalPagesAndItemsCountAsync(int limit, string? username = null, string? requestType = null, int? statusCode = null, string? endpoint = null);

    }
}

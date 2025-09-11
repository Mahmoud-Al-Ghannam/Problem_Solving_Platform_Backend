using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.RequestApiLogs {
    public class RequestApiLogDTO {
        public int RequestApiLogID { get; set; }
        public int? UserID { get; set; }
        public string? Username { get; set; }
        public string Endpoint { get; set; }
        public string RequestType { get; set; }
        public string? RequestBody { get; set; }
        public string RequestHeaders { get; set; }
        public string? ResponseBody { get; set; }
        public string ResponseHeaders { get; set; }
        public int StatusCode { get; set; }
        public int ProcessingTimeMS { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

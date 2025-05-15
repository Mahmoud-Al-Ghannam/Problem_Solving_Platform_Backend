using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.API.Base {
    public class ApiException : Exception {
        public int StatusCode { get; }
        public string Content { get; set; }

        public ApiException(int StatusCode, string Content)
        : base(Content) {
            this.StatusCode = StatusCode;
            this.Content = Content;
        }
    }
}

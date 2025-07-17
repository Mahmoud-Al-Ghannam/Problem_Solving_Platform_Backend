using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL {
    public class Constants {

        public class PaginationDefaultValues {
            public const int Page = 1;
            public const int Limit = 20;
        }

        public class ErrorMessages {
            public const string General = "Some error occurred";
            public const string JwtDoesnotIncludeSomeFields = "The token does not include some fields";
        }
        public class Roles {
            public const string System = "System";
            public const string User = "User";
        }
    }
}

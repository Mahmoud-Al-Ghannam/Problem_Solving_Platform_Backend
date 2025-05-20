using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.UserProfile {

    public class UpdateUserDTO {
        public IFormFile? profileImage { get; set; }
    }
}
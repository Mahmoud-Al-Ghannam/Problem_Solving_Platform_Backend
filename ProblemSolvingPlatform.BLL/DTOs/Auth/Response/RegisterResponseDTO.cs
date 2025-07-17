using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.Auth.Response {

    public class RegisterResponseDTO {
        public int UserID {  get; set; }
        public string Username { get; set; }
        public string? Token { get; set; }
    }
}
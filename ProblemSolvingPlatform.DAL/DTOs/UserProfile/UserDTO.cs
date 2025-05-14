using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.DTOs.UserProfile {
    public class UserDTO {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ImagePath { get; set; }
        public byte role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

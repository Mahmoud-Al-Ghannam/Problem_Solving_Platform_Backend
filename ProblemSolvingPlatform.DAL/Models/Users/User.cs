using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.DAL.Models.Users {

    public class User {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? ImagePath { get; set; }
        public byte Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

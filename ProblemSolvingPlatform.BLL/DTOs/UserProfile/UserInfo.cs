using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.DTOs.UserProfile;

public class UserInfo
{
    public int UserId {  get; set; }
    public string Username { get; set; }
    public string? ImagePath { get; set; }
    public byte role { get; set; }
    public DateTime CreatedAt { get; set; }
}

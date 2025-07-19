using ProblemSolvingPlatform.BLL.DTOs;
using System.Security.Claims;

namespace ProblemSolvingPlatform.BLL.Services.JWT {
    public interface ITokenService {
        string GenerateToken(int userID, Enums.Role role);
        ClaimsPrincipal? GetPrincipalFromToken(string token);
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.UserProfile;
using ProblemSolvingPlatform.BLL.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProblemSolvingPlatform.BLL.Services.JWT;

public class TokenService
{
    private readonly JwtOption _jwtOption;

    public TokenService(JwtOption jwtOption) {
        _jwtOption = jwtOption;
    }

    public string GenerateToken(int userID,Enums.Role role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor() { 
            Audience = _jwtOption.Audience,
            Issuer = _jwtOption.Issuer,
            Expires = DateTime.UtcNow.AddMinutes(_jwtOption.LifeTimeMin),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key)),
                SecurityAlgorithms.HmacSha256
            ),

            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, userID.ToString()),
                new Claim(ClaimTypes.Role,(role == Enums.Role.System ? "System" : "User"))
            })
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        string accessToken = tokenHandler.WriteToken(token);
        return accessToken;
    }
}

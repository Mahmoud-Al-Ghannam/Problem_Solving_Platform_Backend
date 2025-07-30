using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Users;
using ProblemSolvingPlatform.BLL.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProblemSolvingPlatform.BLL.Services.JWT;

public class TokenService : ITokenService {
    private readonly JwtOption _jwtOption;

    public TokenService(JwtOption jwtOption) {
        _jwtOption = jwtOption;
    }

    public string GenerateToken(int userID, Enums.Role role) {
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
                new Claim(ClaimTypes.Role,(role == Enums.Role.System ? Constants.Roles.System : Constants.Roles.User))
            })
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        string accessToken = tokenHandler.WriteToken(token);
        return accessToken;
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string token) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtOption.Key);

        try {
            tokenHandler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuer = true,
                ValidIssuer = _jwtOption.Issuer,

                ValidateAudience = true,
                ValidAudience = _jwtOption.Audience,

                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key))

            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var claims = jwtToken.Claims.ToList();
            claims.Add(new Claim("JWT_TOKEN", token));

            var identity = new ClaimsIdentity(claims, "jwt");
            return new ClaimsPrincipal(identity);
        }
        catch {
            return null;
        }
    }
}

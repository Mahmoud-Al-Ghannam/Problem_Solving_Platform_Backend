
using Microsoft.IdentityModel.Tokens;
using ProblemSolvingPlatform.BLL.Services.Auth;
using ProblemSolvingPlatform.BLL.Services.JWT;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Repos.User;
using System.Text;

namespace ProblemSolvingPlatform
{
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<TokenService, TokenService>();
            builder.Services.AddScoped<DbContext, DbContext>();

            builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                         ValidateIssuer = false, 
                         ValidateAudience = false, 
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                         )
                };
            });

            builder.Services.AddAuthorization();


            var app = builder.Build();


                app.UseSwagger();
                app.UseSwaggerUI();
            

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

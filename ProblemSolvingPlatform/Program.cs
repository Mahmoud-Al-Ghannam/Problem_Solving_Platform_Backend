
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProblemSolvingPlatform.API.Compiler.Services;
using ProblemSolvingPlatform.BLL.Services.Auth;
using ProblemSolvingPlatform.BLL.Services.Compiler;
using ProblemSolvingPlatform.BLL.Services.JWT;
using ProblemSolvingPlatform.BLL.Services.Problem;
using ProblemSolvingPlatform.BLL.Services.Submissions;
using ProblemSolvingPlatform.BLL.Services.Submissions.Handling_Submission;
using ProblemSolvingPlatform.BLL.Services.Tag;
using ProblemSolvingPlatform.BLL.Services.User;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Repos.Problems;
using ProblemSolvingPlatform.DAL.Repos.Submissions;
using ProblemSolvingPlatform.DAL.Repos.Tags;
using ProblemSolvingPlatform.DAL.Repos.Users;
using ProblemSolvingPlatform.Middlewares;
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
            builder.Services.AddScoped<IProblemRepo, ProblemRepo>();
            builder.Services.AddScoped<ITagRepo, TagRepo>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<IProblemService, ProblemService>();
            builder.Services.AddScoped<TokenService, TokenService>();
            builder.Services.AddScoped<DbContext, DbContext>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICompilerApiService,CompilerApiService>();
            builder.Services.AddScoped<ICompilerService,CompilerService>();
            builder.Services.AddScoped<ISubmissionsService, SubmissionsService>();
            builder.Services.AddScoped<ISubmissionRepo, SubmissionRepo>();
            builder.Services.AddScoped<SubmissionHandler, SubmissionHandler>(); 


            builder.Services.AddHttpContextAccessor();

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

            #region authSwagger
            builder.Services.AddSwaggerGen(options =>
            {
                // Add JWT Bearer token support in Swagger UI
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your valid token in the text input below.\n\nExample: `Bearer eyJhbGciOi...`"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                    }
                });
            });
            #endregion

            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthorization();


            var app = builder.Build();

            app.UseCors("AllowAll");
                app.UseSwagger();
                app.UseSwaggerUI();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();


            app.Run();
        }
    }
}

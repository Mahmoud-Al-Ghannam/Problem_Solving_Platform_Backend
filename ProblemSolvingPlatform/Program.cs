
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProblemSolvingPlatform.API.Compiler.Services;
using ProblemSolvingPlatform.BLL.Services.Auth;
using ProblemSolvingPlatform.BLL.Services.Compiler;
using ProblemSolvingPlatform.BLL.Services.JWT;
using ProblemSolvingPlatform.BLL.Services.Problems;
using ProblemSolvingPlatform.BLL.Services.Submissions;
using ProblemSolvingPlatform.BLL.Services.Submissions.Handling_Submission;
using ProblemSolvingPlatform.BLL.Services.Tags;
using ProblemSolvingPlatform.BLL.Services.TestCases;
using ProblemSolvingPlatform.BLL.Services.Users;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Repos;
using ProblemSolvingPlatform.DAL.Repos.Problems;
using ProblemSolvingPlatform.DAL.Repos.Submissions;
using ProblemSolvingPlatform.DAL.Repos.Tags;
using ProblemSolvingPlatform.DAL.Repos.Tests;
using ProblemSolvingPlatform.DAL.Repos.Users;
using ProblemSolvingPlatform.Middlewares;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace ProblemSolvingPlatform
{
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo() {
                    Title = "Problem Solving Platform APIs",
                    Version = "V1",
                    Description = "The backend team is king",
                    Contact = new OpenApiContact() {
                        Name = "Mahmoud Al-Ghannam & Abd Almalek Mokresh"
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();

            });

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<TokenService, TokenService>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProblemRepo, ProblemRepo>();
            builder.Services.AddScoped<IProblemService, ProblemService>();
            builder.Services.AddScoped<ITagRepo, TagRepo>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ITestCaseRepo, TestCaseRepo>();
            builder.Services.AddScoped<ITestCaseService, TestCaseService>();
            builder.Services.AddScoped<DbContext, DbContext>();
            builder.Services.AddScoped<ICompilerApiService, CompilerApiService>();
            builder.Services.AddScoped<ICompilerService, CompilerService>();
            builder.Services.AddScoped<ISubmissionService, SubmissionService>();
            builder.Services.AddScoped<ISubmissionRepo, SubmissionRepo>();
            builder.Services.AddScoped<SubmissionHandler, SubmissionHandler>();
            builder.Services.AddScoped<ISubmissionTestRepo, SubmissionTestRepo>();


            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                         )
                };
            });
            builder.Services.AddControllers()
            .AddJsonOptions(options =>
               {
                   options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
               });


            #region authSwagger
            builder.Services.AddSwaggerGen(options => {
                // Add JWT Bearer token support in Swagger UI
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
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
                options.UseInlineDefinitionsForEnums(); 
            });
            #endregion


            builder.Services.AddCors(options => {
                options.AddPolicy("AllowAll", builder => {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthorization();


            var app = builder.Build();

            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Problem Solving APIs V1");
            });

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

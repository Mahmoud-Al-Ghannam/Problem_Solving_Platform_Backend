
using Microsoft.Extensions.DependencyInjection;
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
using ProblemSolvingPlatform.BLL.Options.Constraint;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using ProblemSolvingPlatform.BLL.Validation.Problem;

namespace ProblemSolvingPlatform
{
    public class Program {

        private static ConstraintsOption GetConstraintsFromConfiguration(ConfigurationManager config) {
            ConstraintsOption constraints = new ConstraintsOption();

            #region Problem 
            constraints.Problem.TitleLength = new Range(
                int.Parse(config["Constraints:Problem:Title:MinLength"] ?? "0"),
                int.Parse(config["Constraints:Problem:Title:MaxLength"] ?? "0")
            );
            constraints.Problem.GeneralDescriptionLength = new Range(
                int.Parse(config["Constraints:Problem:GeneralDescription:MinLength"] ?? "0"),
                int.Parse(config["Constraints:Problem:GeneralDescription:MaxLength"] ?? "0")
            );
            constraints.Problem.InputDescriptionLength = new Range(
                int.Parse(config["Constraints:Problem:InputDescription:MinLength"] ?? "0"),
                int.Parse(config["Constraints:Problem:InputDescription:MaxLength"] ?? "0")
            );
            constraints.Problem.OutputDescriptionLength = new Range(
                int.Parse(config["Constraints:Problem:OutputDescription:MinLength"] ?? "0"),
                int.Parse(config["Constraints:Problem:OutputDescription:MaxLength"] ?? "0")
            );
            constraints.Problem.NoteLength = new Range(
                int.Parse(config["Constraints:Problem:Note:MinLength"] ?? "0"),
                int.Parse(config["Constraints:Problem:Note:MaxLength"] ?? "0")
            );
            constraints.Problem.TutorialLength = new Range(
                int.Parse(config["Constraints:Problem:Tutorial:MinLength"] ?? "0"),
                int.Parse(config["Constraints:Problem:Tutorial:MaxLength"] ?? "0")
            );
            constraints.Problem.SolutionCodeLength = new Range(
                int.Parse(config["Constraints:Problem:SolutionCode:MinLength"] ?? "0"),
                int.Parse(config["Constraints:Problem:SolutionCode:MaxLength"] ?? "0")
            );
            constraints.Problem.NoTotalTestCases = new Range(
               int.Parse(config["Constraints:Problem:NoTotalTestCases:Min"] ?? "0"),
               int.Parse(config["Constraints:Problem:NoTotalTestCases:Max"] ?? "0")
            );
            constraints.Problem.NoSampleTestCases = new Range(
               int.Parse(config["Constraints:Problem:NoSampleTestCases:Min"] ?? "0"),
               int.Parse(config["Constraints:Problem:NoSampleTestCases:Max"] ?? "0")
            );
            constraints.Problem.TimeLimitMS = new Range(
               int.Parse(config["Constraints:Problem:TimeLimitMS:Min"] ?? "0"),
               int.Parse(config["Constraints:Problem:TimeLimitMS:Max"] ?? "0")
            );
            #endregion

            #region TestCase
            constraints.TestCase.General.InputLength = new Range(
                int.Parse(config["Constraints:TestCase:General:Input:MinLength"] ?? "0"),
                int.Parse(config["Constraints:TestCase:General:Input:MaxLength"] ?? "0")
            );
            constraints.TestCase.General.OutputLength = new Range(
                int.Parse(config["Constraints:TestCase:General:Output:MinLength"] ?? "0"),
                int.Parse(config["Constraints:TestCase:General:Output:MaxLength"] ?? "0")
            );
            constraints.TestCase.Sample.InputLength = new Range(
                int.Parse(config["Constraints:TestCase:Sample:Input:MinLength"] ?? "0"),
                int.Parse(config["Constraints:TestCase:Sample:Input:MaxLength"] ?? "0")
            );
            constraints.TestCase.Sample.OutputLength = new Range(
                int.Parse(config["Constraints:TestCase:Sample:Output:MinLength"] ?? "0"),
                int.Parse(config["Constraints:TestCase:Sample:Output:MaxLength"] ?? "0")
            );
            constraints.TestCase.Sample.InputNoLines = new Range(
                int.Parse(config["Constraints:TestCase:Sample:Input:MinNoLines"] ?? "0"),
                int.Parse(config["Constraints:TestCase:Sample:Input:MaxNoLines"] ?? "0")
            );
            constraints.TestCase.Sample.OutputNoLines = new Range(
                int.Parse(config["Constraints:TestCase:Sample:Output:MinNoLines"] ?? "0"),
                int.Parse(config["Constraints:TestCase:Sample:Output:MaxNoLines"] ?? "0")
            );
            #endregion

            #region Tag
            constraints.Tag.NameLength = new Range(
                int.Parse(config["Constraints:Tag:Name:MinLength"] ?? "0"),
                int.Parse(config["Constraints:Tag:Name:MaxLength"] ?? "0")
            );
            #endregion

            #region User
            constraints.User.UsernameLength = new Range(
                int.Parse(config["Constraints:User:Username:MinLength"] ?? "0"),
                int.Parse(config["Constraints:User:Username:MaxLength"] ?? "0")
            );
            constraints.User.PasswordLength = new Range(
                int.Parse(config["Constraints:User:Password:MinLength"] ?? "0"),
                int.Parse(config["Constraints:User:Password:MaxLength"] ?? "0")
            );
            #endregion

            #region Submission 
            constraints.Submission.CodeLength = new Range(
                int.Parse(config["Constraints:Submission:Code:MinLength"] ?? "0"),
                int.Parse(config["Constraints:Submission:Code:MaxLength"] ?? "0")
            );
            #endregion

            return constraints;
        }
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
            builder.Services.AddScoped<ProblemValidation>();

            ConstraintsOption constraintsOption = GetConstraintsFromConfiguration(builder.Configuration);
            builder.Services.AddSingleton(constraintsOption);

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
            .AddJsonOptions(options => {
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

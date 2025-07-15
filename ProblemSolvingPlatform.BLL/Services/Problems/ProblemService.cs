using Microsoft.AspNetCore.Http.HttpResults;
using ProblemSolvingPlatform.API.DTOs;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.BLL.DTOs.TestCases;
using ProblemSolvingPlatform.BLL.DTOs.UserProfile;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Options.Constraint;
using ProblemSolvingPlatform.BLL.Services.Compiler;
using ProblemSolvingPlatform.BLL.Services.Users;
using ProblemSolvingPlatform.BLL.Validation;
using ProblemSolvingPlatform.BLL.Validation.Problem;
using ProblemSolvingPlatform.DAL.Models.Problems;
using ProblemSolvingPlatform.DAL.Models.Users;
using ProblemSolvingPlatform.DAL.Repos.Problems;
using ProblemSolvingPlatform.DAL.Repos.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.BLL.DTOs.Enums;

namespace ProblemSolvingPlatform.BLL.Services.Problems {
    public class ProblemService : IProblemService {

        private readonly IProblemRepo _problemRepo;
        private readonly IUserService _userService;
        private readonly ICompilerService _compilerService;
        private readonly ProblemValidation _problemValidation;
        private readonly ConstraintsOption _constraintsOptions;

        public ProblemService(IProblemRepo problemRepo, ICompilerService compilerService, ProblemValidation problemValidation, IUserService userService, ConstraintsOption constraintsOptions) {
            _problemRepo = problemRepo;
            _compilerService = compilerService;
            _problemValidation = problemValidation;
            _userService = userService;
            _constraintsOptions = constraintsOptions;
        }

        public async Task<int?> AddProblemAsync(NewProblemDTO newProblem, int createdBy) {
            Dictionary<string, List<string>> errors = _problemValidation.ValidateNewProblem(newProblem);
            if (errors == null) errors = new();

            if (!errors.Keys.Contains("UserID")) errors["UserID"] = [];

            if (!await _userService.DoesUserExistByIDAsync(createdBy)) {
                errors["UserID"].Add($"The user with id = {createdBy} was not found");
            }

            CompileRequestDTO compileRequestDTO = new CompileRequestDTO() {
                Inputs = newProblem.TestCases.Select(tc => tc.Input).ToList(),
                Source = newProblem.SolutionCode,
                Compiler = newProblem.CompilerName,
                TimeoutMs = newProblem.TimeLimitMilliseconds
            };


            List<CompileResponseDTO> compileReponsesDTO = await _compilerService.CompileAsync(compileRequestDTO);

            if (compileReponsesDTO.Any(res => !res.CompilationSuccess ||
                                    !res.ExecutionSuccess ||
                                    res.Timeout ||
                                    string.IsNullOrEmpty(res.Output))
                ) {
                for (int i = 0; i < compileReponsesDTO.Count; i++) {
                    if (!errors.Keys.Contains($"TestCases[{i}]"))
                        errors[$"TestCases[{i}]"] = [];
                    if (!errors.Keys.Contains($"SolutionCode_ForTestCase[{i}]"))
                        errors[$"SolutionCode_ForTestCase[{i}]"] = [];

                    if (!compileReponsesDTO[i].CompilationSuccess) {
                        errors[$"SolutionCode_ForTestCase[{i}]"].Add(string.Join("\n", compileReponsesDTO[i].CompilationErrors ?? []));
                        break;
                    }
                    else if (!compileReponsesDTO[i].ExecutionSuccess) {
                        errors[$"TestCases[{i}]"].Add(string.Join("\n", compileReponsesDTO[i].ExecutionErrors ?? []));
                    }
                    else if (compileReponsesDTO[i].Timeout) {
                        errors[$"TestCases[{i}]"].Add($"Timeout: The time limit of problem is {newProblem.TimeLimitMilliseconds} but this test case took {compileReponsesDTO[i].ExecutionTimeMs}ms");
                    }
                    else if (string.IsNullOrEmpty(compileReponsesDTO[i].Output)) {
                        errors[$"TestCases[{i}]"].Add($"The output of this test case is null or empty.");
                    }
                }
            }

            errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
            if (errors.Count > 0) throw new CustomValidationException(errors);

            // Convert NewProblemDTO to NewProblemModel
            NewProblemModel model = new NewProblemModel() {
                CompilerName = newProblem.CompilerName,
                CreatedBy = createdBy,
                Title = newProblem.Title,
                GeneralDescription = newProblem.GeneralDescription,
                InputDescription = newProblem.InputDescription,
                OutputDescription = newProblem.OutputDescription,
                Note = newProblem.Note,
                Tutorial = newProblem.Tutorial,
                Difficulty = (DAL.Models.Enums.Difficulty)(int)newProblem.Difficulty,
                SolutionCode = newProblem.SolutionCode,
                TimeLimitMilliseconds = newProblem.TimeLimitMilliseconds,
                TestCases = newProblem.TestCases.Select((t, i) => new DAL.Models.TestCases.NewTestCaseModel() {
                    Input = t.Input,
                    Output = compileReponsesDTO[i].Output ?? "",
                    IsPublic = t.IsPublic,
                    IsSample = t.IsSample
                }).ToList(),
                TagIDs = newProblem.TagIDs.ToList(),
            };
            return await _problemRepo.AddProblemAsync(model);
        }

        public async Task<bool> DeleteProblemByIDAsync(int problemID, int deletedBy) {
            Dictionary<string, List<string>> errors = new();

            errors["ProblemID"] = [];
            errors["UserID"] = [];

            if (!await _userService.DoesUserExistByIDAsync(deletedBy))
                errors["UserID"].Add($"The user with id = {deletedBy} was not found");

            if (!await _problemRepo.DoesProblemExistByIDAsync(problemID))
                errors["ProblemID"].Add($"The problem with id = {problemID} was not found");
            else {

                ProblemModel? existingProblem = await _problemRepo.GetProblemByIDAsync(problemID);
                UserDTO? existingUser = await _userService.GetUserByIdAsync(deletedBy);
                if (existingProblem == null) throw new Exception("Some errors occurred");
                if (existingUser == null) throw new Exception("Some errors occurred");

                if (existingUser.Role == Role.User && existingProblem.CreatedBy != deletedBy)
                    errors["UserID"].Add($"The problem with id = {problemID} is not belong to user with id = {deletedBy}");
            }

            errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
            if (errors.Count > 0) throw new CustomValidationException(errors);

            return await _problemRepo.DeleteProblemByIDAsync(problemID, deletedBy);
        }


        public async Task<bool> UpdateProblemAsync(UpdateProblemDTO updateProblem, int userID) {
            Dictionary<string, List<string>> errors = _problemValidation.ValidateUpdateProblem(updateProblem);
            if (errors == null) errors = new();

            if (!errors.Keys.Contains("ProblemID")) errors["ProblemID"] = [];
            if (!errors.Keys.Contains("UserID")) errors["UserID"] = [];

            if (!await _userService.DoesUserExistByIDAsync(userID))
                errors["UserID"].Add($"The user with id = {userID} was not found");

            if (!await _problemRepo.DoesProblemExistByIDAsync(updateProblem.ProblemID))
                errors["ProblemID"].Add($"The problem with id = {updateProblem.ProblemID} was not found");
            else {
                ProblemModel? existingProblem = await _problemRepo.GetProblemByIDAsync(updateProblem.ProblemID);
                UserDTO? existingUser = await _userService.GetUserByIdAsync(userID);
                if (existingProblem == null) throw new Exception("Some errors occurred");
                if (existingUser == null) throw new Exception("Some errors occurred");

                if (existingUser.Role == Role.User && existingProblem.CreatedBy != userID) {
                    errors["UserID"].Add($"The problem with id = {updateProblem.ProblemID} is not belong to user with id = {userID}");
                }
            }

            errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
            if (errors.Count > 0) throw new CustomValidationException(errors);

            // Convert UpdateProblemDTO to UpdateProblemModel
            UpdateProblemModel model = new UpdateProblemModel() {
                ProblemID = updateProblem.ProblemID,
                Title = updateProblem.Title,
                GeneralDescription = updateProblem.GeneralDescription,
                InputDescription = updateProblem.InputDescription,
                OutputDescription = updateProblem.OutputDescription,
                Note = updateProblem.Note,
                Tutorial = updateProblem.Tutorial,
                Difficulty = (DAL.Models.Enums.Difficulty)(byte)updateProblem.Difficulty,
            };
            return await _problemRepo.UpdateProblemAsync(model);
        }


        public async Task<IEnumerable<ShortProblemDTO>?> GetAllProblemsAsync(int page, int limit, string? title = null, byte? difficulty = null, int? createdBy = null, byte? role = null, DateTime? createdAt = null, IEnumerable<int>? tagIDs = null) {
            Dictionary<string, List<string>> errors = new();
            errors["Page"] = [];
            errors["Limit"] = [];

            if (page < _constraintsOptions.MinPageNumber)
                errors["Page"].Add($"The page must to be greater than {_constraintsOptions.MinPageNumber}");

            if (limit < _constraintsOptions.PageSize.Start.Value || limit > _constraintsOptions.PageSize.End.Value)
                errors["Limit"].Add($"The limit must to be in range [{_constraintsOptions.PageSize.Start.Value},{_constraintsOptions.PageSize.End.Value}]");

            errors = errors.Where(kp => kp.Value.Count > 0).ToDictionary();
            if (errors.Count > 0) throw new CustomValidationException(errors);


            return (await _problemRepo.GetAllProblemsAsync(page, limit, title, difficulty, createdBy,role, createdAt, tagIDs))
                ?.Select(model => new ShortProblemDTO() {
                    ProblemID = model.ProblemID,
                    Difficulty = (Difficulty)(int)model.Difficulty,
                    Title = model.Title,
                    GeneralDescription = model.GeneralDescription,
                    SolutionsCount = model.SolutionsCount,
                    AttemptsCount = model.AttemptsCount,
                    Tags = model.Tags.Select(t => new TagDTO() { Name = t.Name, TagID = t.TagID }),
                });
        }

        public async Task<ProblemDTO?> GetProblemByIDAsync(int problemID) {
            if (!await _problemRepo.DoesProblemExistByIDAsync(problemID)) throw new CustomValidationException("ProblemID", [$"The problem with id = {problemID} was not found"]);

            var problemModel = await _problemRepo.GetProblemByIDAsync(problemID);
            if (problemModel == null) return null;

            var problemDTO = new ProblemDTO() {
                ProblemID = problemModel.ProblemID,
                CompilerName = problemModel.CompilerName,
                CreatedBy = problemModel.CreatedBy,
                CreatedAt = problemModel.CreatedAt,
                DeletedBy = problemModel.DeletedBy,
                DeletedAt = problemModel.DeletedAt,
                Title = problemModel.Title,
                GeneralDescription = problemModel.GeneralDescription,
                InputDescription = problemModel.InputDescription,
                OutputDescription = problemModel.OutputDescription,
                Note = problemModel.Note,
                Tutorial = problemModel.Tutorial,
                Difficulty = (Enums.Difficulty)(int)problemModel.Difficulty,
                SolutionCode = problemModel.SolutionCode,
                TimeLimitMilliseconds = problemModel.TimeLimitMilliseconds,

                SampleTestCases = problemModel.SampleTestCases.Select(obj => new TestCaseDTO() {
                    TestCaseID = obj.TestCaseID,
                    ProblemID = obj.ProblemID,
                    Input = obj.Input,
                    Output = obj.Output,
                    IsPublic = obj.IsPublic,
                    IsSample = obj.IsSample
                }).ToList(),

                Tags = problemModel.Tags.Select(obj => new TagDTO() {
                    TagID = obj.TagID,
                    Name = obj.Name
                }).ToList()
            };


            return problemDTO;
        }
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using ProblemSolvingPlatform.API.DTOs;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
using ProblemSolvingPlatform.BLL.DTOs.Tags;
using ProblemSolvingPlatform.BLL.DTOs.TestCases;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Services.Compiler;
using ProblemSolvingPlatform.BLL.Validation;
using ProblemSolvingPlatform.DAL.Models.Problems;
using ProblemSolvingPlatform.DAL.Repos.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Problems {
    public class ProblemService : IProblemService {

        private readonly IProblemRepo _problemRepo;
        private readonly ICompilerService _compilerService;

        public ProblemService(IProblemRepo problemRepo,ICompilerService compilerService) {
            _problemRepo = problemRepo;
            _compilerService = compilerService;
        }

        public async Task<int?> AddProblemAsync(NewProblemDTO newProblem,int createdBy) {
            var errors = ValidationHelper.ValidateToDictionary(newProblem, out bool isValid);
            if(!isValid) 
                throw new CustomValidationException(errors);


            CompileRequestDTO compileRequestDTO = new CompileRequestDTO() {
                Inputs = newProblem.TestCases.Select(tc =>  tc.Input).ToList(),
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
                errors = new Dictionary<string, List<string>>();
                for(int i=0;i<compileReponsesDTO.Count;i++) {
                    if(!compileReponsesDTO[i].CompilationSuccess) {
                        errors[$"TestCases[{i}]"] = [string.Join("\n",compileReponsesDTO[i].CompilationErrors??[])];
                    } else if (!compileReponsesDTO[i].ExecutionSuccess) {
                        errors[$"TestCases[{i}]"] = [string.Join("\n", compileReponsesDTO[i].ExecutionErrors ?? [])];
                    } else if (compileReponsesDTO[i].Timeout) {
                        errors[$"TestCases[{i}]"] = [$"Timeout: The time limit of problem is {newProblem.TimeLimitMilliseconds} but this test case took {compileReponsesDTO[i].ExecutionTimeMs}ms"];
                    } else if (string.IsNullOrEmpty(compileReponsesDTO[i].Output)) {
                        errors[$"TestCases[{i}]"] = [$"The output of this test case is null or empty."];
                    }
                }

                throw new CustomValidationException(errors);
            } 

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
                TestCases = newProblem.TestCases.Select((t,i) => new DAL.Models.TestCases.NewTestCaseModel() {
                    Input = t.Input,
                    Output = compileReponsesDTO[i].Output??"", 
                    IsPublic = t.IsPublic,
                    IsSample = t.IsSample
                }).ToList(),
                TagIDs = newProblem.TagIDs.ToList(),
            };
            return await _problemRepo.AddProblemAsync(model);
        }

        public async Task<bool> DeleteProblemByIDAsync(int problemID,int deletedBy) {
            return await _problemRepo.DeleteProblemByIDAsync(problemID, deletedBy); 
        }

        public async Task<ProblemDTO?> GetProblemByIDAsync(int problemID) {
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

                Tags = problemModel.Tags.Select (obj => new TagDTO() { 
                    TagID = obj.TagID,
                    Name = obj.Name
                }).ToList()
            };


            return problemDTO;
        }

        public async Task<bool> UpdateProblemAsync(UpdateProblemDTO updateProblem, int userID) {
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
    }
}

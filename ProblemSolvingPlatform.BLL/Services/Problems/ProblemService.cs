using ProblemSolvingPlatform.API.Compiler.DTOs;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Problems;
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

        public async Task<int?> AddProblemAsync(NewProblemDTO newProblem) {
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
                CreatedBy = newProblem.CreatedBy,
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
    }
}

using ProblemSolvingPlatform.API.Compiler.DTOs;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.BLL.DTOs.Problem;
using ProblemSolvingPlatform.BLL.Exceptions;
using ProblemSolvingPlatform.BLL.Services.Compiler;
using ProblemSolvingPlatform.BLL.Validation;
using ProblemSolvingPlatform.DAL.Models.Problem;
using ProblemSolvingPlatform.DAL.Repos.Problem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolvingPlatform.BLL.Services.Problem {
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
                inputs = newProblem.TestCases.Select(tc =>  tc.Input).ToList(),
                source = newProblem.SolutionCode,
                compiler = newProblem.CompilerID
            };


            List<CompileResponseDTO> compileReponsesDTO = await _compilerService.CompileAsync(compileRequestDTO);

            if (compileReponsesDTO.Any(res => res.standardError != null)) {
                errors = new Dictionary<string, List<string>>();
                for(int i=0;i<compileReponsesDTO.Count;i++) {
                    if(compileReponsesDTO[i].standardError != null) {
                        errors[$"TestCases[{i}]"] = [compileReponsesDTO[i].standardError];
                    }
                }

                throw new CustomValidationException(errors);
            } 

            // Convert NewProblemDTO to NewProblemModel
            NewProblemModel model = new NewProblemModel() {
                CompilerID = newProblem.CompilerID,
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
                TestCases = newProblem.TestCases.Select((t,i) => new DAL.Models.TestCase.NewTestCaseModel() {
                    Input = t.Input,
                    Output = compileReponsesDTO[i].standardOut, 
                    IsPublic = t.IsPublic,
                    IsSample = t.IsSample
                }).ToList(),
                TagIDs = newProblem.TagIDs
            };
            return await _problemRepo.AddProblemAsync(model);
        }
    }
}

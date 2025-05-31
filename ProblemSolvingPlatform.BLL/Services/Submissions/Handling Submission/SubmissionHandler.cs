using ProblemSolvingPlatform.API.DTOs;
using ProblemSolvingPlatform.API.Compiler.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.API.Compiler.Services.CompilerApiService;

namespace ProblemSolvingPlatform.BLL.Services.Submissions.Handling_Submission;

public class SubmissionHandler
{
    private ICompilerApiService _compilerApiService { get; }
    public SubmissionHandler(ICompilerApiService compilerApiService)
    {
        _compilerApiService = compilerApiService;
    }


    public async Task<string> ExecuteSubmission(int submissionId, string code)
    {
        // this is for testing only :) 
        // soon will be implemented it successfully :)))




        //List<string> ins = new List<string>() { "1 2 3 ", "2", "3", "4", "5" };


        //ExecuteRequestDTO compileRequestDTO = new()
        //{
        //    Source = "#include<iostream>\nusing namespace std;\nint main() {\n  int n,a,b;\n  cin >> n >> a >> b;\n  cout << n * -1 << endl;\n  return 0;\n}" ,
        //    input = ins[0],
        //    Compiler = CompilerApiService.Compilers.Cpp.g132
        //};
        //var result = await _compilerApiService.ExecuteCodeAsync(compileRequestDTO);
        //if (!result.Success)
        //{
        //    return result.StandardError + " |||||| ";
        //}    
        //return result.StandardOut + " |||| " + result.ExecutionTimeMs + "  || MEORY " + result.MemoryKB;
        return "";
    }

}

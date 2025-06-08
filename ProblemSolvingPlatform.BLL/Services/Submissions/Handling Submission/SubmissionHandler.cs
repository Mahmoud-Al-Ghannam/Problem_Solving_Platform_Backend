using ProblemSolvingPlatform.API.DTOs;
using ProblemSolvingPlatform.API.Compiler.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProblemSolvingPlatform.API.Compiler.Services.CompilerApiService;
using ProblemSolvingPlatform.DAL.Repos.Problems;
using ProblemSolvingPlatform.DAL.Repos.Tests;
using Microsoft.Identity.Client;
using ProblemSolvingPlatform.DAL.Repos;
using ProblemSolvingPlatform.BLL.DTOs;
using ProblemSolvingPlatform.DAL.Repos.Submissions;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;

namespace ProblemSolvingPlatform.BLL.Services.Submissions.Handling_Submission;

public class SubmissionHandler
{
    private ICompilerApiService _compilerApiService { get; }
    private ITestCaseRepo _testCaseRepo { get; }
    private IProblemRepo _problemRepo { get; }
    private ISubmissionTestRepo _submissionTestRepo { get; }
    private ISubmissionRepo _submissionRepo { get; }
    public SubmissionHandler(ICompilerApiService compilerApiService, 
        IProblemRepo problemRepo, ITestCaseRepo testCaseRepo,
        ISubmissionTestRepo submissionTestRepo, ISubmissionRepo submissionRepo)
    {
        _compilerApiService = compilerApiService;
        _testCaseRepo = testCaseRepo; 
        _problemRepo = problemRepo;
        _submissionTestRepo = submissionTestRepo;
        _submissionRepo = submissionRepo;
    }



    public async Task<string> ExecuteSubmission(int submissionId, string code, int problemId, string CompilerName)
    {
        var testCases = await _testCaseRepo.GetTestCasesByProblemIdAsync(problemId);
        var problem = await _problemRepo.GetProblemByIDAsync(problemId);
        if (testCases == null) return "There is an error";

        int mxExecutionTimeMS = 0;

        foreach (var test in testCases)
        {
            List<string> inputs = new(test.Input.Split(' '));
            string ExpectedOutputs = test.Output;

            List<CompileResponseDTO> compileResults;
            try
            {
                compileResults = await _compilerApiService.CompileAsync(new CompileRequestDTO()
                {
                    Compiler = CompilerName,
                    Inputs = inputs,
                    Source = code
                });
            }
            catch { return "There is an error"; }


            foreach(var ResponseTestCase in compileResults)
            {
                int executionTime = ResponseTestCase.ExecutionTimeMs != null ? Convert.ToInt32(ResponseTestCase.ExecutionTimeMs) : 0;
                mxExecutionTimeMS = Math.Max(mxExecutionTimeMS, executionTime);


                if (!isCompilationSuccess(ResponseTestCase))
                {
                    await AddExecutionTestCase(submissionId, test.TestCaseID, executionTime, (byte)Enums.SubmissionStatus.CompilationError);
                    await _submissionRepo.UpdateSubmissionStatusAndExecTime(submissionId, (byte)Enums.SubmissionStatus.CompilationError, mxExecutionTimeMS);
                    return $"Compilation Error  :) ";
                }
                else if (isRunTimeError(ResponseTestCase))
                {
                    await AddExecutionTestCase(submissionId, test.TestCaseID, executionTime, (byte)Enums.SubmissionStatus.RunTimeError);
                    await _submissionRepo.UpdateSubmissionStatusAndExecTime(submissionId, (byte)Enums.SubmissionStatus.RunTimeError, mxExecutionTimeMS);
                    return $"RunTime Error Error on test {test.TestCaseID} :) ";
                }
                else if(isTimeLimitExceeded(ResponseTestCase, problem.TimeLimitMilliseconds))
                {
                    await AddExecutionTestCase(submissionId, test.TestCaseID, executionTime, (byte)Enums.SubmissionStatus.TimeLimitExceeded);
                    await _submissionRepo.UpdateSubmissionStatusAndExecTime(submissionId, (byte)Enums.SubmissionStatus.TimeLimitExceeded, mxExecutionTimeMS);
                    return $"Time Limit Exceeded on test {test.TestCaseID} :) ";
                }
                else
                {
                    if(isAccepted(ResponseTestCase.Output, ExpectedOutputs))
                    {
                        await AddExecutionTestCase(submissionId, test.TestCaseID, executionTime, (byte)Enums.SubmissionStatus.Accepted);
                    }
                    else
                    {
                        await AddExecutionTestCase(submissionId, test.TestCaseID, executionTime, (byte)Enums.SubmissionStatus.WrongAnswer);
                        await _submissionRepo.UpdateSubmissionStatusAndExecTime(submissionId, (byte)Enums.SubmissionStatus.WrongAnswer, mxExecutionTimeMS);
                        return $"Wrong Answer  on test {test.TestCaseID} :) ";
                    }
                }                   
            }
        }

        await _submissionRepo.UpdateSubmissionStatusAndExecTime(submissionId, (byte)Enums.SubmissionStatus.Accepted, mxExecutionTimeMS);
        return $"Accepted :|";
    }



    private async Task AddExecutionTestCase(int submissionId, int testCaseID, int executionTimeMS, byte status)
    {
        await _submissionTestRepo.AddNewSubmissionTestCase(
                        new DAL.Models.SubmissionTestCase()
                        {
                            ExecutionTimeMilliseconds = executionTimeMS,
                            SubmissionID = submissionId,
                            TestCaseID = testCaseID,
                            Status = status
                        });
    }


    private bool isRunTimeError(CompileResponseDTO response)
        => response.ExecutionSuccess == false;
    
    private bool isCompilationSuccess(CompileResponseDTO response)
        => response.CompilationSuccess;
    
    private bool isTimeLimitExceeded(CompileResponseDTO response, int TimeLimitMS) 
        => response.ExecutionTimeMs > TimeLimitMS;
 
    private bool isAccepted(string? output, string? expectedOutput)
    {
        if (output == null && expectedOutput == null) return true;
        else if (output == null) return false;

        return output.Trim().Equals(expectedOutput.Trim(), StringComparison.Ordinal);
    }

}

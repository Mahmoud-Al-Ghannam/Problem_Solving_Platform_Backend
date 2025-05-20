using Microsoft.Extensions.Configuration;
using ProblemSolvingPlatform.API.Base;
using ProblemSolvingPlatform.API.Compiler.DTOs;
using ProblemSolvingPlatform.API.Compiler.Services;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models.Problem;
using ProblemSolvingPlatform.DAL.Models.TestCase;
using ProblemSolvingPlatform.DAL.Repos.Problem;

namespace TestingConsoleApplication {
    public class Program {

        private DbContext dbContext;
        Program (DbContext dbContext) {
            this.dbContext = dbContext;
        }

        static void Main(string[] args) {

            ///*
            //public string CompilerID {  get; set; }
            //public int CreatedBy {  get; set; }
            //public string Title { get; set; }
            //public string GeneralDescription { get; set; }
            //public string InputDescription { get; set; }
            //public string OutputDescription { get; set; }
            //public string Note { get; set; }
            //public string Tutorial { get; set; }
            //public Difficulty Difficulty { get; set; }
            //public string SolutionCode { get; set; }
            //public int TimeLimitMilliseconds { get; set; }
            //public List<NewTestCaseModel> TestCases { get; set; }
            //public List<int> TagIDs { get; set; }
            //*/
            //NewProblemModel problem = new NewProblemModel() {
            //    CompilerID = "cc",
            //    CreatedBy = 1,
            //    Title = "Test",
            //    GeneralDescription = "gdTest",
            //    InputDescription = "iTest",
            //    OutputDescription = "oTest",
            //    Note = "noteTest",
            //    Tutorial = "tutTest",
            //    Difficulty = ProblemSolvingPlatform.DAL.Models.Enums.Difficulty.eHard,
            //    SolutionCode = "codeTest",
            //    TimeLimitMilliseconds = 1000,
            //    TestCases = new List<NewTestCaseModel> () {

            //    },
            //    TagIDs = new List<int> () {

            //    }
            //};

            //ProblemRepo repo = new ProblemRepo(dbContext);
            //int? id = repo.AddProblemAsync(problem).Result;
            //Console.WriteLine(id);
        }
    }
}

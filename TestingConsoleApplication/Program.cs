using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProblemSolvingPlatform.API.Base;
using ProblemSolvingPlatform.API.Compiler.DTOs;
using ProblemSolvingPlatform.API.Compiler.Services;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models.Problem;
using ProblemSolvingPlatform.DAL.Models.TestCase;
using ProblemSolvingPlatform.DAL.Repos.Problem;
using static ProblemSolvingPlatform.API.Compiler.Services.CompilerApiService;

namespace TestingConsoleApplication {
    public class Program {

        private DbContext dbContext;
        Program (DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        static void Main(string[] args)
        {
            dynamic stuff = JsonConvert.DeserializeObject("{ 'Name': ['Jon Smith'], 'Address': { 'City': 'New York', 'State': 'NY' }, 'Age': 42 }");


            var name = stuff.Name;
            string address = stuff.Address.City;

            //CompilerApiService api = new CompilerApiService();
            //var res = api.CompileAsync(new CompileRequestDTO() {
            //    Source = @"
            //#include<iostream>
            //using namespace std;
            //int main() {

            //    int x;
            //    cin >> x;
            //    cout << x * x << endl;
            //    return 0;
            //}
            //",
            //    Compiler = "clang1810",
            //    Inputs = ["10"]
            //}).Result;


            //Console.WriteLine(res[0].standardOut);
            //Console.WriteLine(res[0].standardError);


            //var res = api.ExecuteCodeAsync(new ExecuteRequestDTO() {
            //    Source = @"
            //#include<iostream>
            //using namespace std;
            //int main() {

            //    int x;
            //    cin >> x;
            //    cout << x * x << endl;
            //    return 0;
            //}
            //",
            //    Compiler = "clang1810",
            //    input = "10"
            //}).Result;


            //Console.WriteLine(res.StandardOut);
            //Console.WriteLine(res.StandardError);
            //Console.WriteLine(res.MemoryKB);
            //Console.WriteLine(res.ExecutionTimeMs);
            //Console.WriteLine(res.ExitCode);

            return;
        
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

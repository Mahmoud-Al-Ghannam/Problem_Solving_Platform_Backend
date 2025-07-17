using AutoMapper.Configuration.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using ProblemSolvingPlatform.API.Base;
using ProblemSolvingPlatform.API.Compiler.Services;
using ProblemSolvingPlatform.DAL.Context;
using ProblemSolvingPlatform.DAL.Models.Problems;
using ProblemSolvingPlatform.DAL.Models.TestCases;
using ProblemSolvingPlatform.DAL.Repos.Problems;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using static ProblemSolvingPlatform.API.Compiler.Services.CompilerApiService;

namespace TestingConsoleApplication {
    public class Program {

        
        static void Main(string[] args) {
            Console.WriteLine("\u001b[1m<source>:2:22: \u001b[0m\u001b[0;1;31merror: \u001b[0m\u001b[1munknown type name 'nt'\u001b[0m\n    2 |  using namespace std;nt main() {int a,b;cin >> a >> b;cout << a+b<< endl;return 0;}\u001b[0m\n      | \u001b[0;1;32m                     ^\n\u001b[0m1 error generated.");

        }
    }
}

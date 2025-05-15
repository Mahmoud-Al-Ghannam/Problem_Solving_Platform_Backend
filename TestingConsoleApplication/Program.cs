using ProblemSolvingPlatform.API.Base;
using ProblemSolvingPlatform.API.Compiler.DTOs;
using ProblemSolvingPlatform.API.Compiler.Services;

namespace TestingConsoleApplication {
    internal class Program {
        static void Main(string[] args) {
            string source = "";
            while (true) {

                string input = Console.ReadLine();
                if (input == ".") {
                    break;
                }
                source += input + "\n";
            }

            Console.WriteLine("\n\n");
            var compiler = new CompilerApiService(new HttpClient() { BaseAddress = new Uri(CompilerApiService.BaseAddress)});

            try {
                var res = compiler.CompileAsync(new CompileRequestDTO() { source = source }).Result;

                Console.WriteLine("Output:");
                Console.WriteLine(res.standardOut);
                Console.WriteLine("Error:");
                Console.WriteLine(res.standardError);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}

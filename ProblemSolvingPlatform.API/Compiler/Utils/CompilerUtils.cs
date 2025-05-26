using ProblemSolvingPlatform.API.Compiler.DTOs;


namespace ProblemSolvingPlatform.API.Compiler.Utils;

public static class CompilerUtils
{
    public static readonly Dictionary<string, CompilerDTO> Compilers = new()
    {
        { "clang1810", new CompilerDTO { Language = "Cpp", CompilerName = "clang1810", Name = "x86-64 clang 18.1.0" } },
        { "g132", new CompilerDTO { Language = "Cpp", CompilerName = "g132", Name = "x86-64 gcc 13.2" } },
        { "cclang1810", new CompilerDTO { Language = "C", CompilerName = "cclang1810", Name = "x86-64 clang 18.1.0" } },
        { "cg132", new CompilerDTO { Language = "C", CompilerName = "cg132", Name = "x86-64 gcc 13.2" } },
        { "python312", new CompilerDTO { Language = "Python", CompilerName = "python312", Name = "Python 3.12" } },
        { "java2102", new CompilerDTO { Language = "Java", CompilerName = "java2102", Name = "jdk 21.0.2" } },
    };

    public static CompilerDTO? GetCompiler(string compilerName)
    {
        var f = Compilers.TryGetValue(compilerName, out var compiler);
        if (f) return compiler;
        return null; 
    }

    // Get all compilers
    public static List<CompilerDTO> GetAllCompilers()
    {
        return Compilers.Values.ToList();
    }


}

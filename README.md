### Problem Solving System

A comprehensive backend system built with ASP.NET Core for managing programming problems, user solutions, and code execution. This platform allows users to solve coding challenges, track their progress, and view detailed statistics.

##✨ Key Features

· User Profile Management: Users can update their information and track their progress
· Problem Management: Create, edit, and delete coding problems with tags and difficulty levels
· Solution System: Submit solutions to problems and view detailed results
· Test Case System: Each problem contains test cases to evaluate solutions
· Statistics Dashboard: View detailed statistics about users, solutions, and problems
· Role-Based Authentication: Multiple roles (System & User) using JWT
· Multi-Language Support: Integration with external code execution system (C++, Python, Java, etc.)
· Efficient Data Handling: Pagination implementation for large datasets

##🛠 Technology Stack

· Backend: ASP.NET Core 8.0+
· Authentication: JWT (JSON Web Tokens)
· Database: SQL Server
· Code Execution: Integration with external code execution service
· Package Management: NuGet

##📦 Installation & Setup

#Prerequisites

· .NET 8.0 SDK or later
· SQL Server
· Code editor like Visual Studio or VS Code

#Setup Steps

1. Clone the repository:
   . git clone https://github.com/your-username/your-repo-name.git
   cd your-repo-name
   
2. Install required packages:
   . dotnet restore
   
3. Database setup:
   · Update connection string in appsettings.json
   . Execute GeneratingScript_ProblemSolvingPlatformDB.sql file on SQL Server Management Studio to initialize database
   
5. Run the application:
   . dotnet run
   
6. Access the application:
  
   https://localhost:7000 (or your configured port)
   
#⚙️ Configuration

appsettings.json Configuration

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=CodingProblemSystem;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyHere",
    "Issuer": "YourApp",
    "Audience": "YourAppUsers"
  },
}

##🚀 API Usage

Authentication and Token Management

1. Login:
  
   POST /api/auth/login
   Body: { "username": "user", "password": "pass" }
   
2. Use Token in subsequent requests:
  
   Authorization: Bearer {your_token}
   
#API Examples

· Get problems list:
 
  GET /api/problems?page=1&pageSize=10
  
· Add new problem (Admin only):
 
  POST /api/problems
  Body: { "title": "Problem Title", "description": "...", "difficulty": "Medium", "tags": ["Array", "Sorting"] }
  
· Submit solution:
 
  POST /api/solutions
  Body: { "problemId": 1, "code": "public class Solution {...}", "language": "Java" }
  
· Get user statistics:
 
  GET /api/users/{userId}/statistics
  
##📁 Project Structure

/
├── Controllers/
│   ├── AuthController.cs
│   ├── ProblemsController.cs
│   ├── SolutionsController.cs
│   ├── UsersController.cs
│   └── StatisticsController.cs
├── Models/
│   ├── User.cs
│   ├── Problem.cs
│   ├── Solution.cs
│   ├── Tag.cs
│   ├── TestCase.cs
│   └── ...
├── Services/
│   ├── AuthService.cs
│   ├── ProblemService.cs
│   ├── CompilerService.cs
│   └── ...
├── Data/
│   ├── ApplicationDbContext.cs
│   └── Migrations/
├── DTOs/
│   ├── LoginDto.cs
│   ├── ProblemDto.cs
│   └── ...
└── Helpers/
    ├── JwtMiddleware.cs
    └── PaginationHelper.cs
##🤝 Contributing

1. Fork the project
2. Create your feature branch (git checkout -b feature/AmazingFeature)
3. Commit your changes (git commit -m 'Add some AmazingFeature')
4. Push to the branch (git push origin feature/AmazingFeature)
5. Open a Pull Request

##📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

##📞 Contact

# Problem Solving System

A comprehensive backend system built with ASP.NET Core for managing programming problems, user solutions, and code execution. This platform allows users to solve coding challenges, track their progress, and view detailed statistics.

## ✨ Key Features

· **User Profile Management:** Users can update their information and track their progress

· **Problem Management:** Create, edit, and delete coding problems with tags and difficulty levels

· **Solution System:** Submit solutions to problems and view detailed results

· **Test Case System:** Each problem contains test cases to evaluate solutions

· **Statistics Dashboard:** View detailed statistics about users, solutions, and problems

· **Role-Based Authentication:** Multiple roles (System & User) using JWT

· **Multi-Language Support:** Integration with external code execution system (C++, Python, Java, etc.)

· **Efficient Data Handling:** Pagination implementation for large datasets


## 🛠 Technology Stack

· **Backend:** ASP.NET Core 8.0+

· **Authentication:** JWT (JSON Web Tokens)

· **Database:** SQL Server

· **Code Execution:** Integration with external code execution service

· **Package Management:** NuGet


## 📦 Installation & Setup

# Prerequisites

· .NET 8.0 SDK or later

· SQL Server

· Code editor like Visual Studio or VS Code


# Setup Steps

**1. Clone the repository:**

   . git clone https://github.com/your-username/your-repo-name.git
   cd your-repo-name
   
**2. Install required packages:**

   . dotnet restore
   
**3. Database setup:**

   · Update connection string in appsettings.json
   
   . Execute GeneratingScript_ProblemSolvingPlatformDB.sql file on SQL Server Management Studio to initialize database
   
**4. Run the application:**

   . dotnet run
   
**5. Access the application:**
  
   https://localhost:7000 (or your configured port)
   
# ⚙️ Configuration

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

## 🚀 API Usage

Authentication and Token Management

**1. Login:**
  
   POST /api/v1/auth/login

   Body: { "username": "user", "password": "pass" }
   

**2. Use Token in subsequent requests:**
  
   Authorization: Bearer {your_token}
   

# API Examples

· **Get problems list:**
 
  GET /api/v1/problems?page=1&limit=10
  
· **Add new problem:**
 
  POST /api/v1/problems
  
  Body: {
  "compilerName": "string",
  "title": "string",
  "generalDescription": "string",
  "inputDescription": "string",
  "outputDescription": "string",
  "note": "string",
  "tutorial": "string",
  "difficulty": "Easy",
  "solutionCode": "string",
  "timeLimitMilliseconds": 0,
  "testCases": [
    {
      "input": "string",
      "isPublic": true,
      "isSample": true
    }
  ],
  "tagIDs": [
    0
  ]
}
  
· **Submit solution:**
 
  POST /api/v1/submissions/submit
  
  Body: {
  "problemID": 0,
  "compilerName": "string",
  "code": "string",
  "visionScope": "onlyme"
}
  
· **Get user statistics:**
 
  GET /api/v1/statistics/users/{userId}
  

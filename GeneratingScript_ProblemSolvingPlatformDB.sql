USE [master]
GO
/****** Object:  Database [ProblemSolvingPlatformDB]    Script Date: 5/20/2025 10:55:31 PM ******/
CREATE DATABASE [ProblemSolvingPlatformDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ProblemSolvingPlatformDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ProblemSolvingPlatformDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ProblemSolvingPlatformDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ProblemSolvingPlatformDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ProblemSolvingPlatformDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET RECOVERY FULL 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET  MULTI_USER 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ProblemSolvingPlatformDB', N'ON'
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ProblemSolvingPlatformDB]
GO
/****** Object:  Table [dbo].[Problems]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Problems](
	[ProblemID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[DeletedBy] [int] NULL,
	[CompilerName] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[GeneralDescription] [nvarchar](max) NOT NULL,
	[InputDescription] [nvarchar](max) NOT NULL,
	[OutputDescription] [nvarchar](max) NOT NULL,
	[Note] [nvarchar](max) NULL,
	[Tutorial] [nvarchar](max) NULL,
	[Difficulty] [tinyint] NOT NULL,
	[SolutionCode] [nvarchar](max) NOT NULL,
	[TimeLimitMilliseconds] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[DeletedAt] [datetime] NULL,
 CONSTRAINT [PK_Problems] PRIMARY KEY CLUSTERED 
(
	[ProblemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProblemSubmissions]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProblemSubmissions](
	[ProblemSubmissionID] [int] IDENTITY(1,1) NOT NULL,
	[ProblemID] [int] NOT NULL,
	[SubmissionID] [int] NOT NULL,
 CONSTRAINT [PK_ProblemSubmissions] PRIMARY KEY CLUSTERED 
(
	[ProblemSubmissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProblemTags]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProblemTags](
	[ProblemTagID] [int] IDENTITY(1,1) NOT NULL,
	[ProblemID] [int] NOT NULL,
	[TagID] [int] NOT NULL,
 CONSTRAINT [PK_ProblemTags] PRIMARY KEY CLUSTERED 
(
	[ProblemTagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Submissions]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Submissions](
	[SubmissionID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ProgrammingLanguage] [tinyint] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[ExecutionTimeMilliseconds] [int] NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
	[VisionScope] [tinyint] NOT NULL,
	[SubType] [tinyint] NOT NULL,
	[SubmittedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Submissions] PRIMARY KEY CLUSTERED 
(
	[SubmissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubmissionTestCases]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubmissionTestCases](
	[SubmissionTestCaseID] [int] IDENTITY(1,1) NOT NULL,
	[TestCaseID] [int] NOT NULL,
	[SubmissionID] [int] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[ExecutionTimeMilliseconds] [int] NOT NULL,
 CONSTRAINT [PK_SubmissionTestCases] PRIMARY KEY CLUSTERED 
(
	[SubmissionTestCaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestCases]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestCases](
	[TestCaseID] [int] IDENTITY(1,1) NOT NULL,
	[ProblemID] [int] NOT NULL,
	[Input] [nvarchar](max) NOT NULL,
	[Output] [nvarchar](max) NOT NULL,
	[IsPublic] [bit] NOT NULL,
	[IsSample] [bit] NOT NULL,
 CONSTRAINT [PK_TestCases] PRIMARY KEY CLUSTERED 
(
	[TestCaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[ImagePath] [nvarchar](max) NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Role] [tinyint] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Problems] ON 

INSERT [dbo].[Problems] ([ProblemID], [CreatedBy], [DeletedBy], [CompilerName], [Title], [GeneralDescription], [InputDescription], [OutputDescription], [Note], [Tutorial], [Difficulty], [SolutionCode], [TimeLimitMilliseconds], [CreatedAt], [DeletedAt]) VALUES (7, 1, NULL, N'cc', N'Test', N'gdTest', N'iTest', N'oTest', N'noteTest', N'tutTest', 2, N'codeTest', 1000, CAST(N'2025-05-18T13:41:39.423' AS DateTime), NULL)
INSERT [dbo].[Problems] ([ProblemID], [CreatedBy], [DeletedBy], [CompilerName], [Title], [GeneralDescription], [InputDescription], [OutputDescription], [Note], [Tutorial], [Difficulty], [SolutionCode], [TimeLimitMilliseconds], [CreatedAt], [DeletedAt]) VALUES (9, 1, NULL, N'clang900', N't1', N'gd', N'id', N'od', N'n', N't', 0, N'#include <iostream> 
 using namespace std;int main() {int n;cin >> n;for (int i = 1;i <= n;i++) {if (n % i == 0)cout << i << " ";}cout << endl;return 0;}', 1000, CAST(N'2025-05-20T19:21:19.077' AS DateTime), NULL)
INSERT [dbo].[Problems] ([ProblemID], [CreatedBy], [DeletedBy], [CompilerName], [Title], [GeneralDescription], [InputDescription], [OutputDescription], [Note], [Tutorial], [Difficulty], [SolutionCode], [TimeLimitMilliseconds], [CreatedAt], [DeletedAt]) VALUES (10, 1, NULL, N'clang900', N't1', N'gd', N'id', N'od', N'n', N't', 0, N'#include <iostream> 
 using namespace std;int main() {int n;cin >> n;for (int i = 1;i <= n;i++) {if (n % i == 0)cout << i << " ";}cout << endl;return 0;}', 1000, CAST(N'2025-05-20T19:26:38.417' AS DateTime), NULL)
INSERT [dbo].[Problems] ([ProblemID], [CreatedBy], [DeletedBy], [CompilerName], [Title], [GeneralDescription], [InputDescription], [OutputDescription], [Note], [Tutorial], [Difficulty], [SolutionCode], [TimeLimitMilliseconds], [CreatedAt], [DeletedAt]) VALUES (11, 1, NULL, N'clang900', N't1', N'gd', N'id', N'od', N'n', N't', 0, N'#include <iostream> 
 using namespace std;int main() {int n;cin >> n;for (int i = 1;i <= n;i++) {if (n % i == 0)cout << i << " ";}cout << endl;return 0;}', 1000, CAST(N'2025-05-20T19:28:57.287' AS DateTime), NULL)
INSERT [dbo].[Problems] ([ProblemID], [CreatedBy], [DeletedBy], [CompilerName], [Title], [GeneralDescription], [InputDescription], [OutputDescription], [Note], [Tutorial], [Difficulty], [SolutionCode], [TimeLimitMilliseconds], [CreatedAt], [DeletedAt]) VALUES (12, 1, NULL, N'clang900', N't1', N'gd', N'id', N'od', N'n', N't', 0, N'#include <iostream> 
 using namespace std;int main() {int n;cin >> n;for (int i = 1;i <= n;i++) {if (n % i == 0)cout << i << " ";}cout << endl;return 0;}', 1000, CAST(N'2025-05-20T19:33:54.433' AS DateTime), NULL)
INSERT [dbo].[Problems] ([ProblemID], [CreatedBy], [DeletedBy], [CompilerName], [Title], [GeneralDescription], [InputDescription], [OutputDescription], [Note], [Tutorial], [Difficulty], [SolutionCode], [TimeLimitMilliseconds], [CreatedAt], [DeletedAt]) VALUES (13, 1, NULL, N'clang900', N't1', N'gd', N'id', N'od', N'n', N't', 0, N'#include <iostream> 
 using namespace std;int main() {int n;cin >> n; for (int i = 1;i <= n;i++) {if (n % i == 0)cout << i << " ";}cout << endl;return 0;}', 1000, CAST(N'2025-05-20T19:38:04.543' AS DateTime), NULL)
INSERT [dbo].[Problems] ([ProblemID], [CreatedBy], [DeletedBy], [CompilerName], [Title], [GeneralDescription], [InputDescription], [OutputDescription], [Note], [Tutorial], [Difficulty], [SolutionCode], [TimeLimitMilliseconds], [CreatedAt], [DeletedAt]) VALUES (14, 1, NULL, N'clang900', N't1', N'gd', N'id', N'od', N'n', N't', 0, N'#include <iostream> 
 using namespace std;int main() {int n;cin >> n;for (int i = 1;i <= n;i++) {if (n % i == 0)cout << i << " ";}cout << endl;return 0;}', 1000, CAST(N'2025-05-20T20:07:00.867' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Problems] OFF
GO
SET IDENTITY_INSERT [dbo].[ProblemTags] ON 

INSERT [dbo].[ProblemTags] ([ProblemTagID], [ProblemID], [TagID]) VALUES (1, 9, 1)
INSERT [dbo].[ProblemTags] ([ProblemTagID], [ProblemID], [TagID]) VALUES (2, 9, 2)
INSERT [dbo].[ProblemTags] ([ProblemTagID], [ProblemID], [TagID]) VALUES (3, 10, 1)
INSERT [dbo].[ProblemTags] ([ProblemTagID], [ProblemID], [TagID]) VALUES (4, 10, 2)
INSERT [dbo].[ProblemTags] ([ProblemTagID], [ProblemID], [TagID]) VALUES (5, 11, 1)
INSERT [dbo].[ProblemTags] ([ProblemTagID], [ProblemID], [TagID]) VALUES (6, 11, 2)
INSERT [dbo].[ProblemTags] ([ProblemTagID], [ProblemID], [TagID]) VALUES (7, 12, 1)
INSERT [dbo].[ProblemTags] ([ProblemTagID], [ProblemID], [TagID]) VALUES (8, 12, 2)
INSERT [dbo].[ProblemTags] ([ProblemTagID], [ProblemID], [TagID]) VALUES (9, 13, 1)
INSERT [dbo].[ProblemTags] ([ProblemTagID], [ProblemID], [TagID]) VALUES (10, 13, 2)
INSERT [dbo].[ProblemTags] ([ProblemTagID], [ProblemID], [TagID]) VALUES (11, 14, 1)
INSERT [dbo].[ProblemTags] ([ProblemTagID], [ProblemID], [TagID]) VALUES (12, 14, 2)
SET IDENTITY_INSERT [dbo].[ProblemTags] OFF
GO
SET IDENTITY_INSERT [dbo].[Tags] ON 

INSERT [dbo].[Tags] ([TagID], [Name]) VALUES (1, N'Math')
INSERT [dbo].[Tags] ([TagID], [Name]) VALUES (2, N'Basic')
SET IDENTITY_INSERT [dbo].[Tags] OFF
GO
SET IDENTITY_INSERT [dbo].[TestCases] ON 

INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (1, 9, N'10', N'
1 2 5 10 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (2, 9, N'100', N'
1 2 4 5 10 20 25 50 100 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (3, 9, N'1000', N'
1 2 4 5 8 10 20 25 40 50 100 125 200 250 500 1000 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (4, 10, N'10', N'
1 2 5 10 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (5, 10, N'100', N'
1 2 4 5 10 20 25 50 100 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (6, 10, N'1000', N'
1 2 4 5 8 10 20 25 40 50 100 125 200 250 500 1000 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (7, 11, N'10', N'
1 2 5 10 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (8, 11, N'100', N'
1 2 4 5 10 20 25 50 100 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (9, 11, N'1000', N'
1 2 4 5 8 10 20 25 40 50 100 125 200 250 500 1000 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (10, 12, N'10', N'
1 2 5 10 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (11, 12, N'100', N'
1 2 4 5 10 20 25 50 100 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (12, 12, N'1000', N'
1 2 4 5 8 10 20 25 40 50 100 125 200 250 500 1000 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (13, 13, N'10', N'
1 2 5 10 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (14, 13, N'100', N'
1 2 4 5 10 20 25 50 100 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (15, 13, N'1000', N'
1 2 4 5 8 10 20 25 40 50 100 125 200 250 500 1000 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (16, 14, N'10', N'
1 2 5 10 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (17, 14, N'100', N'
1 2 4 5 10 20 25 50 100 
', 1, 1)
INSERT [dbo].[TestCases] ([TestCaseID], [ProblemID], [Input], [Output], [IsPublic], [IsSample]) VALUES (18, 14, N'1000', N'
1 2 4 5 8 10 20 25 40 50 100 125 200 250 500 1000 
', 1, 1)
SET IDENTITY_INSERT [dbo].[TestCases] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [ImagePath], [Username], [Password], [Role], [CreatedAt]) VALUES (1, N'koko', N'user1', N'1111', 1, CAST(N'2025-05-18T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Problems]  WITH CHECK ADD  CONSTRAINT [FK_Creating_Users_Problems] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Problems] CHECK CONSTRAINT [FK_Creating_Users_Problems]
GO
ALTER TABLE [dbo].[Problems]  WITH CHECK ADD  CONSTRAINT [FK_Deleting_Users_Problems] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Problems] CHECK CONSTRAINT [FK_Deleting_Users_Problems]
GO
ALTER TABLE [dbo].[ProblemSubmissions]  WITH CHECK ADD  CONSTRAINT [FK_Problems_ProblemSubmissions] FOREIGN KEY([ProblemID])
REFERENCES [dbo].[Problems] ([ProblemID])
GO
ALTER TABLE [dbo].[ProblemSubmissions] CHECK CONSTRAINT [FK_Problems_ProblemSubmissions]
GO
ALTER TABLE [dbo].[ProblemSubmissions]  WITH CHECK ADD  CONSTRAINT [FK_Submissions_ProblemSubmissions] FOREIGN KEY([SubmissionID])
REFERENCES [dbo].[Submissions] ([SubmissionID])
GO
ALTER TABLE [dbo].[ProblemSubmissions] CHECK CONSTRAINT [FK_Submissions_ProblemSubmissions]
GO
ALTER TABLE [dbo].[ProblemTags]  WITH CHECK ADD  CONSTRAINT [FK_Problems_ProblemTags] FOREIGN KEY([ProblemID])
REFERENCES [dbo].[Problems] ([ProblemID])
GO
ALTER TABLE [dbo].[ProblemTags] CHECK CONSTRAINT [FK_Problems_ProblemTags]
GO
ALTER TABLE [dbo].[ProblemTags]  WITH CHECK ADD  CONSTRAINT [FK_Tags_ProblemTags] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tags] ([TagID])
GO
ALTER TABLE [dbo].[ProblemTags] CHECK CONSTRAINT [FK_Tags_ProblemTags]
GO
ALTER TABLE [dbo].[Submissions]  WITH CHECK ADD  CONSTRAINT [FK_Users_Submissions] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Submissions] CHECK CONSTRAINT [FK_Users_Submissions]
GO
ALTER TABLE [dbo].[SubmissionTestCases]  WITH CHECK ADD  CONSTRAINT [FK_Submissions_SubmissionTestCases] FOREIGN KEY([SubmissionID])
REFERENCES [dbo].[Submissions] ([SubmissionID])
GO
ALTER TABLE [dbo].[SubmissionTestCases] CHECK CONSTRAINT [FK_Submissions_SubmissionTestCases]
GO
ALTER TABLE [dbo].[SubmissionTestCases]  WITH CHECK ADD  CONSTRAINT [FK_TestCases_SubmissionTestCases] FOREIGN KEY([TestCaseID])
REFERENCES [dbo].[TestCases] ([TestCaseID])
GO
ALTER TABLE [dbo].[SubmissionTestCases] CHECK CONSTRAINT [FK_TestCases_SubmissionTestCases]
GO
ALTER TABLE [dbo].[TestCases]  WITH CHECK ADD  CONSTRAINT [FK_Problems_TestCases] FOREIGN KEY([ProblemID])
REFERENCES [dbo].[Problems] ([ProblemID])
GO
ALTER TABLE [dbo].[TestCases] CHECK CONSTRAINT [FK_Problems_TestCases]
GO
/****** Object:  StoredProcedure [dbo].[SP_Problem_AddNewProblem]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Problem_AddNewProblem]
	@CompilerName NVARCHAR(max),
	@CreatedBy int,
	@Title NVARCHAR(max),
	@GeneralDescription NVARCHAR(max),
	@InputDescription NVARCHAR(max),
	@OutputDescription NVARCHAR(max),
	@Note NVARCHAR(max),
	@Tutorial NVARCHAR(max),
	@Difficulty tinyint,
	@SolutionCode NVARCHAR(max),
	@TimeLimitMilliseconds int,
	@ProblemID INT OUTPUT,
	@IsSuccess bit = null output
AS
BEGIN
	INSERT INTO Problems(CreatedBy,DeletedBy,CompilerName,Title,GeneralDescription,InputDescription,OutputDescription,
						Note,Tutorial,Difficulty,SolutionCode,TimeLimitMilliseconds,CreatedAt,DeletedAt)
	VALUES (@CreatedBy,null,@CompilerName,@Title,@GeneralDescription,@InputDescription,@OutputDescription,
			@Note,@Tutorial,@Difficulty,@SolutionCode,@TimeLimitMilliseconds,GETDATE(),null);
	
	if @@ROWCOUNT > 0
		set @IsSuccess = 1
	else 
		set @IsSuccess = 0

	SET @ProblemID = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Problem_AddNewTagToProblem]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Problem_AddNewTagToProblem]
	@ProblemID INT,
	@TagID int,
	@ProblemTagID INT OUTPUT,
	@IsSuccess bit = null output
AS
BEGIN
	INSERT INTO ProblemTags(ProblemID,TagID)
	VALUES (@ProblemID,@TagID);
	
	if @@ROWCOUNT > 0
		set @IsSuccess = 1
	else 
		set @IsSuccess = 0

	SET @ProblemTagID = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TestCase_AddNewTestCase]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_TestCase_AddNewTestCase]
	@ProblemID INT,
	@Input NVARCHAR(max),
	@Output NVARCHAR(max),
	@IsPublic bit,
	@IsSample bit,
	@TestCaseID INT OUTPUT,
	@IsSuccess bit = null output
AS
BEGIN
	INSERT INTO TestCases(ProblemID,Input,[Output],IsPublic,IsSample)
	VALUES (@ProblemID,@Input,@Output,@IsPublic,@IsSample);
	
	if @@ROWCOUNT > 0
		set @IsSuccess = 1
	else 
		set @IsSuccess = 0

	SET @TestCaseID = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[SP_User_AddNewUser]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_User_AddNewUser]
	@ImagePath NVARCHAR(max),
	@Username NVARCHAR(max),
	@Password NVARCHAR(max),
	@UserID INT OUTPUT,
	@IsSuccess bit = null output
AS
BEGIN
	INSERT INTO Users (ImagePath,Username,[Password],[Role],CreatedAt)
	VALUES (@ImagePath,@Username,@Password, 1, GetDate());
	
	if @@ROWCOUNT > 0
		set @IsSuccess = 1
	else 
		set @IsSuccess = 0

	SET @UserID = SCOPE_IDENTITY();
END;
GO
/****** Object:  StoredProcedure [dbo].[SP_User_DoesUserExistByID]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_User_DoesUserExistByID]
  @UserID INT
AS
BEGIN
  SELECT CASE WHEN EXISTS (SELECT top 1 f=1 FROM Users WHERE UserID = @UserID) 
  THEN cast (1 as bit)
  ELSE cast (0 as bit) 
  END as UserExist
END
GO
/****** Object:  StoredProcedure [dbo].[SP_User_DoesUserExistByUsername]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_User_DoesUserExistByUsername]
  @Username nvarchar(max)
AS
BEGIN
  SELECT CASE WHEN EXISTS (SELECT top 1 f=1 FROM Users WHERE Username = @Username) 
  THEN cast (1 as bit)
  ELSE cast (0 as bit) 
  END as UserExist
END
GO
/****** Object:  StoredProcedure [dbo].[SP_User_GetAllUsers]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_User_GetAllUsers]
	@Page int,
	@Limit int,
    @Username NVARCHAR(max) = NULL
AS
BEGIN
    SELECT  UserID,ImagePath, Username, [Role], CreatedAt
    FROM Users
    WHERE 
       (@Username IS NULL OR Username LIKE '%' + @Username + '%') 
	order by CreatedAt desc
	offset @Limit * (@Page-1) rows 
	fetch next @Limit rows only
END
GO
/****** Object:  StoredProcedure [dbo].[SP_User_GetNumberOfPagesOfUsers]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_User_GetNumberOfPagesOfUsers]
	@Limit int,
    @Username NVARCHAR(max) = NULL
AS
BEGIN
	declare @countAll int = 0;
    SELECT  @countAll = count(*)
    FROM Users
    WHERE 
       (@Username IS NULL OR Username LIKE '%' + @Username + '%') 

	select NumberOfPages = cast(CEILING(cast(@countAll as float)/@Limit) as int)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_User_GetUserByID]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_User_GetUserByID]
	@UserID INT
AS
BEGIN
	SELECT UserID,ImagePath, Username, [Role], CreatedAt
	FROM Users
	WHERE UserID = @UserID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_User_GetUserByUsername]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_User_GetUserByUsername]
	@Username nvarchar(max)
AS
BEGIN
	SELECT UserID,ImagePath, Username, [Role], CreatedAt
	FROM Users
	WHERE Username = @Username
END
GO
/****** Object:  StoredProcedure [dbo].[SP_User_GetUserByUsernameAndPassword]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_User_GetUserByUsernameAndPassword]
	@Username NVARCHAR(max),
	@Password NVARCHAR(max)
AS
BEGIN
	SELECT UserID,ImagePath, Username, [Role], CreatedAt
	from Users
	WHERE Username = @Username AND [Password] = @Password;
END;
GO
/****** Object:  StoredProcedure [dbo].[SP_User_UpdateUser]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_User_UpdateUser]
	@UserID INT,
	@ImagePath NVARCHAR(max),
	@IsSuccess bit = null output
AS
BEGIN
	UPDATE Users
	SET
	ImagePath = @ImagePath
	WHERE UserID = @UserID;

	if @@ROWCOUNT > 0
		set @IsSuccess = 1
	else 
		set @IsSuccess = 0
END;
GO
/****** Object:  StoredProcedure [dbo].[SP_User_UpdateUserPassword]    Script Date: 5/20/2025 10:55:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_User_UpdateUserPassword]
	@UserID INT,
	@OldPassword NVARCHAR(100) = null,
	@NewPassword NVARCHAR(100),
	@IsSuccess bit = null output
AS
BEGIN
	UPDATE Users
	SET Password = @NewPassword
	WHERE UserID = @UserID AND (@OldPassword is null or Password = @OldPassword)

	if @@ROWCOUNT > 0
		set @IsSuccess = 1
	else 
		set @IsSuccess = 0
END
GO
USE [master]
GO
ALTER DATABASE [ProblemSolvingPlatformDB] SET  READ_WRITE 
GO

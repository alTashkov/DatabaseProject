# 💾 DatabaseProject

<div align="center">

[![GitHub stars](https://img.shields.io/github/stars/alTashkov/DatabaseProject?style=for-the-badge)](https://github.com/alTashkov/DatabaseProject/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/alTashkov/DatabaseProject?style=for-the-badge)](https://github.com/alTashkov/DatabaseProject/network)
[![GitHub issues](https://img.shields.io/github/issues/alTashkov/DatabaseProject?style=for-the-badge)](https://github.com/alTashkov/DatabaseProject/issues)
[![GitHub license](https://img.shields.io/github/license/alTashkov/DatabaseProject?style=for-the-badge)](LICENSE) <!-- TODO: Add LICENSE file to the repository -->

**A C# .NET solution for robust database interaction and management.**

</div>

## 📖 Overview

This repository hosts a C# .NET application designed for efficient database interaction. It provides a foundational structure for managing and manipulating data in desktop applications, console tools, or a data access layer for more complex systems.

## ✨ Features

-   🎯 **Database Connectivity**: Establish and manage connections to relational databases.
-   🛠️ **Data Manipulation**: Perform Create, Read, Update, and Delete (CRUD) operations.
-   ⚙️ **Structured Project**: Organized C# solution for maintainability and scalability.

## 🛠️ Tech Stack

**Language & Runtime:**
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

**Package Manager:**
![NuGet](https://img.shields.io/badge/NuGet-004880?style=for-the-badge&logo=nuget&logoColor=white)

**Database:**
![SQL](https://img.shields.io/badge/SQL-4479A1?style=for-the-badge&logo=sql&logoColor=white)
SSMS

## 🚀 Quick Start

### Prerequisites
-   **Visual Studio** (recommended for .NET development) or **.NET SDK** (version 6.0 or higher recommended).
    -   [Download Visual Studio](https://visualstudio.microsoft.com/downloads/)
    -   [Download .NET SDK](https://dotnet.microsoft.com/download)

### Installation

1.  **Clone the repository**
    ```bash
    git clone https://github.com/alTashkov/DatabaseProject.git
    cd DatabaseProject
    ```

2.  **Restore dependencies**
    NuGet packages will typically be restored automatically when you open the solution in Visual Studio or build from the command line. If not, you can run:
    ```bash
    dotnet restore
    ```

3.  **Database setup**
    This project is designed to interact with a database. You will need to:
    -   **Set up your database**: Create a database instance (e.g., SQL Server, SQLite file, etc.).
    -   **Configure the connection string**: Update the connection string in the project's configuration (`app.config` within the `DatabaseProject` directory).

4.  **Build and Run**
    **Using Visual Studio:**
    -   Open `DatabaseProject.sln` in Visual Studio.
    -   Build the solution (Build > Build Solution).
    -   Run the project (Debug > Start Debugging or F5).

    **Using .NET CLI:**
    ```bash
    dotnet build DatabaseProject.sln
    # To run, navigate into the project directory (e.g., DatabaseProject/bin/Debug/net6.0)
    # and execute the compiled application or use:
    dotnet run --project DatabaseProject/DatabaseProject.csproj
    ```

## 📁 Project Structure

```
DatabaseProject/
├── .gitignore               # Git ignore file (Edited last month)
├── DatabaseProject.sln      # Visual Studio Solution file
├── README.md                # Project documentation
└── DatabaseProject/         # Main C# Project Source
    ├── App.config           # Application configuration
    ├── ContainerConfig.cs   # Autofac IoC registration and configuration
    ├── DatabaseProject.csproj # Project file with dependencies
    ├── Program.cs           # Entry point (CLI Loop and Main scope)
    │
    ├── Data/                # EF Core Context and Filter logic
    │   └── SocialMediaContext.cs
    │
    ├── Diagram/            # Database diagrams or documentation
    │   └── databaseProjectDiagram.png
    | 
    ├── Factories/           # Implementation of service resolution
    │   └── ServiceFactory.cs (Refactored recently)
    │
    ├── Helpers/             # Orchestration and static logic
    │   ├── OperationManager.cs
    │   └── DataProcessor.cs
    │
    ├── Interfaces/          # Generic service and factory definitions
    │   ├── IServiceFactory.cs
    │   ├── IBulkInserter.cs
    │   ├── IJsonProcessor.cs
    |   ├── IBulkExporter.cs
    |   ├── IDataDeleter.cs
    |   ├── IDataUpdater.cs
    |   └── ILoggable.cs
    │
    ├── Migrations/          # EF Core migration history
    │
    ├── Models/              # Entity classes (User, Post, etc.)
    |   ├── Comment.cs
    |   ├── Friendship.cs
    |   ├── Group.cs
    |   ├── Post.cs
    |   ├── Profile.cs
    |   └── User.cs
    ├── Services/            # Concrete implementations of interfaces
    │   ├── BulkInsertService.cs
    │   ├── BulkOutputService.cs
    |   ├── DeleteDataService.cs
    |   ├── JsonFileService.cs
    |   └── UpdateDataService.cs
    │
    └── JSON Test Files/     # Root-level test data for imports
        ├── friendship.json
        ├── posts.json
        ├── testREAD2.json
        ├── dbOutputFromReading.json
        └── ...others...
```

## ⚙️ Configuration

### Database Connection String
The application will require a database connection string. It is defined in the `app.config` file within the `DatabaseProject` directory:

```xml
<configuration>
  <connectionStrings>
    <add name="DefaultConnection" 
         connectionString="Server=.\SQLEXPRESS;Database=DatabaseProject;Trusted_Connection=True;TrustServerCertificate=True;" 
         providerName="System.Data.SqlClient" />
  </connectionStrings>
</configuration>
```

## 🔧 Development

### Available Commands
-   **`dotnet restore`**: Restores the dependencies and tools of a project.
-   **`dotnet build`**: Builds a project and all of its dependencies.
-   **`dotnet run`**: Runs source code without any explicit compile or launch commands.

### Development Workflow
To contribute or develop further:
1.  Open `DatabaseProject.sln` in Visual Studio.
2.  Navigate to the `DatabaseProject` folder to find the C# source files (`.cs`).
3.  Modify the code and project settings as needed.
4.  Build and run to test changes.

## 🤝 Contributing

I welcome contributions! Please consider forking the repository and submitting pull requests. For major changes, please open an issue first to discuss what you would like to change.

## 📞 Support & Contact

-   🐛 Issues: [GitHub Issues](https://github.com/alTashkov/DatabaseProject/issues)

---

<div align="center">

**⭐ Star this repo if you find it helpful!**
</div>

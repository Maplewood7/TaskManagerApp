# TaskManagerApp

A Blazor task manager for organizing tasks by tags, tracking progress, and reviewing completed work.

## Features

- Register, sign in, and sign out with ASP.NET Core Identity.
- Create tasks with a title, optional description, and tag.
- Start with **Work** and **Personal** tags, then add your own.
- View ongoing tasks and completion progress for each tag.
- Mark tasks complete, remove tasks, and review completed tasks in Task History.
- Keep tasks and tags associated with the signed-in user.

## Technology

- **.NET 8**
- **Blazor Web App with Interactive Server rendering**
- **ASP.NET Core Identity**
- **Entity Framework Core**
- **SQL Server**

## Project structure

The solution is in the **TaskManagerApp/** directory:

- **WebApp/** — Blazor UI, routing, authentication configuration, and application startup.
- **App/** — Application services for tasks, tags, users, and authentication.
- **Domain/** — Domain entities and interfaces.
- **Infrastructure/** — EF Core database context, migrations, and repositories.

## Prerequisites

- .NET 8 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 (optional)

## Configure the database

The app reads its SQL Server connection string from *ConnectionStrings:DefaultConnection*. For local development, store it with User Secrets rather than committing credentials:


  &emsp;&emsp;*cd TaskManagerApp*

  &emsp;&emsp;*dotnet user-secrets init --project WebApp/WebApp.csproj*

  &emsp;&emsp;*dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your SQL Server connection string>" --project WebApp/WebApp.csproj*


If User Secrets are already initialized for *WebApp*, skip the *init* command. The app applies pending EF Core migrations at startup, so the configured SQL Server must be available.

## Run locally

From the repository root:


  &emsp;&emsp;*cd TaskManagerApp*

  &emsp;&emsp;*dotnet restore WebApp/WebApp.csproj*

  &emsp;&emsp;*dotnet build WebApp/WebApp.csproj*

  &emsp;&emsp;*dotnet run --project WebApp/WebApp.csproj*


Open the local URL printed by *dotnet run*. In Visual Studio, open the solution in *TaskManagerApp/*, set *WebApp* as the startup project, and run with **F5**.

## Using the app

1. Register an account and sign in.
2. Open **Add Task**. The **Work** and **Personal** tags are created for the account if they do not already exist.
3. Add more tags and choose one when creating a task.
4. Open **Ongoing Tasks** to view tasks and each tag’s completion progress. Progress groups are ordered from most complete to least complete.
5. Mark a task complete to move it to **Task History**, or remove it to delete it.

## Development note

This is a learning/test project. Its current password policy is intentionally permissive. Strengthen it and review authentication, authorization, error handling, and deployment settings before exposing the app publicly. Do not commit real database credentials or other secrets.

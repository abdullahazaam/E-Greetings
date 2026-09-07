# E-Greetings

E-Greetings is an ASP.NET Core project for creating and scheduling digital greeting cards. I built it to practise working with templates, user accounts, scheduled work and email-related features in one application.

## What it does

- Lets users browse greeting-card templates
- Supports personalised messages and card previews
- Saves drafts and scheduled greetings
- Includes registration, sign-in and account management
- Handles subscriptions and user preferences
- Provides an admin area for templates, users and reports
- Uses a background service for scheduled email work

## Technology

- ASP.NET Core MVC on .NET 8
- C# and Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- Razor views, Bootstrap, CSS and JavaScript

## Running the project

You will need the .NET 8 SDK and SQL Server.

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Set the database connection and mail configuration through user secrets, environment variables or a local configuration file that is not committed to Git.

## Current status

The main application flows are implemented and the repository includes a GitHub Actions build check. Automated application tests have not been added yet, so that is one of the next improvements I would make.


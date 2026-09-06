# E-Greetings

E-Greetings is an ASP.NET Core MVC portfolio application for creating, scheduling, managing, and sharing digital greeting cards.

## Features

- Account registration, email confirmation, login, password recovery, and profile management
- Greeting categories and reusable card templates
- Personalised card creation, preview, download, scheduling, and send workflow
- Subscription plans and transaction records
- Customer feedback
- Admin dashboard for templates, users, roles, transactions, feedback, and reports
- Scheduled email background service
- Entity Framework Core migrations for SQL Server

## Technology

- .NET 8 and ASP.NET Core MVC
- C# and Razor views
- ASP.NET Core Identity
- Entity Framework Core with SQL Server
- HTML, CSS, Bootstrap, and JavaScript

## Repository structure

```text
E-Greetings/
├── E-Greetings.sln
└── E-Greetings/
    ├── Controllers/
    ├── Data/
    ├── Migrations/
    ├── Models/
    ├── Services/
    ├── Views/
    └── wwwroot/
```

## Run locally

### Requirements

- .NET 8 SDK
- SQL Server or SQL Server LocalDB
- EF Core CLI

```bash
git clone https://github.com/abdullahazaam/E-Greetings.git
cd E-Greetings
dotnet restore
dotnet ef database update --project E-Greetings/E-Greetings.csproj
dotnet run --project E-Greetings/E-Greetings.csproj
```

Provide configuration through environment variables or an untracked local settings file:

```text
ConnectionStrings__DefaultConnection
EmailSettings__SmtpServer
EmailSettings__SmtpPort
EmailSettings__SmtpUsername
EmailSettings__SmtpPassword
EmailSettings__FromEmail
```

Never commit database or email credentials.

## Build verification

```bash
dotnet build E-Greetings.sln --configuration Release
```

Automated tests are not currently included. Adding controller/service tests and an integration-test project is an explicit next step rather than an unverified claim.

## Portfolio status

This repository demonstrates a complete MVC application flow and admin features. Before real-world deployment it still needs automated test coverage, production secret management, reliable email delivery monitoring, and deployment-specific security review.

## Author

**Abdullah Azaam** — junior web developer focused on ASP.NET Core, C#, SQL Server, PHP, and Laravel.


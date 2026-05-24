<img align="left" width="116" height="116" src="../../Assets/logo.png" />

# &nbsp;**E List - Phase 8 — Portal MVC (Final)** [![.NET - Phase 8 - Portal - MVC - Final](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase8-dashboard-mvc-final.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase8-dashboard-mvc-final.yml)

<br/><br/>

## Quick facts

- Estimated time: 3 - 5 hours (assumes completion of Phase 8 src/02. MVC)
- Difficulty: Intermediate ▮▮▮▯▯ (3/5)
- Target SDK: .NET 10 (net10)
- Audience: Developers who have built the API and generated the NSwag client and want to wire everything together into a running full-stack solution

## Goal

This is the completed full-stack reference solution for Phase 8. It combines the REST API, the NSwag-generated `Api.Client`, the ASP.NET Core MVC `UI` project, a background `Scheduler` (Quartz.NET job for sending email reminders), and the full `Test` suite into one deployable solution.

Use this folder to verify your own implementation or as a starting point for Phase 9.

## Solution structure

```
Phase 8/src/03. MVC/
├── Api/            REST API (CQRS, MediatR, EF Core, FluentValidation)
├── Api.Client/     NSwag-generated C# HTTP client
├── Common/         Shared entities, models, extensions
├── Core/           CQRS commands, queries, validators, behaviours
├── DataAccess/     EF Core DbContext, entity mappings
├── Scheduler/      Quartz.NET background job (email reminders)
├── Test/           NUnit unit tests
└── UI/             ASP.NET Core MVC frontend (consumes Api.Client)
```

## Prerequisites

- Completed Phase 8 src/02. MVC (NSwag client generation working)
- .NET SDK 10 installed
- Basic understanding of ASP.NET Core MVC and session state

## What this solution demonstrates

1. **MVC frontend** consuming the NSwag-generated `Api.Client` for all CRUD operations.
2. **Session-scoped todos** — each browser session gets its own isolated todo list via a `SessionId` GUID.
3. **Background Scheduler** — a Quartz.NET hosted service sends email reminders for incomplete todos using the `Core.Email.EmailService`.
4. **Behaviour pipeline** — `PerformanceBehaviour`, `UnhandledExceptionBehaviour`, and `ValidationBehavior` in MediatR pipeline.
5. **Integrated test suite** — in-memory EF Core database, `TestBase`, `QueryTestBase`, and `TodoTestData` for repeatable unit tests.

## Validate / Quick commands

```powershell
# check .NET version (should be 10.x)
dotnet --version

# restore and build the full solution
dotnet build "./Phase 8/src/03. MVC/Pezza.slnx"

# run the unit tests
dotnet test "./Phase 8/src/03. MVC/Pezza.slnx"

# run the API (in a separate terminal)
dotnet run --project "./Phase 8/src/03. MVC/Api/Api.csproj"

# run the MVC UI (in a separate terminal — update API URL in HomeController first)
dotnet run --project "./Phase 8/src/03. MVC/UI/UI.csproj"
```

## Key implementation notes

### Session-scoped todos (UI `HomeController`)

```csharp
public class HomeController(IHttpClientFactory httpClientFactory) : Controller
{
    private const string SessionKey = "SessionId";
    private readonly TodosClient todoClient = new("https://localhost:44315/", httpClientFactory.CreateClient());

    public async Task<ActionResult<List<TodoModel>>> Index()
    {
        if (string.IsNullOrEmpty(this.HttpContext.Session.GetString(SessionKey)))
        {
            this.HttpContext.Session.SetString(SessionKey, Guid.NewGuid().ToString());
        }
        // ...
    }
}
```

### Session + HttpClient registration (`Program.cs`)

```csharp
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpClient();

app.UseSession();
```

### Scheduler (Quartz.NET email job)

`Scheduler/Jobs/EmailJob.cs` fires on a schedule and calls `Core.Email.EmailService` to notify users about incomplete todos. Configure the cron expression and SMTP settings in `appsettings.json`.

## Outcomes / Learning goals

- Understand how an NSwag-generated client decouples the UI from direct HTTP calls.
- Wire ASP.NET Core MVC session state to isolate per-user data without authentication.
- Add a Quartz.NET background job to an existing solution without disrupting the API or UI.
- Run the complete layered solution (API + UI + Scheduler) simultaneously.

# Phase 9: Aspire Orchestration with OpenTelemetry

This phase introduces **cloud-native architecture** using .NET Aspire for service orchestration and OpenTelemetry for comprehensive observability.

## Architecture Overview

### Key Components

1. **AspireHost** - Service orchestration and discovery
   - Configures Pezza.Api with database resources
   - Manages container lifecycle
   - Provides Aspire dashboard for monitoring

2. **SQL Server** - Relational data store
   - Docker container (configurable)
   - SQL Server 2022 or MySQL 8.0 alternatives
   - Persistent volumes for data

3. **OpenTelemetry** - Observability stack
   - Structured logging with Serilog
   - Distributed tracing
   - Metrics collection
   - OTLP exporter support

## Prerequisites

- .NET 8.0 SDK or later
- Docker Desktop (for containerized databases)
- Visual Studio Code or Visual Studio

## Running the Solution

### Option 1: Using Aspire (Recommended for Development)

```powershell
cd Phase9/src/01.StartSolution
dotnet run --project AspireHost/AspireHost.csproj
```

This will:
1. Start the Aspire orchestration host
2. Launch SQL Server in a container
3. Start Pezza.Api with database connection
4. Open the Aspire dashboard at `http://localhost:18888`

### Option 2: Using Docker Compose

```powershell
docker-compose -f Phase9/src/01.StartSolution/docker-compose.yml up -d
```

Then start the application:

```powershell
cd Phase9/src/01.StartSolution
dotnet run --project Pezza.Api/Pezza.Api.csproj
```

## OpenTelemetry Configuration

### Logging

Configured via `Pezza.Api/Program.cs`:
- Serilog with structured logging
- OTLP exporter for centralized logging
- Log levels: Information (default), Error (System/Microsoft)

### Metrics

Enabled through OpenTelemetry:
- ASP.NET Core instrumentation
- HTTP client instrumentation  
- Runtime metrics

### Tracing

Distributed tracing configuration:
- ASP.NET Core tracing
- HTTP client tracing
- W3C Trace Context propagation

## Service Discovery

Services automatically discoverable via:
- Service name: `http://api`
- Resilience: Automatic retry policies
- Circuit breaker patterns included

## Database Configuration

### Using Aspire

The AspireHost automatically manages SQL Server:

```csharp
var database = builder
    .AddSqlServer("sql-server", port: 1433)
    .AddDatabase("pezza-db");
```

### Connection String

Environment variable: `ConnectionStrings__DefaultConnection`

**SQL Server Format:**
```
Server=localhost,1433;User Id=sa;Password=YourComplexPassword123!;Database=pezza-db;TrustServerCertificate=true
```

**MySQL Format:**
```
Server=localhost;Port=3306;Database=pezza_db;User Id=pezza_user;Password=UserPassword123!
```

## Aspire Dashboard

Access monitoring dashboard:
- **URL**: `http://localhost:18888`
- **Features**:
  - Service topology visualization
  - Real-time metrics
  - Log streaming
  - Environment variables view
  - Health check status

## Directory Structure

```
Phase 9/
├── src/
│   ├── 01. StartSolution/
│   │   ├── AspireHost/
│   │   │   ├── AspireHost.csproj
│   │   │   ├── Program.cs
│   │   │   ├── Extensions.cs
│   │   │   ├── GlobalUsings.cs
│   │   │   └── appsettings.json
│   │   ├── Pezza.Api/
│   │   ├── Pezza.Core/
│   │   ├── Pezza.Common/
│   │   ├── docker-compose.yml
│   │   └── .NET.Pezza.sln
│   └── ... (other solution items)
└── README.md
```

## LiteBus Integration

Pezza.Api uses **LiteBus 1.0.0** for command/query handling:

```csharp
// Query example
var customer = await this.QryMediator.SendAsync(
    new GetCustomerByIdQuery { Id = id }
);

// Command example
await this.CmdMediator.SendAsync(
    new AddPizzaCommand { Name = "Margherita", Price = 9.99m }
);
```

## Configuration

### Aspire Host Settings

`appsettings.json` - Logging configuration:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Aspire.Hosting.Dcp": "Warning"
    }
  }
}
```

### Environment Variables

Automatically set by AspireHost:
- `ASPIRE_RESOURCE_NAME` - Resource identifier
- `ASPIRE_DASHBOARD_URL` - Dashboard endpoint
- OpenTelemetry endpoints for API

## Troubleshooting

### Port Conflicts

If ports 1433 (SQL Server), 5000 (API), or 18888 (Dashboard) are in use:

1. Find process using port: `netstat -ano | findstr :1433`
2. Kill process: `taskkill /PID <PID> /F`
3. Or adjust ports in `docker-compose.yml` or AspireHost Program.cs

### Database Connection Issues

Check connection string:
```powershell
sqlcmd -S localhost,1433 -U sa -P YourComplexPassword123! -Q "SELECT 1"
```

### OpenTelemetry Not Working

Verify environment variables in Aspire dashboard:
- Check `OTEL_EXPORTER_OTLP_ENDPOINT`
- Ensure OTLP collector is running (if using external)

## Next Steps

- **Phase 10**: Add DbUp for database migrations
- **Phase 11**: Combine Aspire orchestration with DbUp migrations
- **Phase 12**: Create MCP Server for AI integration

## References

- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [OpenTelemetry Documentation](https://opentelemetry.io/docs/)
- [LiteBus Documentation](https://github.com/rafaelfeitosa/LiteBus)
- [Docker Compose Reference](https://docs.docker.com/compose/)
- .NET SDK 10 installed
- Basic knowledge of authentication concepts (JWT/OAuth2) and web security fundamentals

## What you'll do (high level)

1. Add or validate authentication (JWT/OAuth2) for the Api and protect endpoints.
2. Add antiforgery validation to MVC endpoints that accept browser POSTs.
3. Enforce HTTPS and HSTS for production environments.
4. Harden cookie settings, secure headers and secrets handling.
5. Run quick automated checks and add CI validations for build and basic security tests where possible.

## Quick checklist & snippets

1. Enforce HTTPS & HSTS (Program.cs / Startup)

```csharp
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
```

1. Secure cookie settings

```csharp
services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});
```

1. JWT authentication (example)

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });

app.UseAuthentication();
app.UseAuthorization();
```

1. Antiforgery for browser POSTs

```csharp
services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

[ValidateAntiForgeryToken]
public IActionResult PostOrder(OrderModel model) { ... }
```

1. Secure headers (minimal middleware)

```csharp
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    await next();
});
```

1. Secrets & data protection

- Use `dotnet user-secrets` for local dev secrets.
- Use a vault (Azure Key Vault, AWS Secrets Manager) for CI/production secrets.
- Persist data protection keys to a shared store in multi-instance deployments.

## Validate / Quick commands

```powershell
# check .NET version (should be 10.x)
dotnet --version

# build the Phase 8 solution
dotnet build "./Phase 8/src/01. StartSolution/Pezza.sln"

# run tests (if present)
dotnet test "./Phase 8/src/01. StartSolution/Pezza.sln"
```

## Outcomes / Learning goals

- Configure JWT/OAuth and protect API endpoints.
- Harden cookie policies, antiforgery and secure headers for web UIs.
- Understand secrets management options for dev and CI.

---

If you'd like, I can make a small, gated code change to the `Api` project's `Program.cs` to enable HTTPS/HSTS and add the secure-headers middleware behind an environment check — I will only do that if you ask me to create a PR.


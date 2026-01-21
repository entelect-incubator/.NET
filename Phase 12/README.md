# Phase 12: Complete Cloud-Native Stack (Aspire + DbUp Integration)

This phase combines **Aspire orchestration** with **DbUp migrations** to create a complete, production-ready cloud-native architecture with automated database versioning and service orchestration.

## Architecture Overview

**What’s unique in Phase 12**
- Adds Aspire orchestration on top of the Phase 11 DbUp migrations.
- Ensures migrations run before API startup with service dependencies wired in AspireHost.
- Extends observability (logging, metrics, tracing) across migrations and API.

### The Complete Stack

```
┌─────────────────────────────────────────────────────────────┐
│                    Aspire Orchestrator                       │
│                   (AspireHost - .NET 8)                     │
│                                                              │
│  ┌──────────────────┐    ┌──────────────────┐              │
│  │  SQL Server      │    │  Pezza.Api       │              │
│  │  (Container)     │◄───┤  (with OpenTel)  │              │
│  │  1433 → 1433     │    │  5000, 5001      │              │
│  └──────────────────┘    └──────────────────┘              │
│         ▲                          ▲                        │
│         │ Managed by               │ Depends on            │
│         └─────────────┬─────────────┘                       │
│                       │                                    │
│         ┌─────────────────────────┐                        │
│         │  DbUp.Migrations        │                        │
│         │  (Runs First!)          │                        │
│         │  • Initial Setup        │                        │
│         │  • Create Tables        │                        │
│         │  • Seed Data            │                        │
│         └─────────────────────────┘                        │
│                                                              │
│         Aspire Dashboard: http://localhost:18888            │
│         API Endpoints: http://localhost:5000/api            │
└─────────────────────────────────────────────────────────────┘

OpenTelemetry Stack:
├── Logging (Serilog → OTLP)
├── Metrics (Runtime, HTTP)
└── Tracing (W3C Trace Context)
```

## Key Innovations in Phase 12

### 1. Orchestrated Migrations

- DbUp runs **before** Pezza.Api starts
- Automatic schema versioning and tracking
- Zero-downtime deployment ready

### 2. Service Dependencies

- AspireHost manages dependency ordering
- Network isolation with Docker
- Health checks on all services

### 3. Full Observability

- Structured logging from both API and migrations
- Distributed tracing across services
- Real-time Aspire dashboard monitoring

### 4. Production Patterns

- Database backup strategies
- Migration rollback procedures
- CI/CD integration examples

## Prerequisites

- .NET 10.0 SDK or later
- Docker Desktop (for SQL Server container)
- 2GB free disk space (for Docker image)
- SQL Server Express (local) or equivalent

## Running Migrations

### Option 1: Run from Visual Studio (Phase 12 path)

```powershell
cd "Phase 12/src/01. StartSolution"
dotnet run --project DbUp.Migrations/DbUp.Migrations.csproj
```

### Option 2: Pass Connection String as Argument

```powershell
dotnet run --project DbUp.Migrations/DbUp.Migrations.csproj -- "Server=localhost,1433;User Id=sa;Password=YourComplexPassword123!;Database=pezza-db;TrustServerCertificate=true"
```

### Option 3: Using Docker

First, start the database:

```powershell
docker-compose -f docker-compose.yml up -d sql-server
```

Then run migrations:

```powershell
dotnet run --project DbUp.Migrations/DbUp.Migrations.csproj
```

## Migration Script Structure

### Naming Convention

Scripts follow the format: `{SequenceNumber}_{ScriptName}.sql`

Example:

```text
Scripts/001_Initial/001_InitialSetup.sql
Scripts/002_Tables/001_CreateTables.sql
Scripts/003_Data/001_InsertSampleData.sql
```

**Important**: DbUp executes scripts in order, so naming matters!

### Idempotency Requirement

All scripts must be **idempotent** (safe to run multiple times):

```sql
-- ✅ Good - Idempotent
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pizza')
BEGIN
    CREATE TABLE [dbo].[Pizza] (...)
END

-- ❌ Bad - Not idempotent
CREATE TABLE [dbo].[Pizza] (...)
```

## Database Schema

### Core Tables

#### Customer

```sql
Id (GUID, PK)
FirstName, LastName, Email
Phone, Address, City, ZipCode
CreatedDate, UpdatedDate
```

#### Pizza

```sql
Id (GUID, PK)
Name, Description, Price
PictureUrl, Offer, OfferStarts, OfferEnds
CreatedDate, UpdatedDate
```

#### Order

```sql
Id (GUID, PK)
CustomerId (FK → Customer)
OrderNumber (UNIQUE)
OrderDate, Completed, Total
CreatedDate, UpdatedDate
```

#### OrderItem

```sql
Id (GUID, PK)
OrderId (FK → Order, CASCADE)
PizzaId (FK → Pizza)
Quantity, UnitPrice, Total
CreatedDate
```

#### Stock

```sql
Id (GUID, PK)
PizzaId (FK → Pizza, CASCADE)
Quantity, ReorderLevel
CreatedDate, UpdatedDate
```

#### Notify

```sql
Id (GUID, PK)
Title, Message
IsRead
CreatedDate
```

#### SchemaVersions (DbUp Internal)

```sql
Id (INT, PK - Identity)
SchemaVersion, Description
Installed, Success
```

## Adding New Migrations

### Step 1: Create Migration Directory

```powershell
mkdir "Scripts/004_NewFeature"
```

### Step 2: Create SQL Script

Example: `Scripts/004_NewFeature/001_AddPizzaCategory.sql`

```sql
-- Add PizzaCategory table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PizzaCategory')
BEGIN
    CREATE TABLE [dbo].[PizzaCategory]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [Name] NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETUTCDATE()
    );
    
    CREATE INDEX IX_PizzaCategory_Name ON [dbo].[PizzaCategory] ([Name]);
    
    PRINT 'PizzaCategory table created successfully';
END

-- Add CategoryId column to Pizza
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Table_Name = 'Pizza' AND Column_Name = 'CategoryId')
BEGIN
    ALTER TABLE [dbo].[Pizza]
    ADD [CategoryId] UNIQUEIDENTIFIER NULL;
    
    ALTER TABLE [dbo].[Pizza]
    ADD CONSTRAINT FK_Pizza_Category 
    FOREIGN KEY ([CategoryId]) 
    REFERENCES [dbo].[PizzaCategory] ([Id]);
    
    PRINT 'CategoryId column added to Pizza table';
END
```

### Step 3: Run Migrations

```powershell
dotnet run --project DbUp.Migrations/DbUp.Migrations.csproj
```

## Tracking Applied Migrations

DbUp maintains a `SchemaVersions` table:

```sql
SELECT * FROM [dbo].[SchemaVersions] ORDER BY [SchemaVersion]
```

Output:
```
Id  SchemaVersion  Description              Installed            Success
1   1              001_InitialSetup         2025-10-30 14:32:10  1
2   2              001_CreateTables         2025-10-30 14:32:11  1
3   3              001_InsertSampleData     2025-10-30 14:32:12  1
```

## Logging

DbUp logs to console with Serilog:

```text
[10:30:15 INF] Starting database migration...
[10:30:15 INF] Connection string: Server=localhost,1433;User Id=sa;Password=****;...
[10:30:15 INF] Executing script: 001_InitialSetup.sql
[10:30:15 INF] Executing script: 001_CreateTables.sql
[10:30:16 INF] Executing script: 001_InsertSampleData.sql
[10:30:16 INF] Database migration completed successfully
[10:30:16 INF] Scripts executed: 3
```

## Connection String Management

### Development (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;User Id=sa;Password=YourComplexPassword123!;Database=pezza-db;TrustServerCertificate=true"
  }
}
```

### Production (Environment Variable)

Pass via command line:

```powershell
dotnet run --project DbUp.Migrations/DbUp.Migrations.csproj -- "Server=prod-sql.example.com;User Id=app_user;Password=SecurePassword123!;Database=pezza-prod"
```

### Azure SQL / Cloud

```json
Server=pezza.database.windows.net;User Id=sa@pezza;Password=AzurePassword123!;Database=pezza-db;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30
```

## CI/CD Integration

### GitHub Actions Example

```yaml
name: Database Migration

on: [push, pull_request]

jobs:
  migrate:
    runs-on: ubuntu-latest
    services:
      mssql:
        image: mcr.microsoft.com/mssql/server:latest
        env:
          SA_PASSWORD: TestPassword123!
          ACCEPT_EULA: Y
        options: >-
          --health-cmd "/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TestPassword123! -Q 'SELECT 1'"
          --health-interval 10s
          --health-timeout 5s
          --health-retries 10

    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      
      - name: Run Migrations
        run: |
          dotnet run --project "Phase 12/src/01. StartSolution/DbUp.Migrations/DbUp.Migrations.csproj" -- \
            "Server=localhost;User Id=sa;Password=TestPassword123!;Database=pezza-db;TrustServerCertificate=true"
```

## Directory Structure

```md
Phase 12/
├── src/
│   ├── 01. StartSolution/
│   │   ├── DbUp.Migrations/
│   │   │   ├── DbUp.Migrations.csproj
│   │   │   ├── Program.cs
│   │   │   ├── GlobalUsings.cs
│   │   │   ├── appsettings.json
│   │   │   └── Scripts/
│   │   │       ├── 001_Initial/
│   │   │       │   └── 001_InitialSetup.sql
│   │   │       ├── 002_Tables/
│   │   │       │   └── 001_CreateTables.sql
│   │   │       └── 003_Data/
│   │   │           └── 001_InsertSampleData.sql
│   │   ├── Pezza.Api/
│   │   ├── Pezza.Core/
│   │   ├── Pezza.Common/
│   │   ├── AspireHost/
│   │   ├── docker-compose.yml
│   │   └── Pezza.slnx
│   └── ... (other solution items)
└── README.md
```

## Rollback Strategy

DbUp **does not support rollbacks** by design. Instead:

1. **Create rollback scripts** in a new migration
2. **Test thoroughly** before production deployment
3. **Backup database** before each migration run

Example rollback script:

```sql
-- Scripts/004_Rollback/001_RemoveUnusedColumn.sql
IF EXISTS (SELECT * FROM sys.columns WHERE Table_Name = 'Pizza' AND Column_Name = 'OldColumn')
BEGIN
    ALTER TABLE [dbo].[Pizza]
    DROP COLUMN [OldColumn];
    
    PRINT 'OldColumn removed from Pizza table';
END
```

## Performance Tips

### Indexes

- Create indexes on foreign keys
- Create indexes on frequently searched columns (Email, Name)
- Use filtered indexes for sparse data

### Constraints

- Enforce NOT NULL where appropriate
- Use UNIQUE constraints for business keys (OrderNumber)
- Cascade deletes carefully to prevent data loss

### Batch Operations

- Insert sample data in batches of 1000+
- Use bulk insert tools for large datasets
- Monitor migration execution time

## Troubleshooting

### Connection String Errors

```text
Error: Cannot open database 'pezza-db' requested by the login.
```

**Solution**: 
- Check database exists: `SELECT name FROM sys.databases`
- Create database if missing: `CREATE DATABASE [pezza-db]`

### Script Execution Failures

```text
Error: Column 'PizzaId' already exists in table 'Pizza'
```

**Solution**:

- Check if script ran previously: `SELECT * FROM SchemaVersions`
- Make scripts idempotent with `IF NOT EXISTS` checks

### Permission Denied

```text
Error: The server principal 'domain\user' is not able to access the database 'pezza-db'
```

**Solution**:

- Grant user database access: `GRANT CONNECT ON DATABASE pezza-db TO [domain\user]`
- Use SQL authentication if Windows auth unavailable

## Integration with Pezza.Api

In Phase 12, DbUp migrations are orchestrated by Aspire so migrations run before the API starts:

```csharp
// AspireHost/Program.cs - Phase 12 example
var database = builder
    .AddSqlServer("sql-server")
    .AddDatabase("pezza-db");

var migrations = builder
    .AddProject<DbUp_Migrations>("migrations")
    .WithReference(database);

var api = builder
    .AddProject<Pezza_Api>("api")
    .WithReference(database)
    .WithReference(migrations); // Ensure migrations run first
```

## References

- [DbUp Documentation](https://dbup.readthedocs.io/)
- [DbUp GitHub](https://github.com/DbUp/DbUp)
- [SQL Server Tutorial](https://learn.microsoft.com/en-us/sql/t-sql/tutorial-writing-transact-sql-statements)
- [Database Design Best Practices](https://learn.microsoft.com/en-us/sql/relational-databases/tables/primary-and-foreign-key-constraints)
- [DbUp Advanced Scenarios](https://dbup.readthedocs.io/en/latest/more-info/advanced-scripts/)

[Move to Phase 13](https://github.com/entelect-incubator/.NET/tree/master/Phase%2013)

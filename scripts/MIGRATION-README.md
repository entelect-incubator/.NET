# MediatR → LiteBus Migration Scripts

**Purpose**: Automated migration from MediatR to LiteBus (CQS Pattern) across Phases 2-9

## Available Scripts

### 1. Enhanced Migration Script (Recommended)
**File**: `Migrate-MediatR-To-LiteBus-Enhanced.ps1`

**Features**:
- ✅ All 8 migration types
- ✅ Regex and literal string replacements
- ✅ Automatic backups
- ✅ Dry-run mode
- ✅ Detailed logging
- ✅ Phase selection

**Usage**:
```powershell
# Preview changes without making them
.\Migrate-MediatR-To-LiteBus-Enhanced.ps1 -DryRun -Verbose

# Migrate all phases (2-9)
.\Migrate-MediatR-To-LiteBus-Enhanced.ps1

# Migrate specific phases only
.\Migrate-MediatR-To-LiteBus-Enhanced.ps1 -Phases 2,3,4

# Skip backup creation
.\Migrate-MediatR-To-LiteBus-Enhanced.ps1 -SkipBackup
```

### 2. Basic Migration Script
**File**: `Migrate-MediatR-To-LiteBus.ps1`

Simpler version with core functionality for basic migrations.

## Migration Types

The scripts will automatically convert:

### 1. GlobalUsings.cs
```csharp
// Before
global using MediatR;

// After
global using LiteBus.Commands.Abstractions;
global using LiteBus.Queries.Abstractions;
```

### 2. DependencyInjection.cs
```csharp
// Before
services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<...>());

// After
services.AddLiteBus(cfg => cfg.RegisterServicesFromAssemblyContaining<...>());
```

### 3. Command Classes
```csharp
// Before
public class CreatePizzaCommand : IRequest<Result>

// After
public class CreatePizzaCommand : ICommand<Result>
```

### 4. CommandHandlers
```csharp
// Before
public class CreatePizzaCommandHandler : IRequestHandler<CreatePizzaCommand, Result>

// After
public class CreatePizzaCommandHandler : ICommandHandler<CreatePizzaCommand, Result>
```

### 5. Query Classes
```csharp
// Before
public class GetPizzasQuery : IRequest<List<PizzaModel>>

// After
public class GetPizzasQuery : IQuery<List<PizzaModel>>
```

### 6. QueryHandlers
```csharp
// Before
public class GetPizzasQueryHandler : IRequestHandler<GetPizzasQuery, List<PizzaModel>>

// After
public class GetPizzasQueryHandler : IQueryHandler<GetPizzasQuery, List<PizzaModel>>
```

### 7. ApiController
```csharp
// Before
using MediatR;
private IMediator mediator;
protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();

// After
using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
private ICommandMediator commandMediator;
private IQueryMediator queryMediator;
protected ICommandMediator CommandMediator => this.commandMediator ??= this.HttpContext.RequestServices.GetService<ICommandMediator>();
protected IQueryMediator QueryMediator => this.queryMediator ??= this.HttpContext.RequestServices.GetService<IQueryMediator>();
```

### 8. Package References
```xml
<!-- Before -->
<PackageReference Include="MediatR" Version="..." />

<!-- After (commented out) -->
<!-- MediatR removed: migrated to LiteBus -->
<!-- <PackageReference Include="MediatR" Version="..." /> -->
```

## Dry-Run Mode

Always test with dry-run first:

```powershell
.\Migrate-MediatR-To-LiteBus-Enhanced.ps1 -DryRun -Verbose
```

This will show all changes that would be made WITHOUT making them.

## Execution Steps

### Step 1: Run Dry-Run
```powershell
cd d:\Dev\Incubator\.NET\scripts
.\Migrate-MediatR-To-LiteBus-Enhanced.ps1 -DryRun -Verbose
```

### Step 2: Review Output
Check the log file at: `d:\Dev\Incubator\.NET\logs\migration-enhanced-*.log`

### Step 3: Execute Migration
```powershell
.\Migrate-MediatR-To-LiteBus-Enhanced.ps1 -Verbose
```

### Step 4: Verify Changes
- Open VS and reload solutions
- Build each phase
- Check git diff for changes

### Step 5: Run Tests
```powershell
cd "d:\Dev\Incubator\.NET\Phase 2\src\01. StartSolution"
dotnet test
```

## Backup & Recovery

Backups are created automatically before migration:
- Location: `d:\Dev\Incubator\.NET\backups\migration-[timestamp]`
- Skip backup: Use `-SkipBackup` flag

To restore from backup:
```powershell
Copy-Item -Path "d:\Dev\Incubator\.NET\backups\migration-20251030-120000\*" -Destination "d:\Dev\Incubator\.NET" -Recurse -Force
```

## Troubleshooting

### Issue: "File not found"
- Verify phases exist in `d:\Dev\Incubator\.NET\Phase X`
- Check phase directories structure

### Issue: "No replacements made"
- Might already be migrated
- Run script again to verify

### Issue: Build failures after migration
- Common causes:
  1. Missing LiteBus packages in .csproj
  2. Some handler inheritance might need adjustment
  3. Check logs for specific files

### Solution: Restore from Backup
```powershell
# Restore specific backup
$backupPath = "d:\Dev\Incubator\.NET\backups\migration-20251030-120000"
Get-ChildItem -Path $backupPath -Recurse | ForEach-Object {
    Copy-Item -Path $_ -Destination $_.FullName.Replace($backupPath, "d:\Dev\Incubator\.NET") -Force
}
```

## Progress Tracking

- [ ] Phase 2 Step 1
- [ ] Phase 2 Step 2
- [ ] Phase 3 Step 1
- [ ] Phase 3 Step 2
- [ ] Phase 4 Steps
- [ ] Phase 5 Steps
- [ ] Phase 6 Steps
- [ ] Phase 7 Steps
- [ ] Phase 8 Steps
- [ ] Phase 9 Steps

## Performance

Expected execution time:
- Dry-run: ~30-60 seconds
- Live migration: ~1-2 minutes
- Phases 2-9 combined: ~5 files/second

## Support

For issues or questions:
1. Check the log file
2. Verify file permissions
3. Ensure .NET SDK is installed
4. Run with `-Verbose` flag for details

## Notes

- Scripts are idempotent (safe to run multiple times)
- Backup strategy prevents data loss
- Detailed logging for debugging
- Phase-selective execution available

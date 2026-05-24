# Apply Phase 9 LiteBus Pattern to Other Phases

**Quick Guide**: Use the working Phase 9 implementation as the template for Phases 2-8

## The Pattern (From Phase 9)

### 1. GlobalUsings.cs

```csharp
// Remove this:
global using MediatR;

// Add these:
global using LiteBus.Commands.Abstractions;
global using LiteBus.Queries.Abstractions;
```

### 2. ApiController.cs

```csharp
// Before
using MediatR;
private IMediator mediator;
protected IMediator Mediator => this.mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();

// After
using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
private ICommandMediator _cmdMediator;
private IQueryMediator _qryMediator;
protected ICommandMediator CmdMediator => _cmdMediator ??= HttpContext.RequestServices.GetRequiredService<ICommandMediator>();
protected IQueryMediator QryMediator => _qryMediator ??= HttpContext.RequestServices.GetRequiredService<IQueryMediator>();
```

### 3. DependencyInjection.cs

Replace `services.AddMediatR()` with manual handler registration:

```csharp
// Old
services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateCustomerCommand>());

// New
var assembly = typeof(CreateCustomerCommand).Assembly;
var commandHandlerTypes = assembly.GetTypes()
    .Where(t => t.Name.EndsWith("CommandHandler") && !t.IsInterface && !t.IsAbstract)
    .ToList();
foreach (var handlerType in commandHandlerTypes) {
    services.AddScoped(handlerType);
}
var queryHandlerTypes = assembly.GetTypes()
    .Where(t => t.Name.EndsWith("QueryHandler") && !t.IsInterface && !t.IsAbstract)
    .ToList();
foreach (var handlerType in queryHandlerTypes) {
    services.AddScoped(handlerType);
}
```

### 4. Command Classes

```csharp
// Before
public class CreateCustomerCommand : IRequest<Result<CustomerModel>>

// After
public class CreateCustomerCommand : ICommand<Result<CustomerModel>>
```

### 5. CommandHandlers

```csharp
// Before
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerModel>>
{
    public async Task<Result<CustomerModel>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)

// After
public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, Result<CustomerModel>>
{
    public async Task<Result<CustomerModel>> HandleAsync(CreateCustomerCommand request, CancellationToken cancellationToken)
```

### 6. Query Classes

```csharp
// Before
public class GetCustomersQuery : IRequest<List<CustomerModel>>

// After
public class GetCustomersQuery : IQuery<List<CustomerModel>>
```

### 7. QueryHandlers

```csharp
// Before
public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerModel>>
{
    public async Task<List<CustomerModel>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)

// After
public class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, List<CustomerModel>>
{
    public async Task<List<CustomerModel>> HandleAsync(GetCustomersQuery request, CancellationToken cancellationToken)
```

## Run the Script

### Dry-Run (Preview Changes)

```powershell
cd d:\Dev\Incubator\.NET\scripts
.\Apply-Phase9-Pattern.ps1 -DryRun -Verbose
```

### Execute Migration (Phases 2-8)

```powershell
.\Apply-Phase9-Pattern.ps1
```

### Specific Phases Only

```powershell
.\Apply-Phase9-Pattern.ps1 -Phases 2,3,4
```


## What Gets Changed

| File Type              | Change                                                  | Count |
| ---------------------- | ------------------------------------------------------- | ----- |
| GlobalUsings.cs        | MediatR → LiteBus                                       | 12    |
| ApiController.cs       | IMediator → separated mediators                         | 8     |
| DependencyInjection.cs | AddMediatR() → manual registration                      | 8     |
| *Command.cs            | IRequest → ICommand                                     | 50+   |
| *CommandHandler.cs     | IRequestHandler → ICommandHandler, Handle → HandleAsync | 60+   |
| *Query.cs              | IRequest → IQuery                                       | 10+   |
| *QueryHandler.cs       | IRequestHandler → IQueryHandler, Handle → HandleAsync   | 15+   |

**Total Expected Changes**: 150+

## Verify Changes

After running the script:

```powershell
# Check for remaining MediatR references
grep -r "using MediatR" d:\Dev\Incubator\.NET\Phase*

# Build to verify
cd d:\Dev\Incubator\.NET\Phase 2\src\01. StartSolution
dotnet build

# Run tests
dotnet test
```

## Phase 9 Reference

Phase 9 is already fully migrated and working:

- Location: `d:\Dev\Incubator\.NET\Phase 9\src\01. StartSolution`
- Example files to compare:
  - `Api\Controllers\ApiController.cs`
  - `Core\DependencyInjection.cs`
  - `Core\Customer\Commands\CreateCustomerCommand.cs`

## Troubleshooting

### Issue: Some replacements didn't apply

- File might already be partially migrated
- Some patterns might use different formatting
- Run script again - it's idempotent

### Issue: Build fails after migration

1. Check for missing LiteBus packages in .csproj files
2. Verify all using statements are updated
3. Compare with Phase 9 working example

### Issue: Tests fail

- Handler method names changed from `Handle` → `HandleAsync`
- Verify all handler implementations use `HandleAsync`

## Next Steps

1. ✅ Run script with `-DryRun` first
2. ✅ Review proposed changes in log
3. ✅ Run script in LIVE mode
4. ✅ Build all phases
5. ✅ Run all tests
6. ✅ Commit changes

---

**Status**: Ready to execute
**Reference Phase**: Phase 9 (working LiteBus implementation)
**Script**: Apply-Phase9-Pattern.ps1

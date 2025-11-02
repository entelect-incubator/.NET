// LiteBus Startup Template
// Location: Program.cs in your API project
// This template shows all required DI configurations for LiteBus CQRS

using LiteBus;
using LiteBus.Extensions.FluentValidation;
using LiteBus.Extensions.Microsoft.Hosting;
using FluentValidation;
using Core;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// LITBUS CONFIGURATION - Replace MediatR setup with this
// ============================================================================

// Add LiteBus to the dependency injection container
builder.Services.AddLiteBus(config =>
{
    // Configure Command Module - registers all ICommand and ICommandHandler
    config.AddCommandModule(commandModuleBuilder =>
    {
        // Auto-register all commands and command handlers from this assembly
        // This will scan for:
        // - ICommand<TResult>
        // - ICommandHandler<TCommand, TResult>
        // - ICommandHandler<TCommand> (for commands with no result)
        commandModuleBuilder.RegisterFromAssembly(typeof(CreatePizzaCommand).Assembly);
        
        // Optional: Add multiple assemblies
        // commandModuleBuilder.RegisterFromAssembly(typeof(OrderCommand).Assembly);
    });
    
    // Configure Query Module - registers all IQuery and IQueryHandler
    config.AddQueryModule(queryModuleBuilder =>
    {
        // Auto-register all queries and query handlers from this assembly
        // This will scan for:
        // - IQuery<TResult>
        // - IQueryHandler<TQuery, TResult>
        queryModuleBuilder.RegisterFromAssembly(typeof(GetPizzasQuery).Assembly);
        
        // Optional: Add multiple assemblies
        // queryModuleBuilder.RegisterFromAssembly(typeof(GetOrdersQuery).Assembly);
    });
});

// ============================================================================
// FLUENT VALIDATION INTEGRATION
// ============================================================================

// Register FluentValidation validators
// LiteBus automatically integrates validators for command validation
builder.Services
    .AddValidatorsFromAssembly(typeof(CreatePizzaCommandValidator).Assembly);

// Optional: Add FluentValidation with automatic registration from nested types
// builder.Services.AddFluentValidationAutoValidation();

// ============================================================================
// COMMAND/QUERY PRE/POST HANDLERS (Optional - for cross-cutting concerns)
// ============================================================================

// If you have common behavior/logging across commands:
// Register ICommandPreHandler<T> implementations
builder.Services.AddTransient(typeof(ICommandPreHandler<>), typeof(CommandLoggingPreHandler<>));
builder.Services.AddTransient(typeof(ICommandPostHandler<>), typeof(CommandLoggingPostHandler<>));

// For queries:
// builder.Services.AddTransient(typeof(IQueryPreHandler<>), typeof(QueryLoggingPreHandler<>));
// builder.Services.AddTransient(typeof(IQueryPostHandler<>), typeof(QueryLoggingPostHandler<>));

// ============================================================================
// REMOVE THIS - Old MediatR Configuration (DELETE THESE LINES)
// ============================================================================

// DELETE: builder.Services.AddMediatR(cfg => 
//     cfg.RegisterServicesFromAssembly(typeof(CreatePizzaCommand).Assembly));
// 
// DELETE: builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
// DELETE: builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

// ============================================================================
// DATABASE & OTHER SERVICES
// ============================================================================

// Add Entity Framework Core
builder.Services.AddScoped<DatabaseContext>();

// Add Caching (keep as-is)
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IAppCache, AppCache>();

// Add AutoMapper (keep as-is)
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add other services as needed...

// ============================================================================
// BUILD & CONFIGURE MIDDLEWARE
// ============================================================================

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// ============================================================================
// HANDLER IMPLEMENTATION EXAMPLES
// ============================================================================

// ─────────────────────────────────────────────────────────────────────────
// COMMAND HANDLER PATTERN (with result)
// ─────────────────────────────────────────────────────────────────────────
//
// namespace Core.Pizza.Commands.Handlers;
// 
// using LiteBus.Commands.Abstractions;
//
// public class CreatePizzaCommandHandler(DatabaseContext db, IAppCache cache)
//     : ICommandHandler<CreatePizzaCommand, Result<PizzaModel>>
// {
//     public async Task<Result<PizzaModel>> HandleAsync(
//         CreatePizzaCommand command,
//         CancellationToken cancellationToken = default)
//     {
//         try
//         {
//             // Implementation
//             var pizza = new Pizza { Name = command.Data.Name, ... };
//             db.Pizzas.Add(pizza);
//             await db.SaveChangesAsync(cancellationToken);
//
//             // Invalidate cache
//             cache.Remove(CacheKeys.Pizzas);
//
//             return Result<PizzaModel>.Success(mapper.Map<PizzaModel>(pizza));
//         }
//         catch (Exception ex)
//         {
//             return Result<PizzaModel>.Failure(ex.Message);
//         }
//     }
// }

// ─────────────────────────────────────────────────────────────────────────
// COMMAND HANDLER PATTERN (no result)
// ─────────────────────────────────────────────────────────────────────────
//
// namespace Core.Pizza.Commands.Handlers;
//
// public class DeletePizzaCommandHandler(DatabaseContext db)
//     : ICommandHandler<DeletePizzaCommand>
// {
//     public async Task HandleAsync(
//         DeletePizzaCommand command,
//         CancellationToken cancellationToken = default)
//     {
//         var pizza = await db.Pizzas.FindAsync(new object[] { command.Id }, cancellationToken);
//         if (pizza != null)
//         {
//             db.Pizzas.Remove(pizza);
//             await db.SaveChangesAsync(cancellationToken);
//         }
//     }
// }

// ─────────────────────────────────────────────────────────────────────────
// QUERY HANDLER PATTERN
// ─────────────────────────────────────────────────────────────────────────
//
// namespace Core.Pizza.Queries.Handlers;
//
// using LiteBus.Queries.Abstractions;
//
// public class GetPizzasQueryHandler(DatabaseContext db, IAppCache cache)
//     : IQueryHandler<GetPizzasQuery, ListResult<PizzaModel>>
// {
//     public async Task<ListResult<PizzaModel>> HandleAsync(
//         GetPizzasQuery query,
//         CancellationToken cancellationToken = default)
//     {
//         var cacheKey = $"{CacheKeys.Pizzas}_{query.Data.PageIndex}_{query.Data.PageSize}";
//
//         // Try to get from cache
//         if (cache.Get<ListResult<PizzaModel>>(cacheKey) is { } cached)
//             return cached;
//
//         // Get from database
//         var pizzas = db.Pizzas
//             .FilterByName(query.Data.Name)
//             .ApplyPaging(query.Data.PagingArgs)
//             .ToList();
//
//         var result = new ListResult<PizzaModel> 
//         { 
//             Data = mapper.Map<List<PizzaModel>>(pizzas),
//             Total = db.Pizzas.Count()
//         };
//
//         // Cache for 10 minutes
//         cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
//
//         return result;
//     }
// }

// ─────────────────────────────────────────────────────────────────────────
// VALIDATOR PATTERN (FluentValidation)
// ─────────────────────────────────────────────────────────────────────────
//
// namespace Core.Pizza.Commands.Validators;
//
// using FluentValidation;
//
// public class CreatePizzaCommandValidator : AbstractValidator<CreatePizzaCommand>
// {
//     public CreatePizzaCommandValidator()
//     {
//         RuleFor(cmd => cmd.Data)
//             .NotNull()
//             .WithMessage("Pizza data is required");
//
//         RuleFor(cmd => cmd.Data.Name)
//             .NotEmpty()
//             .WithMessage("Pizza name is required")
//             .MaximumLength(100)
//             .WithMessage("Pizza name cannot exceed 100 characters");
//
//         RuleFor(cmd => cmd.Data.Price)
//             .GreaterThan(0)
//             .WithMessage("Price must be greater than 0");
//     }
// }

// ─────────────────────────────────────────────────────────────────────────
// COMMAND PRE-HANDLER (Optional - for logging/auditing)
// ─────────────────────────────────────────────────────────────────────────
//
// namespace Core.Handlers;
//
// using LiteBus.Commands.Abstractions;
// using ILogger = Microsoft.Extensions.Logging.ILogger;
//
// public class CommandLoggingPreHandler<TCommand>(ILogger<CommandLoggingPreHandler<TCommand>> logger)
//     : ICommandPreHandler<TCommand> where TCommand : ICommand
// {
//     public async Task PreHandleAsync(
//         TCommand command,
//         CancellationToken cancellationToken = default)
//     {
//         logger.LogInformation("Executing command: {CommandName}", typeof(TCommand).Name);
//         await Task.CompletedTask;
//     }
// }

// ─────────────────────────────────────────────────────────────────────────
// COMMAND POST-HANDLER (Optional - for notifications/events)
// ─────────────────────────────────────────────────────────────────────────
//
// namespace Core.Handlers;
//
// using LiteBus.Commands.Abstractions;
//
// public class CommandLoggingPostHandler<TCommand>(ILogger<CommandLoggingPostHandler<TCommand>> logger)
//     : ICommandPostHandler<TCommand> where TCommand : ICommand
// {
//     public async Task PostHandleAsync(
//         TCommand command,
//         CancellationToken cancellationToken = default)
//     {
//         logger.LogInformation("Command executed: {CommandName}", typeof(TCommand).Name);
//         await Task.CompletedTask;
//     }
// }

// ─────────────────────────────────────────────────────────────────────────
// CONTROLLER PATTERN - Updated DI
// ─────────────────────────────────────────────────────────────────────────
//
// namespace Api.Controllers;
//
// using LiteBus.Commands.Abstractions;
// using LiteBus.Queries.Abstractions;
// using Microsoft.AspNetCore.Mvc;
//
// [ApiController]
// [Route("api/[controller]")]
// public class PizzaController(
//     ICommandMediator cmdMediator,    // NEW: Replace IMediator with this
//     IQueryMediator qryMediator)      // NEW: Add this
//     : ControllerBase
// {
//     [HttpPost]
//     public async Task<ActionResult> Create([FromBody] CreatePizzaModel model)
//     {
//         var result = await cmdMediator.SendAsync(
//             new CreatePizzaCommand { Data = model });
//         
//         return ResponseHelper.ResponseOutcome(result, this);
//     }
//
//     [HttpGet]
//     public async Task<ActionResult> GetAll([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
//     {
//         var result = await qryMediator.QueryAsync(
//             new GetPizzasQuery 
//             { 
//                 Data = new SearchPizzaModel 
//                 { 
//                     PagingArgs = new PagingArgs { PageIndex = pageIndex, PageSize = pageSize }
//                 }
//             });
//
//         return ResponseHelper.ResponseOutcome(result, this);
//     }
//
//     [HttpPut("{id}")]
//     public async Task<ActionResult> Update(int id, [FromBody] UpdatePizzaModel model)
//     {
//         var result = await cmdMediator.SendAsync(
//             new UpdatePizzaCommand { Id = id, Data = model });
//         
//         return ResponseHelper.ResponseOutcome(result, this);
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<ActionResult> Delete(int id)
//     {
//         var result = await cmdMediator.SendAsync(
//             new DeletePizzaCommand { Id = id });
//         
//         return ResponseHelper.ResponseOutcome(result, this);
//     }
// }

// ============================================================================
// MIGRATION CHECKLIST
// ============================================================================
//
// ✅ BEFORE running this template:
// 1. [ ] Add LiteBus NuGet packages:
//        - LiteBus
//        - LiteBus.Extensions.FluentValidation
//        - LiteBus.Extensions.Microsoft.Hosting
//        - FluentValidation
//
// 2. [ ] In your Command/Query files:
//        - Change IRequest<T> → ICommand<T> (for commands)
//        - Change IRequest<T> → IQuery<T> (for queries)
//        - Update using MediatR; → using LiteBus.Commands.Abstractions;
//
// 3. [ ] In your Handler files:
//        - Change IRequestHandler<T, R> → ICommandHandler<T, R>
//        - Move handlers to Commands/Handlers/ subfolder
//        - Rename Handle() → HandleAsync()
//        - Update namespaces
//
// 4. [ ] In your Controller files:
//        - Replace IMediator with ICommandMediator, IQueryMediator
//        - Change mediator.Send() → cmdMediator.SendAsync()
//        - Change mediator.Send() → qryMediator.QueryAsync()
//
// 5. [ ] Copy this Program.cs template and adapt to your project
//
// 6. [ ] Update validators (already in CQRS pattern, just organize in 
//        Commands/Validators/ and Queries/Validators/ folders)
//
// 7. [ ] Test: dotnet run - should start without errors
//
// 8. [ ] Check for remaining IMediator references: grep -r "IMediator" .
//
// ✅ AFTER deployment:
// 1. [ ] Remove old MediatR packages
// 2. [ ] Remove IPipelineBehavior<,> registrations
// 3. [ ] Remove old ValidationBehavior, PerformanceBehaviour if not converted
// 4. [ ] Update API documentation/OpenAPI specs
// 5. [ ] Run full test suite
//
// ============================================================================

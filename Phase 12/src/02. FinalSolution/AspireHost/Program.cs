global using Aspire.Hosting;
using AspireHost;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder
    .AddSqlServer("sql-server", port: 1433)
    .AddDatabase("pezza-db");

// Add DbUp migrations project - runs first to ensure schema is ready
var migrations = builder
    .AddProject<DbUp_Migrations>("migrations")
    .WithReference(database);

var api = builder
    .AddProject<Api>("api")
    .WithReference(database)
    .WithOpenTelemetry()
    .WithHttpEndpoint(port: 5000, name: "http")
    .WithHttpsEndpoint(port: 5001, name: "https");

builder.Build().Run();

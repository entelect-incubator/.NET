global using Aspire.Hosting;
using AspireHost;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder
    .AddSqlServer("sql-server", port: 1433)
    .AddDatabase("pezza-db");

var api = builder
    .AddProject<Api>("api")
    .WithReference(database)
    .WithOpenTelemetry()
    .WithHttpEndpoint(port: 5000, name: "http")
    .WithHttpsEndpoint(port: 5001, name: "https");

builder.Build().Run();

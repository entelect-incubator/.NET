global using Aspire.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder
    .AddSqlServer("sql-server", port: 1433)
    .AddDatabase("pezza-db");

var api = builder
    .AddProject<Pezza_Api>("api")
    .WithReference(database)
    .WithHttpEndpoint(port: 5000, name: "http")
    .WithHttpsEndpoint(port: 5001, name: "https");

builder.Build().Run();

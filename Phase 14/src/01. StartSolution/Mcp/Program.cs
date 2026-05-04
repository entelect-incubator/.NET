namespace Pezza.Mcp;

using System.Text.Json;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pezza.Mcp.Protocol;
using Pezza.Mcp.Server;
using Pezza.Mcp.Tools;
using Serilog;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();

        builder.Services.AddSingleton(Log.Logger);

        var connectionString = builder.Configuration.GetConnectionString("PezzaDatabase")
            ?? builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No database connection string found. Use ConnectionStrings:PezzaDatabase.");

        builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));
        builder.Services.AddScoped<ToolHandlers>();
        builder.Services.AddScoped<McpServer>();

        using var host = builder.Build();

        Log.Information("Starting Pezza MCP Server...");
        Log.Information("MCP Server ready. Listening for requests on STDIO...");

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        while (true)
        {
            var line = await Console.In.ReadLineAsync();
            if (line is null)
            {
                break;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            McpResponse response;

            try
            {
                var request = JsonSerializer.Deserialize<McpRequest>(line, jsonOptions);
                if (request is null)
                {
                    throw new JsonException("Request is empty");
                }

                using var scope = host.Services.CreateScope();
                var server = scope.ServiceProvider.GetRequiredService<McpServer>();
                response = await server.HandleAsync(request);
            }
            catch (JsonException)
            {
                response = new McpResponse
                {
                    Id = default,
                    Error = new McpError
                    {
                        Code = -32700,
                        Message = "Parse error"
                    }
                };
            }

            var output = JsonSerializer.Serialize(response, jsonOptions);
            await Console.Out.WriteLineAsync(output);
            await Console.Out.FlushAsync();
        }

        return 0;
    }
}

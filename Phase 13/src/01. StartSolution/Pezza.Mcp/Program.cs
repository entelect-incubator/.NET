namespace Pezza.Mcp;

using System.Text;
using DataAccess;
using Server;
using Tools;
using Protocol;

class Program
{
    static async Task Main(string[] args)
    {
        // Setup logging
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .Enrich.WithProperty("ServiceName", "Pezza.MCP")
            .CreateLogger();

        Log.Information("Starting Pezza MCP Server...");

        try
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var services = new ServiceCollection()
                .AddLogging(builder => builder.AddSerilog())
                .AddDbContext<DatabaseContext>(options =>
                {
                    var connectionString = config.GetConnectionString("DefaultConnection");
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        throw new InvalidOperationException("DefaultConnection is not configured");
                    }
                    options.UseSqlServer(connectionString);
                })
                .AddScoped<IPizzaToolHandler, PizzaToolHandler>()
                .AddScoped<IOrderToolHandler, OrderToolHandler>()
                .AddScoped<IStockToolHandler, StockToolHandler>()
                .AddScoped<McpServer>()
                .BuildServiceProvider();

            using var scope = services.CreateScope();
            var mcpServer = scope.ServiceProvider.GetRequiredService<McpServer>();

            // Display available tools
            Log.Information("Available MCP Tools:");
            var tools = mcpServer.GetAvailableTools();
            foreach (var tool in tools)
            {
                Log.Information("  - {ToolName}: {ToolDescription}", tool.Name, tool.Description);
            }

            // Simple STDIO-based MCP server loop
            Log.Information("MCP Server ready. Listening for requests on STDIO...");

            string? line;
            while ((line = await Console.In.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    var request = JsonSerializer.Deserialize<McpRequest>(line);
                    if (request is null)
                    {
                        Console.WriteLine(JsonSerializer.Serialize(new
                        {
                            jsonrpc = "2.0",
                            error = new { code = -32700, message = "Parse error" }
                        }));
                        continue;
                    }

                    var response = await mcpServer.HandleRequestAsync(request);
                    Console.WriteLine(response);
                    Console.Out.Flush();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error processing request");
                    Console.WriteLine(JsonSerializer.Serialize(new
                    {
                        jsonrpc = "2.0",
                        error = new { code = -32603, message = $"Internal error: {ex.Message}" }
                    }));
                }
            }

            Log.Information("MCP Server shutting down");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "MCP Server failed");
            return 1;
        }

        return 0;
    }
}

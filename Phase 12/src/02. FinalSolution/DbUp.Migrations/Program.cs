namespace DbUp.Migrations;

using System.Linq;
using System.Reflection;
using DbUp.Engine.Output;
using Microsoft.Extensions.Configuration;
using Serilog;

class Program
{
    static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            var connectionString = args.FirstOrDefault() ?? GetConnectionStringFromConfig();

            Log.Information("Starting database migration...");
            Log.Information("Connection string: {ConnectionString}", MaskConnectionString(connectionString));

            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(
                    Assembly.GetExecutingAssembly(),
                    s => s.StartsWith("DbUp.Migrations.Scripts"))
                .LogScriptOutput()
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Log.Error(result.Error, "Database migration failed");
                return -1;
            }

            Log.Information("Database migration completed successfully");
            Log.Information("Scripts executed: {Count}", result.Scripts.Count());

            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Migration process failed");
            return -1;
        }
    }

    static string GetConnectionStringFromConfig()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        return config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No connection string found in configuration");
    }

    static string MaskConnectionString(string connectionString)
    {
        return System.Text.RegularExpressions.Regex.Replace(
            connectionString,
            @"(Password=)[^;]*",
            "$1****");
    }
}

namespace Pezza.Common
{
    using System;
    using Serilog;

    public static class Logging
    {
        public static void LogInfo(string name, object data)
        {
            Setup();
            Log.Information(name, data);
        }

        public static void LogException(Exception e)
        {
            Setup();
            Log.Fatal(e, "Exception");
        }

        private static void Setup()
        {
            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        }
    }
}
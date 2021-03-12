<img align="left" width="116" height="116" src="../pezza-logo.png" />

# &nbsp;**Pezza - Phase 4 - Step 2** [![.NET Core - Phase 4 - Step 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase4-step2.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase4-step2.yml)

<br/><br/>

Install Nuget Package Serilog.AspNetCore and Serilog.Sinks.File

![](2021-01-15-11-13-06.png)

Error handling

In Pezza.Common create Logging.cs

```cs
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

        private static void Setup() => Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File("logs\log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}
```

## **Global Error handling**

Modify ExceptionHandlerMiddleware.cs

Add Logging to HandleExceptionAsync function

```cs
else
{
    var code = HttpStatusCode.InternalServerError;
    var result = JsonConvert.SerializeObject(new { isSuccess = false, error = exception.Message });
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)code;
    Common.Logging.Logging.LogException(exception);

    return context.Response.WriteAsync(result);
}
```

With Serilog you can different hooks into i.e. Elastic etc.

Using a Static class for Logging makes it easier to add Logging and don't have to inject it before able to use it.

## **Move to Phase 5**

[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%205) 
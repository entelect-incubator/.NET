<img align="left" width="116" height="116" src="../Assets/logo.png" />

# &nbsp;**E List - Phase 4 - Step 2** [![.NET - Phase 4 - Step 2](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase4-step2.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase4-step2.yml)

<br/><br/><br/>

## Error handling

Install Nuget Package Serilog and Serilog.Sinks.File on all but the Test project.

![](./Assets/2021-01-15-11-13-06.png)

In the root of Common, create Logging.cs as per the following code snippet. Notice that Logging is a static class. This makes it easy to use in any calling code without the need of injecting it.

```cs
namespace Common;

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
        .WriteTo.File(@"logs\log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
}
```

Modify UnhandledExceptionBehaviour.cs in COre Project by logging an exception in the final else of the Handle Method. All exceptions that we have not defined behaviour for gets handled here.

```cs
catch (Exception ex)
{
	Logging.LogException(ex);
	await HandleUnhandledExceptionAsync(httpContext, ex);
}
```

Update PerformanceBehaviour.cs by removing the old logging and using the new static Logging class instead.

```cs
namespace Core.Behaviours;

using System.Diagnostics;
using Common;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
	private readonly Stopwatch timer = new();

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		this.timer.Restart();

		var response = await next();

		this.timer.Stop();

		var elapsedMilliseconds = this.timer.ElapsedMilliseconds;

		if (elapsedMilliseconds > 500)
		{
			var requestName = typeof(TRequest).Name;
			Logging.LogInfo($"Long Running Request: {requestName} ({elapsedMilliseconds} milliseconds)", request);
		}

		return response;
	}
}
```

Serilog provides sinks for writing log events to storage in various formats. Read more on [provided sinks](https://github.com/serilog/serilog/wiki/Provided-Sinks) or move on to the next phase.

## **Move to Phase 5**

[Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%205)

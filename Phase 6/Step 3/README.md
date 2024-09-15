<img align="left" width="116" height="116" src="../Assets/logo.png" />

# &nbsp;**E List - Phase 6 - Step 3** [![.NET - Phase 6 - Step 3](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step3.yml/badge.svg)](https://github.com/entelect-incubator/.NET/actions/workflows/dotnet-phase6-step3.yml)

<br/><br/>

## **Schedule Background Jobs**

**Disclaimer**: This solution is effective for on-premises or virtual hosting environments. However, transitioning to a Cloud Native environment may introduce certain complexities. If you are utilizing AWS, it is recommended to employ AWS Step Functions, or if you are on Azure, Azure Logic Apps are a suitable choice. If these options are not feasible, consider exploring infrastructure options that support long-running applications such as AWS Fargate or Azure Containers.

<br/>
<hr/>
<br/>

[Hangfire](https://www.hangfire.io/) provides an easy way to perform background processing in .NET and .NET applications. No Windows Service or separate process is required.

The benefit of using Hangfire is that it comes with a Dashboard. [Hangfire Dashboard](https://docs.hangfire.io/en/latest/configuration/using-dashboard.html) is a place where you could find all the information about your background jobs. It is written as an OWIN middleware, so you can plug it into your ASP.NET, ASP.NET MVC, Nancy, ServiceStack application as well as use the OWIN Self-Host feature to host Dashboard inside console applications or in Windows Services.

General problems with relying on the external systems are fire and forget or performance. You send a request out, but what happens if it fails or times out? Do you impact the system by waiting for this request to be sent?

Read more on [Fire and Forget Pattern](https://ducmanhphan.github.io/2020-02-24-fire-and-forget-pattern/).

## Solution...

Adding the request on a type of queue system and forgetting about the result. You can also implement a webhook/event when the request is done. In this Step, we will only look at adding on the Notify table and using the Hangfire job to read any emails that haven't been sent.

## **Scheduler**

Create a new ASP.NET Web Application Scheduler under 01. Apis

![](./Assets/2023-07-23-08-49-01.png)

![](./Assets/2024-09-15-23-33-02.png)

## **Nuget Packages**

Once the project is ready, I will open the NuGet package manager UI and add the necessary NuGet packages. The three main Nuget packages needed for hangfire are:

-   [ ] Hangfire.Core – The core package that supports the core logic of Hangfire
-   [ ] Hangfire.AspNetCore – Support for ASP.NET Middleware and Middleware for the dashboard user interface
-   [ ] Hangfire.InMemory - In-memory job storage for Hangfire with transactional implementation.
-   [ ] Microsoft.Extensions.DependencyInjection
-   [ ] Microsoft.AspNetCore.Mvc.NewtonsoftJson
-   [ ] LazyCache.AspNetCore

Remove WeatherForecast.cs and WeatherForecastController.cs

Copy Program.cs, GlobalUsings.cs and Startup.cs from Api project

## **Configuring Hangfire**

Once all the NuGet packages are installed, it is time to configure the server. To do that, I will open the Startup.cs file and update the ConfigureServices method of the Startup class.

Program.cs

```cs
var builder = WebApplication.CreateBuilder(args);
var startup = new Api.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services); // calling ConfigureServices method
var app = builder.Build();
startup.Configure(app, builder.Environment); // calling Configure method
```

```cs
services.AddHangfire(config =>
			config
				.UseSimpleAssemblyNameTypeSerializer()
				.UseDefaultTypeSerializer()
				.UseInMemoryStorage());
		services.AddHangfireServer();
```

In the Project properties remember to turn XML Documentation on

![](./Assets/2023-07-23-09-12-15.png)

In the above code, the AddHangfire method takes an Action delegate, which passes IGlobalConfiguration of the Hangfire ecosystem to configure the Hangfire.

Next, I will call the UseSimpleAssemblyNameTypeSerializer and UseDefaultTypeSerializer one after the other, to set serialization configuration.

Finally, I will call UseInMemoryStorage and will use InMemory for this Incubator.

Next, I will call the extension method AddHangfireServer on the IServiceCollection instance to add the Hangfire server to the dependency injection container. Which we will use later to configure and run jobs.

## **Dashboard**

Once the basic setup for the dependency injection container is done, now I will add the middleware needed to add the Hangfire Dashboard UI. For that, I will call the extension method UseHangfireDashboard on the IApplicationBuilder instance in the Configure method of the Scheduler StartUp class.

```cs
app.UseHangfireDashboard();
```

Add Common Project to Scheduler Project

Final Startup.cs

Startup.cs

```cs
namespace Api;

using System.Reflection;
using System.Text.Json.Serialization;
using Core;
using Core.Behaviours;
using DataAccess;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Scheduler.Jobs;

public class Startup
{
	public Startup(IConfiguration configuration) => this.ConfigRoot = configuration;

	public IConfiguration ConfigRoot
	{
		get;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddHangfire(config =>
			config
				.UseSimpleAssemblyNameTypeSerializer()
				.UseDefaultTypeSerializer()
				.UseInMemoryStorage());
		services.AddHangfireServer();

		services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
			.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
			.AddNewtonsoftJson(x => x.SerializerSettings.ContractResolver = new DefaultContractResolver())
			.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

		DependencyInjection.AddApplication(services);
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "EList API",
				Version = "v1"
			});

			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			c.IncludeXmlComments(xmlPath);
		});

		services.AddLazyCache();
		services.AddDbContext<DatabaseContext>(options =>
			options.UseInMemoryDatabase("EListDB"));

		services.AddResponseCompression(options =>
		{
			options.Providers.Add<BrotliCompressionProvider>();
			options.Providers.Add<GzipCompressionProvider>();
		});
		services.AddResponseCompression();

		services.AddScoped<IOrderCompleteJob, OrderCompleteJob>();
	}

	public void Configure(WebApplication app, IWebHostEnvironment env)
	{
		app.UseSwagger();
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EList Scheduler API V1"));
		app.UseHttpsRedirection();
		app.UseMiddleware(typeof(UnhandledExceptionBehaviour));
		app.UseRouting();
		app.UseEndpoints(endpoints => endpoints.MapControllers());
		app.UseAuthorization();
		app.UseResponseCompression();
		app.UseHangfireDashboard();

		var jobOptions = new RecurringJobOptions()
		{
			TimeZone = TimeZoneInfo.Local
		};
		RecurringJob.AddOrUpdate<IOrderCompleteJob>("SendNotificationAsync", x => x.SendNotificationAsync(), "* * * * *");

		app.Run();
	}
}
```

## **Running the Scheduler**

Set Scheduler as Startup Project

Now, the above job will just print Run Hangfire job while it's hot! to the console output.

I will run the application to see the output as well as the Hangfire dashboard UI. To access the dashboard UI, we will navigate to the resource /hangfire.

![](./Assets/2023-07-23-09-13-16.png)

## **Create a recurring job**

Creating a background job as we did above is easy with Hangfire, but it is as easy using an instance of Task class as well. So why go with something like Hangfire and install all these packages into the project?

Well, the main advantage of Hangfire comes in when we use it to create scheduling jobs. It uses CRON expressions for scheduling.

Let us say we need to create a job that is responsible for finding any emails in Notify table that hasn't been send and send them out.

Create a new folder called Jobs inside Scheduler and inside that IEmailJob.cs interface and EmailJob.cs.

```cs
namespace Scheduler.Jobs;

using System.Threading.Tasks;
using Core.Todos.Commands;
using MediatR;

public interface IEmailJob
{
	Task SendAsync(CancellationToken cancellationToken = default);
}

public sealed class EmailJob(IMediator mediator) : IEmailJob
{
	public async Task SendAsync(CancellationToken cancellationToken = default)
		=> await mediator.Send(new ExpiredTodoCommand() { ToEmail = "fakeemail@test.com" }, cancellationToken);
}
```

## **Configure Startup class**

Once the EmailJob class is created, it is time to configure the Startup class. In the Startup class, the objective is to configure a recurring job to call the SendNotificationAsync method every minute.

Firstly, I will add the EmailJob to the dependency injection container in the ConfigureServices method.

At the end of ConfigureServices in Startup.cs

```cs
services.AddScoped<IEmailJob, EmailJob>();
```

Secondly, I will update the Configure method to take two new parameters. The first one is the IRecurringJobManager necessary for creating a recurring job. And the second one is the IServiceProvider to get the IOrderCompleteJob instance from the dependency injection container.

Thirdly, I will call the AddOrUpdate on the RecurringJobManager instance to set up a recurring job at the end of Configure method in Startup.cs.

```cs
var jobOptions = new RecurringJobOptions()
{
	TimeZone = TimeZoneInfo.Local
};
RecurringJob.AddOrUpdate<IEmailJob>("SendAsync", x => x.SendAsync(), "0 * * ? * *");
```

In the above code, the CRON expression "\* \* \* \* \*" is an expression to run the job every minute.

![](./Assets/2023-07-23-13-31-35.png)

![](./Assets/2023-07-23-13-32-48.png)

## **Step 4 - Orders**

Move to Phase 7 Step 1 [Click Here](https://github.com/entelect-incubator/.NET/tree/master/Phase%207/Step%201)

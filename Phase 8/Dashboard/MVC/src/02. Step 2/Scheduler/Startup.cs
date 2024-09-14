namespace Api;

using System.Reflection;
using System.Text.Json.Serialization;
using Common.Behaviour;
using Core;
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
		app.UseMiddleware(typeof(ExceptionHandlerMiddleware));
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

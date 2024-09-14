namespace Api;

using System.Reflection;
using System.Text.Json.Serialization;
using Common.Behaviour;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

public class Startup
{
	public Startup(IConfiguration configuration) => this.ConfigRoot = configuration;

	public IConfiguration ConfigRoot
	{
		get;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
			.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
			.AddNewtonsoftJson(x => x.SerializerSettings.ContractResolver = new DefaultContractResolver())
			.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

		DependencyInjection.AddApplication(services);

		services.AddSwaggerDocument(config =>
		{
			config.GenerateEnumMappingDescription = true;
			config.PostProcess = document =>
			{
				document.Info.Version = "V1";
				document.Info.Title = "EList Api";
			};
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
		using (var serviceProvider = services.BuildServiceProvider())
		{
			var dbContext = serviceProvider.GetRequiredService<DatabaseContext>();
			dbContext.Database.EnsureCreated();
			dbContext.SaveChanges();
			dbContext.Dispose();
		}
	}

	public void Configure(WebApplication app, IWebHostEnvironment env)
	{
		app.UseOpenApi();
		app.UseSwaggerUi3(c => c.AdditionalSettings.Add("displayRequestDuration", true));
		app.UseHttpsRedirection();
		app.UseMiddleware(typeof(ExceptionHandlerMiddleware));
		app.UseRouting();
		app.UseEndpoints(endpoints => endpoints.MapControllers());
		app.UseAuthorization();
		app.UseResponseCompression();
		app.Run();
	}
}

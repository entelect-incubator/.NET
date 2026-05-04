namespace Api;

using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Newtonsoft.Json.Serialization;

/// <summary>
/// Startup configuration for the Pezza API application.
/// Configures services, dependency injection, database context, and middleware pipeline.
/// Phase 2 focuses on laying the foundation for LiteBus-based CQRS in future phases.
/// </summary>
public class Startup(IConfiguration configuration)
{
	private readonly IConfiguration _configuration = configuration;

	/// <summary>
	/// Configures services for the application.
	/// Sets up controllers, JSON serialization, Swagger, Entity Framework, and dependency injection.
	/// </summary>
	/// <param name="services">The service collection to configure</param>
	public void ConfigureServices(IServiceCollection services)
	{
		// Configure controllers with JSON serialization options
		services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
			.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
			.AddNewtonsoftJson(x => x.SerializerSettings.ContractResolver = new DefaultContractResolver())
			.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

		// Add application services
		DependencyInjection.AddApplication(services);

		// Configure Swagger documentation generation
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "Pezza API",
				Version = "v1"
			});

			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			c.IncludeXmlComments(xmlPath);
		});

		// Configure Entity Framework Core with in-memory database
		services.AddDbContext<DatabaseContext>(options =>
			options.UseInMemoryDatabase(Guid.NewGuid().ToString())
		);
	}

	/// <summary>
	/// Configures the HTTP request pipeline and middleware.
	/// Sets up Swagger, HTTPS redirection, routing, and controller endpoints.
	/// </summary>
	/// <param name="app">The web application builder</param>
	/// <param name="env">The hosting environment</param>
	public void Configure(WebApplication app, IWebHostEnvironment env)
	{
		app.UseSwagger();
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pezza API V1"));
		app.UseHttpsRedirection();
		app.UseRouting();
		app.MapControllers();
		app.UseAuthorization();
		app.Run();
	}
}

namespace Api;

using System.Reflection;
using System.Text.Json.Serialization;
using Core.Behaviours;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Newtonsoft.Json.Serialization;

#pragma warning disable SA1516
public class Startup
{
	public Startup(IConfiguration configuration)
	{
		this.ConfigRoot = configuration;
	}

	public IConfiguration ConfigRoot { get; }

	/// <summary>
	/// Configures services for the API.
	/// </summary>
	/// <param name="services">The service collection.</param>
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddResponseCompression(options =>
		{
			options.Providers.Add<BrotliCompressionProvider>();
			options.Providers.Add<GzipCompressionProvider>();
		});

		services.AddResponseCompression();

		services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
		.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
		.AddNewtonsoftJson(x => x.SerializerSettings.ContractResolver = new DefaultContractResolver())
		.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

		DependencyInjection.AddApplication(services);

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

		services.AddDbContext<DatabaseContext>(options =>
		options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
	}

	/// <summary>
	/// Configures the HTTP request pipeline.
	/// </summary>
	/// <param name="app">The application builder.</param>
	/// <param name="env">The hosting environment.</param>
	public void Configure(WebApplication app, IWebHostEnvironment env)
	{
		app.UseMiddleware<UnhandledExceptionBehaviour>();
		app.UseResponseCompression();
		app.UseSwagger();
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pezza API V1"));
		app.UseHttpsRedirection();
		app.UseRouting();
		app.MapControllers();
		app.UseAuthorization();
		app.Run();
	}
}
#pragma warning restore SA1516

namespace Api;

using System.Reflection;
using System.Text.Json.Serialization;
<<<<<<<< HEAD:Phase 4/src/03. Step2/Api/Startup.cs
========
using Core.Behaviours;
>>>>>>>> master:Phase 8/src/02. MVC/Api/Startup.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Newtonsoft.Json.Serialization;

public class Startup(IConfiguration configuration)
{
	public IConfiguration ConfigRoot
	{
		get;
	} = configuration;

	public Startup(IConfiguration configuration) => this.ConfigRoot = configuration;

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
<<<<<<<< HEAD:Phase 4/src/03. Step2/Api/Startup.cs

		// Modern exception handling using .NET 8+ IExceptionHandler pattern
		// Order matters: specific handlers (ValidationExceptionHandler) before generic ones (GlobalExceptionHandler)
		services.AddExceptionHandler<Handlers.ValidationExceptionHandler>();
		services.AddExceptionHandler<Handlers.GlobalExceptionHandler>();
		services.AddProblemDetails();

========
		services.AddOpenApiDocument();
>>>>>>>> master:Phase 8/src/02. MVC/Api/Startup.cs
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

		services.AddDbContext<DatabaseContext>(options =>
<<<<<<<< HEAD:Phase 4/src/03. Step2/Api/Startup.cs
			options.UseInMemoryDatabase(Guid.NewGuid().ToString())
========
			options.UseInMemoryDatabase("EListDB")
>>>>>>>> master:Phase 8/src/02. MVC/Api/Startup.cs
		);
	}

	public void Configure(WebApplication app, IWebHostEnvironment env)
	{
		app.UseMiddleware<UnhandledExceptionBehaviour>();
		app.UseResponseCompression();
		app.UseSwagger();
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EList API V1"));
		app.UseHttpsRedirection();
<<<<<<<< HEAD:Phase 4/src/03. Step2/Api/Startup.cs
		// Exception handling is now handled by the IExceptionHandler pipeline
		// The new handlers are registered in ConfigureServices
========
>>>>>>>> master:Phase 8/src/02. MVC/Api/Startup.cs
		app.UseRouting();
		app.MapControllers();
		app.UseAuthorization();
		app.Run();
	}
}

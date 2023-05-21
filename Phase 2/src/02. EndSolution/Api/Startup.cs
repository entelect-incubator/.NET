namespace Api;

using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

public class Startup
{
	public IConfiguration configRoot
	{
		get;
	}

	public Startup(IConfiguration configuration) => configRoot = configuration;

	public void ConfigureServices(IServiceCollection services)
	{
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
			options.UseInMemoryDatabase(Guid.NewGuid().ToString())
		);
	}

	public void Configure(WebApplication app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		// Enable middleware to serve generated Swagger as a JSON endpoint.
		app.UseSwagger();
		//// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
		//// specifying the Swagger JSON endpoint.
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pezza API V1"));

		app.UseHttpsRedirection();
		app.UseRouting();

		app.Run();
	}
}

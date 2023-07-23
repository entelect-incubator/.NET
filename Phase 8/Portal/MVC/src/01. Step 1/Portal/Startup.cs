namespace Api;

using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
	public Startup(IConfiguration configuration)
	{
		this.ConfigRoot = configuration;
		this.ConfigRoot.Bind("AppSettings", new AppSettings());
	}

	public IConfiguration ConfigRoot
	{
		get;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddRazorPages();
		services.AddHttpClient();
		services.AddResponseCompression(options =>
		{
			options.Providers.Add<BrotliCompressionProvider>();
			options.Providers.Add<GzipCompressionProvider>();
		});
		services.AddResponseCompression();
	}

	public void Configure(WebApplication app, IWebHostEnvironment env)
	{
		app.UseHttpsRedirection();
		app.UseStaticFiles();
		app.UseMiddleware(typeof(ExceptionHandlerMiddleware));
		app.UseRouting();
		app.UseAuthorization();
		app.MapRazorPages();
		app.UseResponseCompression();
		app.Run();
	}
}

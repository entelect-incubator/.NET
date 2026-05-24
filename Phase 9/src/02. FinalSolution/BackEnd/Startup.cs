namespace Portal;

using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Common;
using Portal.Helpers;

public class Startup
{
    public Startup(IHostEnvironment env, IConfiguration configuration)
    {
        this.Configuration = configuration;

        this.Configuration = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        this.Configuration.Bind("AppSettings", new AppSettings());
    }

    public IConfiguration Configuration { get; }

    public IHostEnvironment CurrentEnvironment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });
        services.AddResponseCompression();

        services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.HttpOnly = HttpOnlyPolicy.Always;
            options.Secure = CookieSecurePolicy.Always;
            options.MinimumSameSitePolicy = SameSiteMode.Strict;
        });

        services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
        services.AddControllersWithViews();
        services.AddMvc(options =>
        {
            options.Filters.Add(typeof(ValidateModelStateAttribute));
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler(errorApp => errorApp.Run(async context => await this.ErrorHandling(context)));

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseCookiePolicy();
        app.Use(async (context, next) =>
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-Frame-Options"] = "DENY";
            context.Response.Headers["Referrer-Policy"] = "no-referrer";
            await next();
        });
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }

    private async Task<HttpContext> ErrorHandling(HttpContext context)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "plain/text";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        await context.Response.WriteAsync($"An error occurred - {exceptionHandlerPathFeature?.Error?.Message}\r\n");

        return context;
    }
}

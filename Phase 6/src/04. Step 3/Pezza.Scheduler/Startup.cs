namespace Pezza.Scheduler
{
    using System;
    using Hangfire;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Pezza.Common.Behaviours;
    using Pezza.Core;
    using Pezza.DataAccess;
    using Pezza.DataAccess.Contracts;
    using Pezza.Scheduler.Jobs;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseDefaultTypeSerializer()
                    .UseSqlServerStorage(this.Configuration.GetConnectionString("PezzaDatabase")));

            services.AddDbContext<DatabaseContext>(options =>
               options.UseSqlServer(this.Configuration.GetConnectionString("PezzaDatabase")));

            services.AddSingleton<IOrderCompleteJob, OrderCompleteJob>();

            DependencyInjection.AddApplication(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        {
            recurringJobManager.AddOrUpdate(
                "Run every minute",
                () => serviceProvider.GetService<IOrderCompleteJob>().SendNotificationAsync(),
                "* * * * *"
                );

            app.UseHangfireDashboard();
            app.UseMiddleware(typeof(ExceptionHandlerMiddleware));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}

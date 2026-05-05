namespace UI;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddSession(options =>
		{
			options.IdleTimeout = TimeSpan.FromMinutes(20); 
			options.Cookie.HttpOnly = true;
			options.Cookie.IsEssential = true;
		});
		
		builder.Services.AddHttpClient();
		builder.Services.AddControllersWithViews();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseRouting();
		app.UseSession();
		app.UseAuthorization();

		app.MapStaticAssets();
		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}")
			.WithStaticAssets();

		app.Run();
	}
}

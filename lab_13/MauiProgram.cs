using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using TaxiParkAppMobile1.Data;
using TaxiParkAppMobile1.Pages;
using TaxiParkAppMobile1.Services;

namespace TaxiParkAppMobile1;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "taxipark.db");

		builder.Services.AddDbContextFactory<TaxiParkDbContext>(options =>
			options.UseSqlite($"Filename={dbPath}"));

		builder.Services.AddSingleton<DbInitializer>();
		builder.Services.AddSingleton<TaxiDataService>();

		builder.Services.AddTransient<DriversPage>();
		builder.Services.AddTransient<CarsPage>();
		builder.Services.AddTransient<TripsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();

		var initializer = app.Services.GetRequiredService<DbInitializer>();
		initializer.InitializeAsync().GetAwaiter().GetResult();

		return app;
	}
}

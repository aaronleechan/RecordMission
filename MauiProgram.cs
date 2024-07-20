using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace RecordMission;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureSyncfusionCore()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		#if DEBUG
				builder.Logging.AddDebug();
		#endif

		builder.Services.AddSingleton<BaseRepository<Mission>>();
		builder.Services.AddSingleton<BaseRepository<Record>>();

		return builder.Build();
	}
}

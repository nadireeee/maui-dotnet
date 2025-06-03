using Microsoft.Extensions.Logging;
using MovieApp.Maui.Services;
using MovieApp.Maui.ViewModels;
using MovieApp.Maui.Views;
using CommunityToolkit.Maui;

namespace MovieApp.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// Register services
		builder.Services.AddSingleton<ApiService>();

		// ViewModels
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<RegisterViewModel>();
		builder.Services.AddTransient<ProfileViewModel>();
		builder.Services.AddTransient<ForgotPasswordViewModel>();
		builder.Services.AddTransient<MovieListViewModel>();
		builder.Services.AddTransient<MovieDetailViewModel>();
		builder.Services.AddTransient<FavoriteMoviesViewModel>();

		// Pages
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<RegisterPage>();
		builder.Services.AddTransient<ForgotPasswordPage>();
		builder.Services.AddTransient<ProfilePage>();
		builder.Services.AddTransient<MovieListPage>();
		builder.Services.AddTransient<MovieDetailPage>();
		builder.Services.AddTransient<FavoriteMoviesPage>();

		return builder.Build();
	}
}

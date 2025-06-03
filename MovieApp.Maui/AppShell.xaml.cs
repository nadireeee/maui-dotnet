using MovieApp.Maui.Views;
using Microsoft.Maui.Controls;

namespace MovieApp.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register all routes
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
		Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
		Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
		Routing.RegisterRoute(nameof(MovieListPage), typeof(MovieListPage));
		Routing.RegisterRoute(nameof(MovieDetailPage), typeof(MovieDetailPage));
		Routing.RegisterRoute(nameof(FavoriteMoviesPage), typeof(FavoriteMoviesPage));

		// Configure navigation
		Shell.SetNavBarIsVisible(this, true);
		Shell.SetTabBarIsVisible(this, true);

		// Add navigation events
		this.Navigating += OnNavigating;
		this.Navigated += OnNavigated;
	}

	private void OnNavigating(object sender, ShellNavigatingEventArgs e)
	{
		// Navigation başlamadan önce yapılacak işlemler
		// Örneğin: Yetkilendirme kontrolü, yükleme göstergesi
	}

	private void OnNavigated(object sender, ShellNavigatedEventArgs e)
	{
		// Navigation tamamlandıktan sonra yapılacak işlemler
		// Örneğin: Sayfa başlığını güncelleme, analitik gönderme
	}
}

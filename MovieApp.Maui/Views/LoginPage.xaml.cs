using MovieApp.Maui.ViewModels;
using MovieApp.Maui.Services;
using System.Diagnostics;

namespace MovieApp.Maui.Views;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService;

    public LoginPage(LoginViewModel viewModel, ApiService apiService)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _apiService = apiService;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    private async void OnTestConnectionClicked(object sender, EventArgs e)
    {
        try
        {
            Debug.WriteLine("Testing API connection...");
            var response = await _apiService.TestConnectionAsync();
            await DisplayAlert("Connection Test", response ? "API connection successful!" : "API connection failed!", "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Connection test error: {ex.Message}");
            await DisplayAlert("Connection Error", $"Error connecting to API: {ex.Message}", "OK");
        }
    }
}

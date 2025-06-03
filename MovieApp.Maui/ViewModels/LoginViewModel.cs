using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MovieApp.Maui.Models;
using MovieApp.Maui.Services;
using System.Diagnostics;

namespace MovieApp.Maui.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private bool _isBusy;
    private string _errorMessage = string.Empty;

    public LoginViewModel(ApiService apiService)
    {
        _apiService = apiService;
        LoginCommand = new Command(OnLoginClicked);
        GoToRegisterCommand = new Command(OnGoToRegisterClicked);
        ForgotPasswordCommand = new Command(OnForgotPasswordClicked);
    }

    public string Username
    {
        get => _username;
        set
        {
            if (_username != value)
            {
                _username = value;
                OnPropertyChanged();
            }
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (_password != value)
            {
                _password = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy != value)
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            if (_errorMessage != value)
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand LoginCommand { get; }
    public ICommand GoToRegisterCommand { get; }
    public ICommand ForgotPasswordCommand { get; }

    private async void OnLoginClicked()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Username and password are required.";
                return;
            }

            Debug.WriteLine($"Login attempt started for user: {Username}");
            var user = await _apiService.LoginAsync(Username, Password);
            
            if (user != null)
            {
                Debug.WriteLine($"Login successful for user: {Username}");
                // Redirect to MovieListPage after successful login
                await Shell.Current.GoToAsync("//MovieListPage");
            }
            else
            {
                Debug.WriteLine($"Login failed for user: {Username}");
                ErrorMessage = "Login failed. Please check your credentials.";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Login error in ViewModel: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            ErrorMessage = $"An error occurred during login: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnGoToRegisterClicked()
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }

    private async void OnForgotPasswordClicked()
    {
        await Shell.Current.GoToAsync("//ForgotPasswordPage");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 
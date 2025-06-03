using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MovieApp.Maui.Models;
using MovieApp.Maui.Services;

namespace MovieApp.Maui.ViewModels;

public class RegisterViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private string _username = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _confirmPassword = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isBusy;

    public RegisterViewModel(ApiService apiService)
    {
        _apiService = apiService;
        RegisterCommand = new Command(OnRegisterClicked);
        GoToLoginCommand = new Command(OnGoToLoginClicked);
    }

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    public ICommand RegisterCommand { get; }
    public ICommand GoToLoginCommand { get; }

    private async void OnRegisterClicked()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            // Validate input
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || 
                string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorMessage = "All fields are required.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            if (Password.Length < 6)
            {
                ErrorMessage = "Password must be at least 6 characters long.";
                return;
            }

            if (!Email.Contains("@"))
            {
                ErrorMessage = "Please enter a valid email address.";
                return;
            }

            // Create user object
            var user = new User
            {
                Username = Username,
                Email = Email,
                Password = Password // Note: In a real app, this should be hashed
            };

            // Register user
            var registeredUser = await _apiService.RegisterAsync(user);
            if (registeredUser != null)
            {
                await Shell.Current.DisplayAlert("Success", "Registration successful! Please login.", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                ErrorMessage = "Registration failed. Please try again.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"An error occurred: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnGoToLoginClicked()
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MovieApp.Maui.Services;

namespace MovieApp.Maui.ViewModels;

public class ForgotPasswordViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private string _email = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isBusy;

    public ForgotPasswordViewModel(ApiService apiService)
    {
        _apiService = apiService;
        SendResetLinkCommand = new Command(OnSendResetLinkClicked);
        GoToLoginCommand = new Command(OnGoToLoginClicked);
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

    public ICommand SendResetLinkCommand { get; }
    public ICommand GoToLoginCommand { get; }

    private async void OnSendResetLinkClicked()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Please enter your email address.";
                return;
            }

            if (!Email.Contains("@"))
            {
                ErrorMessage = "Please enter a valid email address.";
                return;
            }

            var result = await _apiService.ForgotPasswordAsync(Email);
            if (result)
            {
                await Shell.Current.DisplayAlert("Success", "Password reset link has been sent to your email.", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                ErrorMessage = "Failed to send reset link. Please try again.";
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
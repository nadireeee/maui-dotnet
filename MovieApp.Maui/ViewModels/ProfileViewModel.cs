using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MovieApp.Maui.Models;
using MovieApp.Maui.Services;
using System.Windows.Input;
using System.Diagnostics;

namespace MovieApp.Maui.ViewModels;

public class ProfileViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private User? _user;
    private ObservableCollection<Movie> _favoriteMovies = new();
    private bool _isBusy;
    private string _errorMessage = string.Empty;
    private string _username = string.Empty;
    private string _email = string.Empty;

    public ProfileViewModel(ApiService apiService)
    {
        _apiService = apiService;
        EditProfileCommand = new Command(OnEditProfileClicked);
        ChangePasswordCommand = new Command(OnChangePasswordClicked);
        ForgotPasswordCommand = new Command(OnForgotPasswordClicked);
        LogoutCommand = new Command(OnLogoutClicked);
        LoadUserData();
    }

    public User? User
    {
        get => _user;
        set
        {
            if (_user != value)
            {
                _user = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Movie> FavoriteMovies
    {
        get => _favoriteMovies;
        set
        {
            if (_favoriteMovies != value)
            {
                _favoriteMovies = value;
                OnPropertyChanged();
            }
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

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
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

    public ICommand EditProfileCommand { get; }
    public ICommand ChangePasswordCommand { get; }
    public ICommand ForgotPasswordCommand { get; }
    public ICommand LogoutCommand { get; }

    public async void LoadUserData()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            if (!await _apiService.IsAuthenticated())
            {
                ErrorMessage = "You are not logged in.";
                Debug.WriteLine("User is not authenticated");
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            var userId = await _apiService.GetCurrentUserId();
            if (userId == null)
            {
                ErrorMessage = "Could not get user ID.";
                Debug.WriteLine("Failed to get user ID");
                return;
            }

            Debug.WriteLine($"Loading data for user ID: {userId}");
            _user = await _apiService.GetUserByIdAsync(userId.Value);
            
            if (_user != null)
            {
                Username = _user.Username;
                Email = _user.Email;
                Debug.WriteLine($"Loaded user data: {Username}, {Email}");

                // Load favorite movies
                var favoriteMovies = await _apiService.GetFavoriteMoviesAsync(userId.Value);
                if (favoriteMovies != null)
                {
                    FavoriteMovies.Clear();
                    foreach (var movie in favoriteMovies)
                    {
                        FavoriteMovies.Add(movie);
                    }
                    Debug.WriteLine($"Loaded {FavoriteMovies.Count} favorite movies");
                }
            }
            else
            {
                ErrorMessage = "Failed to load user data.";
                Debug.WriteLine("Failed to load user data");
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading user data: {ex.Message}";
            Debug.WriteLine($"Error loading user data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnEditProfileClicked()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            if (_user == null)
            {
                ErrorMessage = "User data not loaded.";
                return;
            }

            // Show edit dialog
            var newUsername = await Shell.Current.DisplayPromptAsync("Edit Profile", "Enter new username:", initialValue: Username);
            if (string.IsNullOrWhiteSpace(newUsername))
                return;

            var newEmail = await Shell.Current.DisplayPromptAsync("Edit Profile", "Enter new email:", initialValue: Email);
            if (string.IsNullOrWhiteSpace(newEmail))
                return;

            // Update user object
            _user.Username = newUsername;
            _user.Email = newEmail;

            // Send update to API
            var updatedUser = await _apiService.UpdateUserAsync(_user);
            if (updatedUser != null)
            {
                Username = updatedUser.Username;
                Email = updatedUser.Email;
                await Shell.Current.DisplayAlert("Success", "Profile updated successfully.", "OK");
            }
            else
            {
                ErrorMessage = "Failed to update profile.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error updating profile: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnChangePasswordClicked()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var currentPassword = await Shell.Current.DisplayPromptAsync("Change Password", "Enter current password:", maxLength: 50, keyboard: Keyboard.Text);
            if (string.IsNullOrWhiteSpace(currentPassword))
                return;

            var newPassword = await Shell.Current.DisplayPromptAsync("Change Password", "Enter new password:", maxLength: 50, keyboard: Keyboard.Text);
            if (string.IsNullOrWhiteSpace(newPassword))
                return;

            var confirmPassword = await Shell.Current.DisplayPromptAsync("Change Password", "Confirm new password:", maxLength: 50, keyboard: Keyboard.Text);
            if (string.IsNullOrWhiteSpace(confirmPassword))
                return;

            if (newPassword != confirmPassword)
            {
                ErrorMessage = "New passwords do not match.";
                return;
            }

            var success = await _apiService.UpdatePasswordAsync(Email, currentPassword, newPassword);
            if (success)
            {
                await Shell.Current.DisplayAlert("Success", "Password updated successfully.", "OK");
            }
            else
            {
                ErrorMessage = "Failed to update password. Please check your current password.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error changing password: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnForgotPasswordClicked()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var email = await Shell.Current.DisplayPromptAsync("Forgot Password", "Enter your email address:", maxLength: 100, keyboard: Keyboard.Email);
            if (string.IsNullOrWhiteSpace(email))
                return;

            var success = await _apiService.ForgotPasswordAsync(email);
            if (success)
            {
                await Shell.Current.DisplayAlert("Success", "Password reset instructions have been sent to your email.", "OK");
            }
            else
            {
                ErrorMessage = "Failed to process forgot password request. Please check your email address.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error processing forgot password request: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnLogoutClicked()
    {
        await _apiService.ClearToken();
        await Shell.Current.GoToAsync("//LoginPage");
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 
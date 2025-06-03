using System.ComponentModel;
using System.Runtime.CompilerServices;
using MovieApp.Maui.Services;

namespace MovieApp.Maui.ViewModels;

public class PasswordUpdateViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private string _email = string.Empty;
    private string _currentPassword = string.Empty;
    private string _newPassword = string.Empty;
    private string _confirmNewPassword = string.Empty;
    private bool _isBusy;
    private string _errorMessage = string.Empty;
    private string _successMessage = string.Empty;

    public PasswordUpdateViewModel(ApiService apiService)
    {
        _apiService = apiService;
        UpdatePasswordCommand = new Command(async () => await UpdatePasswordAsync());
    }

    public string Email
    {
        get => _email;
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged();
            }
        }
    }

    public string CurrentPassword
    {
        get => _currentPassword;
        set
        {
            if (_currentPassword != value)
            {
                _currentPassword = value;
                OnPropertyChanged();
            }
        }
    }

    public string NewPassword
    {
        get => _newPassword;
        set
        {
            if (_newPassword != value)
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }
    }

    public string ConfirmNewPassword
    {
        get => _confirmNewPassword;
        set
        {
            if (_confirmNewPassword != value)
            {
                _confirmNewPassword = value;
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

    public string SuccessMessage
    {
        get => _successMessage;
        set
        {
            if (_successMessage != value)
            {
                _successMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public Command UpdatePasswordCommand { get; }

    private async Task UpdatePasswordAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(CurrentPassword) ||
                string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmNewPassword))
            {
                ErrorMessage = "Tüm alanlar gereklidir.";
                return;
            }

            if (NewPassword != ConfirmNewPassword)
            {
                ErrorMessage = "Yeni şifreler eşleşmiyor.";
                return;
            }

            var result = await _apiService.UpdatePasswordAsync(Email, CurrentPassword, NewPassword);
            if (result)
            {
                SuccessMessage = "Şifreniz başarıyla güncellendi.";
                // Başarılı güncelleme - Giriş sayfasına yönlendir
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                ErrorMessage = "Şifre güncellenirken bir hata oluştu.";
            }
        }
        catch
        {
            ErrorMessage = "Şifre güncellenirken bir hata oluştu.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 
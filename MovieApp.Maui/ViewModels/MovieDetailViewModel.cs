using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MovieApp.Maui.Models;
using MovieApp.Maui.Services;
using Microsoft.Maui.ApplicationModel;

namespace MovieApp.Maui.ViewModels;

public class MovieDetailViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private Movie? _movie;
    private UserRating? _userRating;
    private int _rating;
    private string _review = string.Empty;
    private bool _isBusy;
    private string _errorMessage = string.Empty;
    private bool _isFavorite;
    private bool _isTrailerVisible;

    public MovieDetailViewModel(ApiService apiService)
    {
        _apiService = apiService;
        SaveRatingCommand = new Command(async () => await SaveRatingAsync());
        AddToFavoritesCommand = new Command(OnAddToFavorites);
        OpenTrailerCommand = new Command(OnOpenTrailer);
        OpenHomepageCommand = new Command(OnOpenHomepage);
        RateMovieCommand = new Command(OnRateMovie);
        GoBackCommand = new Command(async () => await Shell.Current.GoToAsync("///MovieListPage"));
        ShowTrailerCommand = new Command(() => IsTrailerVisible = true);
        HideTrailerCommand = new Command(() => IsTrailerVisible = false);
    }

    public Movie? Movie
    {
        get => _movie;
        set
        {
            if (_movie != value)
            {
                _movie = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Overview));
                OnPropertyChanged(nameof(PosterPath));
                OnPropertyChanged(nameof(ReleaseDate));
                OnPropertyChanged(nameof(VoteAverage));
                OnPropertyChanged(nameof(Genres));
                OnPropertyChanged(nameof(Director));
                OnPropertyChanged(nameof(Cast));
                OnPropertyChanged(nameof(OriginalTitle));
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(VoteCount));
                OnPropertyChanged(nameof(UserRatingAverage));
                OnPropertyChanged(nameof(UserRatingCount));
                OnPropertyChanged(nameof(Runtime));
                OnPropertyChanged(nameof(OriginalLanguage));
                OnPropertyChanged(nameof(Tagline));
                OnPropertyChanged(nameof(ProductionCompanies));
                OnPropertyChanged(nameof(ProductionCountries));
                OnPropertyChanged(nameof(SpokenLanguages));
                OnPropertyChanged(nameof(Budget));
                OnPropertyChanged(nameof(Revenue));
                OnPropertyChanged(nameof(Keywords));
                OnPropertyChanged(nameof(IsAdult));
                OnPropertyChanged(nameof(Popularity));
            }
        }
    }

    public UserRating? UserRating
    {
        get => _userRating;
        set
        {
            if (_userRating != value)
            {
                _userRating = value;
                OnPropertyChanged();
            }
        }
    }

    public int Rating
    {
        get => _rating;
        set
        {
            if (_rating != value)
            {
                _rating = value;
                OnPropertyChanged();
            }
        }
    }

    public string Review
    {
        get => _review;
        set
        {
            if (_review != value)
            {
                _review = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsFavorite
    {
        get => _isFavorite;
        set
        {
            if (_isFavorite != value)
            {
                _isFavorite = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsTrailerVisible
    {
        get => _isTrailerVisible;
        set { _isTrailerVisible = value; OnPropertyChanged(); }
    }

    public string Title => Movie?.Title ?? string.Empty;
    public string Overview => Movie?.Overview ?? string.Empty;
    public string? PosterPath => Movie?.PosterPath;
    public DateTime ReleaseDate => Movie?.ReleaseDate ?? DateTime.MinValue;
    public double VoteAverage => Movie?.VoteAverage ?? 0;
    public string? Genres => Movie?.Genres;
    public string? Director => Movie?.Director;
    public string? Cast => Movie?.Cast;
    public string? OriginalTitle => Movie?.OriginalTitle;
    public string? Status => Movie?.Status;
    public int VoteCount => Movie?.VoteCount ?? 0;
    public double? UserRatingAverage => Movie?.UserRatingAverage;
    public int? UserRatingCount => Movie?.UserRatingCount;
    public int? Runtime => Movie?.Runtime;
    public string? OriginalLanguage => Movie?.OriginalLanguage;
    public string? Tagline => Movie?.Tagline;
    public string? ProductionCompanies => Movie?.ProductionCompanies;
    public string? ProductionCountries => Movie?.ProductionCountries;
    public string? SpokenLanguages => Movie?.SpokenLanguages;
    public decimal? Budget => Movie?.Budget;
    public decimal? Revenue => Movie?.Revenue;
    public string? Keywords => Movie?.Keywords;
    public bool IsAdult => Movie?.IsAdult ?? false;
    public double? Popularity => Movie?.Popularity;

    public ICommand AddToFavoritesCommand { get; }
    public ICommand OpenTrailerCommand { get; }
    public ICommand OpenHomepageCommand { get; }
    public ICommand RateMovieCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand ShowTrailerCommand { get; }
    public ICommand HideTrailerCommand { get; }

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

    public Command SaveRatingCommand { get; }

    public string? TrailerEmbedUrl
    {
        get
        {
            if (Movie?.TrailerUrl == null) return null;
            // YouTube linkini embed formatına çevir
            if (Movie.TrailerUrl.Contains("youtube.com") || Movie.TrailerUrl.Contains("youtu.be"))
            {
                var videoId = ExtractYouTubeId(Movie.TrailerUrl);
                if (!string.IsNullOrEmpty(videoId))
                    return $"https://www.youtube.com/embed/{videoId}?autoplay=1";
            }
            return Movie.TrailerUrl;
        }
    }

    private string? ExtractYouTubeId(string url)
    {
        // youtu.be/xxxx veya youtube.com/watch?v=xxxx
        if (url.Contains("youtu.be/"))
        {
            var parts = url.Split("youtu.be/");
            if (parts.Length > 1) return parts[1].Split('?')[0];
        }
        if (url.Contains("watch?v="))
        {
            var parts = url.Split("watch?v=");
            if (parts.Length > 1) return parts[1].Split('&')[0];
        }
        return null;
    }

    public async Task LoadMovieAsync(int movieId)
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] Film detayları yükleniyor: {movieId}");

            Movie = await _apiService.GetMovieByIdAsync(movieId);
            System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] GetMovieByIdAsync yanıtı: {(Movie != null ? "Başarılı" : "Başarısız")}");
            if (Movie == null)
            {
                ErrorMessage = "Film bulunamadı.";
                System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] Film bulunamadı: {movieId}");
                return;
            }

            // Kullanıcı ID'sini al
            var userId = await _apiService.GetCurrentUserId();
            System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] Kullanıcı ID: {userId}");
            if (userId != null)
            {
                // Favori durumunu kontrol et
                IsFavorite = await _apiService.IsFavoriteAsync(userId.Value, movieId);
                System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] IsFavorite: {IsFavorite}");
                // Kullanıcı değerlendirmesini al
                UserRating = await _apiService.GetUserRatingAsync(userId.Value, movieId);
                System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] GetUserRatingAsync yanıtı: {(UserRating != null ? "Var" : "Yok")}");
                if (UserRating != null)
                {
                    Rating = (int)UserRating.Rating;
                    Review = UserRating.Review ?? string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Film detayları yüklenirken hata: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] Hata: {ex.Message}\n{ex.StackTrace}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SaveRatingAsync()
    {
        if (IsBusy || Movie == null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] SaveRatingAsync başlatıldı. MovieId: {Movie.Id}");
            var userId = await _apiService.GetCurrentUserId();
            System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] SaveRatingAsync userId: {userId}");
            if (userId == null)
            {
                ErrorMessage = "Kullanıcı bulunamadı. Lütfen tekrar giriş yapın.";
                await Shell.Current.DisplayAlert("Hata", ErrorMessage, "Tamam");
                System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] SaveRatingAsync: Kullanıcı bulunamadı.");
                return;
            }

            var rating = new UserRating
            {
                UserId = userId.Value,
                MovieId = Movie.Id,
                Rating = Rating,
                Review = Review
            };
            System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] SaveRatingAsync rating objesi oluşturuldu: {System.Text.Json.JsonSerializer.Serialize(rating)}");
            if (UserRating == null)
            {
                UserRating = await _apiService.CreateUserRatingAsync(rating);
                System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] CreateUserRatingAsync yanıtı: {(UserRating != null ? "Başarılı" : "Başarısız")}");
            }
            else
            {
                UserRating = await _apiService.UpdateUserRatingAsync(rating);
                System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] UpdateUserRatingAsync yanıtı: {(UserRating != null ? "Başarılı" : "Başarısız")}");
            }

            if (UserRating != null)
            {
                await Shell.Current.DisplayAlert("Başarılı", "Puan kaydedildi!", "Tamam");
            }
            else
            {
                ErrorMessage = "Değerlendirme kaydedilirken bir hata oluştu.";
                await Shell.Current.DisplayAlert("Hata", ErrorMessage, "Tamam");
                System.Diagnostics.Debug.WriteLine($"[MovieDetailViewModel] SaveRatingAsync: Değerlendirme kaydedilemedi.");
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Değerlendirme kaydedilirken bir hata oluştu: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"[SaveRatingAsync] Hata: {ex.Message}\n{ex.StackTrace}");
            await Shell.Current.DisplayAlert("Hata", ErrorMessage, "Tamam");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async void OnAddToFavorites()
    {
        if (Movie == null) return;

        try
        {
            var userId = await _apiService.GetCurrentUserId();
            if (userId == null)
            {
                await Shell.Current.DisplayAlert("Error", "You must be logged in to add favorites.", "OK");
                return;
            }
            bool result;
            if (!IsFavorite)
            {
                result = await _apiService.AddToFavoritesAsync(userId.Value, Movie.Id);
                if (result)
                {
                    IsFavorite = true;
                    await Shell.Current.DisplayAlert("Success", "Added to favorites!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to add to favorites.", "OK");
                }
            }
            else
            {
                result = await _apiService.RemoveFromFavoritesAsync(userId.Value, Movie.Id);
                if (result)
                {
                    IsFavorite = false;
                    await Shell.Current.DisplayAlert("Success", "Removed from favorites!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to remove from favorites.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Failed to update favorites.", "OK");
        }
    }

    private async void OnOpenTrailer()
    {
        if (Movie?.TrailerUrl == null) return;

        try
        {
            await Launcher.OpenAsync(new Uri(Movie.TrailerUrl));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Could not open trailer.", "OK");
        }
    }

    private async void OnOpenHomepage()
    {
        if (Movie?.Homepage == null) return;

        try
        {
            await Launcher.OpenAsync(new Uri(Movie.Homepage));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Could not open homepage.", "OK");
        }
    }

    private async void OnRateMovie()
    {
        if (Movie == null) return;

        try
        {
            var rating = await Shell.Current.DisplayPromptAsync(
                "Rate Movie",
                "Enter your rating (1-10):",
                "Rate",
                "Cancel",
                "10",
                maxLength: 2,
                keyboard: Keyboard.Numeric);

            if (string.IsNullOrEmpty(rating)) return;

            if (double.TryParse(rating, out double ratingValue) && ratingValue >= 1 && ratingValue <= 10)
            {
                // TODO: Implement rating logic
                await Shell.Current.DisplayAlert("Success", "Rating submitted!", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Please enter a valid rating between 1 and 10.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Failed to submit rating.", "OK");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 
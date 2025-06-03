using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MovieApp.Maui.Models;
using MovieApp.Maui.Services;
using System.Windows.Input;

namespace MovieApp.Maui.ViewModels;

public class RatedMoviesViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private ObservableCollection<UserRating> _ratedMovies = new();
    private UserRating? _selectedRating;
    public ObservableCollection<UserRating> RatedMovies
    {
        get => _ratedMovies;
        set { _ratedMovies = value; OnPropertyChanged(); }
    }

    public UserRating? SelectedRating
    {
        get => _selectedRating;
        set { _selectedRating = value; OnPropertyChanged(); }
    }

    public ICommand RatingSelectedCommand { get; }

    public RatedMoviesViewModel(ApiService apiService)
    {
        _apiService = apiService;
        LoadRatedMoviesCommand = new Command(async () => await LoadRatedMoviesAsync());
        RatingSelectedCommand = new Command<UserRating>(OnRatingSelected);
        LoadRatedMoviesCommand.Execute(null);
    }

    public Command LoadRatedMoviesCommand { get; }

    private async Task LoadRatedMoviesAsync()
    {
        var userId = await _apiService.GetCurrentUserId();
        if (userId == null) return;
        var ratings = await _apiService.GetUserRatingsAsync(userId.Value);
        foreach (var rating in ratings)
        {
            rating.Movie = await _apiService.GetMovieByIdAsync(rating.MovieId);
        }
        RatedMovies = new ObservableCollection<UserRating>(ratings);
    }

    private async void OnRatingSelected(UserRating? rating)
    {
        if (rating?.Movie == null) return;
        var route = $"///MovieDetailPage?movieId={rating.Movie.Id}";
        await Shell.Current.GoToAsync(route);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
} 
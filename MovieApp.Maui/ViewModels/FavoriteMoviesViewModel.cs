using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MovieApp.Maui.Models;
using MovieApp.Maui.Services;

namespace MovieApp.Maui.ViewModels;

public class FavoriteMoviesViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private ObservableCollection<Movie> _favoriteMovies = new();
    private Movie? _selectedMovie;
    private bool _hasSelectedMovies;
    public event PropertyChangedEventHandler? PropertyChanged;

    public FavoriteMoviesViewModel(ApiService apiService)
    {
        _apiService = apiService;
        LoadFavoriteMoviesCommand = new Command(async () => await LoadFavoriteMoviesAsync());
        MovieSelectedCommand = new Command<Movie>(OnMovieSelected);
        RemoveSelectedCommand = new Command(async () => await RemoveSelectedMoviesAsync());
    }

    public ObservableCollection<Movie> FavoriteMovies
    {
        get => _favoriteMovies;
        set { _favoriteMovies = value; OnPropertyChanged(); }
    }

    public Movie? SelectedMovie
    {
        get => _selectedMovie;
        set { _selectedMovie = value; OnPropertyChanged(); }
    }

    public bool HasSelectedMovies
    {
        get => _hasSelectedMovies;
        set { _hasSelectedMovies = value; OnPropertyChanged(); }
    }

    public ICommand LoadFavoriteMoviesCommand { get; }
    public ICommand MovieSelectedCommand { get; }
    public ICommand RemoveSelectedCommand { get; }

    private async Task LoadFavoriteMoviesAsync()
    {
        var userId = await _apiService.GetCurrentUserId();
        if (userId == null) return;
        var movies = await _apiService.GetFavoriteMoviesAsync(userId.Value);
        FavoriteMovies = new ObservableCollection<Movie>(movies);
        UpdateHasSelectedMovies();
    }

    private async void OnMovieSelected(Movie? movie)
    {
        if (movie == null) return;
        var route = $"///MovieDetailPage?movieId={movie.Id}";
        await Shell.Current.GoToAsync(route);
    }

    public void UpdateHasSelectedMovies()
    {
        HasSelectedMovies = FavoriteMovies.Any(m => m.IsSelected);
    }

    private async Task RemoveSelectedMoviesAsync()
    {
        try
        {
            var userId = await _apiService.GetCurrentUserId();
            if (userId == null)
            {
                await Shell.Current.DisplayAlert("Error", "You must be logged in to manage favorites.", "OK");
                return;
            }

            var selectedMovies = FavoriteMovies.Where(m => m.IsSelected).ToList();
            if (!selectedMovies.Any())
            {
                await Shell.Current.DisplayAlert("Error", "Please select movies to remove.", "OK");
                return;
            }

            bool allSuccess = true;
            foreach (var movie in selectedMovies)
            {
                var result = await _apiService.RemoveFromFavoritesAsync(userId.Value, movie.Id);
                if (result)
                {
                    FavoriteMovies.Remove(movie);
                }
                else
                {
                    allSuccess = false;
                }
            }

            if (allSuccess)
            {
                await Shell.Current.DisplayAlert("Success", "Selected movies removed from favorites!", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Warning", "Some movies could not be removed.", "OK");
            }

            UpdateHasSelectedMovies();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Failed to update favorites.", "OK");
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
} 
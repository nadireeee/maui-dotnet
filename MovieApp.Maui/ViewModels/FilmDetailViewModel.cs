using System.ComponentModel;
using System.Runtime.CompilerServices;
using MovieApp.Maui.Models;
using MovieApp.Maui.Services;

namespace MovieApp.Maui.ViewModels;

public class FilmDetailViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private Movie? _movie;
    private int _movieId;
    public Movie? Movie
    {
        get => _movie;
        set { _movie = value; OnPropertyChanged(); }
    }

    public int MovieId
    {
        get => _movieId;
        set
        {
            if (_movieId != value)
            {
                _movieId = value;
                OnPropertyChanged();
                _ = LoadMovieAsync(_movieId);
            }
        }
    }

    public FilmDetailViewModel()
    {
        _apiService = new ApiService();
    }

    public async Task LoadMovieAsync(int movieId)
    {
        Movie = await _apiService.GetMovieByIdAsync(movieId);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 
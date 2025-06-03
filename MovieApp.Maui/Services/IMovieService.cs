using MovieApp.Maui.Models;

namespace MovieApp.Maui.Services;

public interface IMovieService
{
    Task<Movie?> GetFeaturedMovieAsync();
    Task<IEnumerable<Movie>> GetPopularMoviesAsync();
    Task<IEnumerable<Movie>> GetTrendingMoviesAsync();
    Task<IEnumerable<Movie>> GetContinueWatchingMoviesAsync();
    Task<IEnumerable<Movie>> SearchMoviesAsync(string query);
    Task<Movie?> GetMovieByIdAsync(int id);
    Task<double> GetWatchProgressAsync(int movieId);
    Task<bool> AddToFavoritesAsync(int movieId);
    Task<bool> RemoveFromFavoritesAsync(int movieId);
    Task<bool> IsFavoriteAsync(int movieId);
    Task<bool> RateMovieAsync(int movieId, double rating);
    Task<double?> GetUserRatingAsync(int movieId);
    Task<bool> UpdateWatchProgressAsync(int movieId, double progress);
} 
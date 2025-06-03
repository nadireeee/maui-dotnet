using MovieApp.Api.Models;

namespace MovieApp.Api.Services.Interfaces;

public interface IMovieService
{
    Task<Movie?> GetMovieByIdAsync(int id);
    Task<IEnumerable<Movie>> GetAllMoviesAsync();
    Task<IEnumerable<Movie>> SearchMoviesAsync(string searchTerm);
    Task<Movie> CreateMovieAsync(Movie movie, int userId);
    Task<Movie> UpdateMovieAsync(Movie movie, int userId);
    Task<bool> DeleteMovieAsync(int id);
    Task<IEnumerable<Movie>> GetFavoriteMoviesAsync(int userId);
    Task<bool> AddToFavoritesAsync(int userId, int movieId);
    Task<bool> RemoveFromFavoritesAsync(int userId, int movieId);
    Task<bool> IsFavoriteAsync(int userId, int movieId);
    
    // New methods for advanced filtering and search
    Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre);
    Task<IEnumerable<Movie>> GetLocalMoviesAsync();
    Task<IEnumerable<Movie>> GetForeignMoviesAsync();
    Task<IEnumerable<Movie>> GetMoviesByYearAsync(int year);
    Task<IEnumerable<Movie>> GetMoviesByRatingRangeAsync(double minRating, double maxRating);
    Task<IEnumerable<Movie>> GetMoviesByDirectorAsync(string director);
    Task<IEnumerable<Movie>> GetMoviesByActorAsync(string actor);
    Task<IEnumerable<Movie>> GetTopRatedMoviesAsync(int count);
    Task<IEnumerable<Movie>> GetNewestMoviesAsync(int count);
    Task<IEnumerable<Movie>> GetPopularMoviesAsync(int count);
    Task<IEnumerable<Movie>> GetMoviesByLanguageAsync(string language);
    Task<IEnumerable<Movie>> GetAdultMoviesAsync();
    Task<IEnumerable<Movie>> GetFamilyMoviesAsync();
    Task<IEnumerable<Movie>> GetMoviesByKeywordsAsync(string[] keywords);
    Task<IEnumerable<Movie>> GetMoviesByProductionCompanyAsync(string company);
    Task<IEnumerable<Movie>> GetMoviesByProductionCountryAsync(string country);
    Task<IEnumerable<Movie>> GetMoviesByStatusAsync(string status);
    Task<IEnumerable<Movie>> GetMoviesByBudgetRangeAsync(decimal minBudget, decimal maxBudget);
    Task<IEnumerable<Movie>> GetMoviesByRevenueRangeAsync(decimal minRevenue, decimal maxRevenue);
    Task<IEnumerable<Movie>> GetMoviesByRuntimeRangeAsync(int minRuntime, int maxRuntime);
} 
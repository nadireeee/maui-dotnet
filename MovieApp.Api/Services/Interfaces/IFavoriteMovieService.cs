using MovieApp.Api.Models;

namespace MovieApp.Api.Services.Interfaces;

public interface IFavoriteMovieService
{
    Task<IEnumerable<Movie>> GetUserFavoriteMoviesAsync(int userId);
    Task<bool> AddFavoriteMovieAsync(int userId, int movieId);
    Task<bool> RemoveFavoriteMovieAsync(int userId, int movieId);
    Task<bool> IsFavoriteMovieAsync(int userId, int movieId);
} 
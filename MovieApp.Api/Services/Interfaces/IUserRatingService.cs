using MovieApp.Api.Models;

namespace MovieApp.Api.Services.Interfaces;

public interface IUserRatingService
{
    Task<UserRating?> GetUserRatingAsync(int userId, int movieId);
    Task<IEnumerable<UserRating>> GetUserRatingsAsync(int userId);
    Task<IEnumerable<UserRating>> GetMovieRatingsAsync(int movieId);
    Task<UserRating> CreateUserRatingAsync(UserRating userRating);
    Task<UserRating> UpdateUserRatingAsync(UserRating userRating);
    Task<bool> DeleteUserRatingAsync(int userId, int movieId);
    Task<double> GetAverageRatingForMovieAsync(int movieId);
    Task<int> GetRatingCountForMovieAsync(int movieId);
} 
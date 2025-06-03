using Microsoft.EntityFrameworkCore;
using MovieApp.Api.Data;
using MovieApp.Api.Models;
using MovieApp.Api.Services.Interfaces;

namespace MovieApp.Api.Services;

public class UserRatingService : IUserRatingService
{
    private readonly ApplicationDbContext _context;
    private readonly IMovieService _movieService;

    public UserRatingService(ApplicationDbContext context, IMovieService movieService)
    {
        _context = context;
        _movieService = movieService;
    }

    public async Task<UserRating?> GetUserRatingAsync(int userId, int movieId)
    {
        return await _context.UserRatings
            .Include(ur => ur.User)
            .Include(ur => ur.Movie)
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.MovieId == movieId);
    }

    public async Task<IEnumerable<UserRating>> GetUserRatingsAsync(int userId)
    {
        return await _context.UserRatings
            .Include(ur => ur.Movie)
            .Where(ur => ur.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserRating>> GetMovieRatingsAsync(int movieId)
    {
        return await _context.UserRatings
            .Include(ur => ur.User)
            .Where(ur => ur.MovieId == movieId)
            .ToListAsync();
    }

    public async Task<UserRating> CreateUserRatingAsync(UserRating userRating)
    {
        // Check if the user has already rated this movie
        var existingRating = await GetUserRatingAsync(userRating.UserId, userRating.MovieId);
        if (existingRating != null)
        {
            throw new InvalidOperationException("User has already rated this movie. Use UpdateUserRatingAsync instead.");
        }

        _context.UserRatings.Add(userRating);
        await _context.SaveChangesAsync();

        // Update the movie's average rating and rating count
        await UpdateMovieRatingStatsAsync(userRating.MovieId);

        return userRating;
    }

    public async Task<UserRating> UpdateUserRatingAsync(UserRating userRating)
    {
        var existingRating = await GetUserRatingAsync(userRating.UserId, userRating.MovieId);
        if (existingRating == null)
        {
            throw new KeyNotFoundException($"Rating not found for user {userRating.UserId} and movie {userRating.MovieId}");
        }

        existingRating.Rating = userRating.Rating;
        existingRating.Review = userRating.Review;
        existingRating.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Update the movie's average rating and rating count
        await UpdateMovieRatingStatsAsync(userRating.MovieId);

        return existingRating;
    }

    public async Task<bool> DeleteUserRatingAsync(int userId, int movieId)
    {
        var userRating = await GetUserRatingAsync(userId, movieId);
        if (userRating == null)
            return false;

        _context.UserRatings.Remove(userRating);
        await _context.SaveChangesAsync();

        // Update the movie's average rating and rating count
        await UpdateMovieRatingStatsAsync(movieId);

        return true;
    }

    public async Task<double> GetAverageRatingForMovieAsync(int movieId)
    {
        var ratings = await _context.UserRatings
            .Where(ur => ur.MovieId == movieId)
            .Select(ur => ur.Rating)
            .ToListAsync();

        if (!ratings.Any())
            return 0;

        return ratings.Average();
    }

    public async Task<int> GetRatingCountForMovieAsync(int movieId)
    {
        return await _context.UserRatings
            .CountAsync(ur => ur.MovieId == movieId);
    }

    private async Task UpdateMovieRatingStatsAsync(int movieId)
    {
        var movie = await _movieService.GetMovieByIdAsync(movieId);
        if (movie == null)
            return;

        movie.UserRatingAverage = await GetAverageRatingForMovieAsync(movieId);
        movie.UserRatingCount = await GetRatingCountForMovieAsync(movieId);

        await _movieService.UpdateMovieAsync(movie, movie.CreatedById);
    }
} 
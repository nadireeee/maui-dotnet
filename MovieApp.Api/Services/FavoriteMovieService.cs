using Microsoft.EntityFrameworkCore;
using MovieApp.Api.Data;
using MovieApp.Api.Models;
using MovieApp.Api.Services.Interfaces;

namespace MovieApp.Api.Services;

public class FavoriteMovieService : IFavoriteMovieService
{
    private readonly ApplicationDbContext _context;

    public FavoriteMovieService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Movie>> GetUserFavoriteMoviesAsync(int userId)
    {
        return await _context.FavoriteMovies
            .Where(fm => fm.UserId == userId)
            .Include(fm => fm.Movie)
            .Select(fm => fm.Movie)
            .ToListAsync();
    }

    public async Task<bool> AddFavoriteMovieAsync(int userId, int movieId)
    {
        if (await IsFavoriteMovieAsync(userId, movieId))
            return false;

        var favoriteMovie = new FavoriteMovie
        {
            UserId = userId,
            MovieId = movieId,
            CreatedAt = DateTime.UtcNow
        };

        await _context.FavoriteMovies.AddAsync(favoriteMovie);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFavoriteMovieAsync(int userId, int movieId)
    {
        var favoriteMovie = await _context.FavoriteMovies
            .FirstOrDefaultAsync(fm => fm.UserId == userId && fm.MovieId == movieId);

        if (favoriteMovie == null)
            return false;

        _context.FavoriteMovies.Remove(favoriteMovie);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsFavoriteMovieAsync(int userId, int movieId)
    {
        return await _context.FavoriteMovies
            .AnyAsync(fm => fm.UserId == userId && fm.MovieId == movieId);
    }
} 
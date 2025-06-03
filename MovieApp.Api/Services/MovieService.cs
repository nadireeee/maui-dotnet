using Microsoft.EntityFrameworkCore;
using MovieApp.Api.Data;
using MovieApp.Api.Models;
using MovieApp.Api.Services.Interfaces;

namespace MovieApp.Api.Services;

public class MovieService : IMovieService
{
    private readonly ApplicationDbContext _context;

    public MovieService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Movie?> GetMovieByIdAsync(int id)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> SearchMoviesAsync(string searchTerm)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => (m.Title ?? string.Empty).Contains(searchTerm) || 
                        (m.Overview ?? string.Empty).Contains(searchTerm) || 
                        (m.Keywords ?? string.Empty).Contains(searchTerm) ||
                        (m.Cast ?? string.Empty).Contains(searchTerm) ||
                        (m.Director ?? string.Empty).Contains(searchTerm))
            .ToListAsync();
    }

    public async Task<Movie> CreateMovieAsync(Movie movie, int userId)
    {
        movie.CreatedById = userId;
        movie.CreatedAt = DateTime.UtcNow;
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return movie;
    }

    public async Task<Movie> UpdateMovieAsync(Movie movie, int userId)
    {
        var existingMovie = await _context.Movies.FindAsync(movie.Id);
        if (existingMovie == null)
            throw new KeyNotFoundException($"Movie with ID {movie.Id} not found.");

        existingMovie.Title = movie.Title;
        existingMovie.Overview = movie.Overview;
        existingMovie.PosterPath = movie.PosterPath;
        existingMovie.ReleaseDate = movie.ReleaseDate;
        existingMovie.VoteAverage = movie.VoteAverage;
        existingMovie.VoteCount = movie.VoteCount;
        existingMovie.OriginalLanguage = movie.OriginalLanguage;
        existingMovie.OriginalTitle = movie.OriginalTitle;
        existingMovie.IsLocal = movie.IsLocal;
        existingMovie.Director = movie.Director;
        existingMovie.Cast = movie.Cast;
        existingMovie.Genres = movie.Genres;
        existingMovie.Runtime = movie.Runtime;
        existingMovie.Tagline = movie.Tagline;
        existingMovie.Status = movie.Status;
        existingMovie.Budget = movie.Budget;
        existingMovie.Revenue = movie.Revenue;
        existingMovie.ProductionCompanies = movie.ProductionCompanies;
        existingMovie.ProductionCountries = movie.ProductionCountries;
        existingMovie.SpokenLanguages = movie.SpokenLanguages;
        existingMovie.Keywords = movie.Keywords;
        existingMovie.TrailerUrl = movie.TrailerUrl;
        existingMovie.Homepage = movie.Homepage;
        existingMovie.IsAdult = movie.IsAdult;
        existingMovie.Popularity = movie.Popularity;
        existingMovie.UserRatingAverage = movie.UserRatingAverage;
        existingMovie.UserRatingCount = movie.UserRatingCount;
        existingMovie.UpdatedById = userId;
        existingMovie.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingMovie;
    }

    public async Task<bool> DeleteMovieAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
            return false;

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Movie>> GetFavoriteMoviesAsync(int userId)
    {
        var favoriteMovies = await _context.FavoriteMovies
            .Where(fm => fm.UserId == userId)
            .Include(fm => fm.Movie)
                .ThenInclude(m => m.CreatedBy)
            .Include(fm => fm.Movie)
                .ThenInclude(m => m.UpdatedBy)
            .Select(fm => fm.Movie)
            .Where(m => m != null)
            .ToListAsync();

        return favoriteMovies!;
    }

    public async Task<bool> AddToFavoritesAsync(int userId, int movieId)
    {
        if (await IsFavoriteAsync(userId, movieId))
            return false;

        var favoriteMovie = new FavoriteMovie
        {
            UserId = userId,
            MovieId = movieId
        };

        _context.FavoriteMovies.Add(favoriteMovie);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFromFavoritesAsync(int userId, int movieId)
    {
        var favoriteMovie = await _context.FavoriteMovies
            .FirstOrDefaultAsync(fm => fm.UserId == userId && fm.MovieId == movieId);

        if (favoriteMovie == null)
            return false;

        _context.FavoriteMovies.Remove(favoriteMovie);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsFavoriteAsync(int userId, int movieId)
    {
        return await _context.FavoriteMovies
            .AnyAsync(fm => fm.UserId == userId && fm.MovieId == movieId);
    }

    // New methods for advanced filtering and search
    public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => (m.Genres ?? string.Empty).Contains(genre))
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetLocalMoviesAsync()
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => m.IsLocal)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetForeignMoviesAsync()
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => !m.IsLocal)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByYearAsync(int year)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => m.ReleaseDate.Year == year)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByRatingRangeAsync(double minRating, double maxRating)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => m.VoteAverage >= minRating && m.VoteAverage <= maxRating)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByDirectorAsync(string director)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => (m.Director ?? string.Empty).Contains(director))
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByActorAsync(string actor)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => (m.Cast ?? string.Empty).Contains(actor))
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetTopRatedMoviesAsync(int count)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .OrderByDescending(m => m.VoteAverage)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetNewestMoviesAsync(int count)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .OrderByDescending(m => m.ReleaseDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetPopularMoviesAsync(int count)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .OrderByDescending(m => m.Popularity)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByLanguageAsync(string language)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => m.OriginalLanguage == language || m.SpokenLanguages.Contains(language))
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetAdultMoviesAsync()
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => m.IsAdult)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetFamilyMoviesAsync()
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => !m.IsAdult)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByKeywordsAsync(string[] keywords)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => keywords.All(k => (m.Keywords ?? string.Empty).Contains(k)))
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByProductionCompanyAsync(string company)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => (m.ProductionCompanies ?? string.Empty).Contains(company))
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByProductionCountryAsync(string country)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => (m.ProductionCountries ?? string.Empty).Contains(country))
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByStatusAsync(string status)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => m.Status == status)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByBudgetRangeAsync(decimal minBudget, decimal maxBudget)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => m.Budget >= minBudget && m.Budget <= maxBudget)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByRevenueRangeAsync(decimal minRevenue, decimal maxRevenue)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => m.Revenue >= minRevenue && m.Revenue <= maxRevenue)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByRuntimeRangeAsync(int minRuntime, int maxRuntime)
    {
        return await _context.Movies
            .Include(m => m.CreatedBy)
            .Include(m => m.UpdatedBy)
            .Where(m => m.Runtime >= minRuntime && m.Runtime <= maxRuntime)
            .ToListAsync();
    }
} 
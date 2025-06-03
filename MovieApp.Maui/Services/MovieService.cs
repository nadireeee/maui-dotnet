using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using MovieApp.Maui.Models;

namespace MovieApp.Maui.Services;

public class MovieService : IMovieService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public MovieService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001/api";
    }

    public async Task<Movie?> GetFeaturedMovieAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Movie>($"{_baseUrl}/movies/featured");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting featured movie: {ex.Message}");
            return null;
        }
    }

    public async Task<IEnumerable<Movie>> GetPopularMoviesAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Movie>>($"{_baseUrl}/movies/popular") ?? new List<Movie>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting popular movies: {ex.Message}");
            return new List<Movie>();
        }
    }

    public async Task<IEnumerable<Movie>> GetTrendingMoviesAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Movie>>($"{_baseUrl}/movies/trending") ?? new List<Movie>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting trending movies: {ex.Message}");
            return new List<Movie>();
        }
    }

    public async Task<IEnumerable<Movie>> GetContinueWatchingMoviesAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Movie>>($"{_baseUrl}/movies/continue-watching") ?? new List<Movie>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting continue watching movies: {ex.Message}");
            return new List<Movie>();
        }
    }

    public async Task<IEnumerable<Movie>> SearchMoviesAsync(string query)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Movie>>($"{_baseUrl}/movies/search?query={Uri.EscapeDataString(query)}") ?? new List<Movie>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching movies: {ex.Message}");
            return new List<Movie>();
        }
    }

    public async Task<Movie?> GetMovieByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Movie>($"{_baseUrl}/movies/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting movie by id: {ex.Message}");
            return null;
        }
    }

    public async Task<double> GetWatchProgressAsync(int movieId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<WatchProgressResponse>($"{_baseUrl}/movies/{movieId}/progress");
            return response?.Progress ?? 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting watch progress: {ex.Message}");
            return 0;
        }
    }

    public async Task<bool> AddToFavoritesAsync(int movieId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/movies/{movieId}/favorites", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding to favorites: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveFromFavoritesAsync(int movieId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/movies/{movieId}/favorites");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing from favorites: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> IsFavoriteAsync(int movieId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<FavoriteResponse>($"{_baseUrl}/movies/{movieId}/favorites");
            return response?.IsFavorite ?? false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking favorite status: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RateMovieAsync(int movieId, double rating)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/movies/{movieId}/rate", new { Rating = rating });
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error rating movie: {ex.Message}");
            return false;
        }
    }

    public async Task<double?> GetUserRatingAsync(int movieId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<UserRatingResponse>($"{_baseUrl}/movies/{movieId}/user-rating");
            return response?.Rating;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user rating: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdateWatchProgressAsync(int movieId, double progress)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/movies/{movieId}/progress", new { Progress = progress });
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating watch progress: {ex.Message}");
            return false;
        }
    }
}

public class WatchProgressResponse
{
    public double Progress { get; set; }
}

public class FavoriteResponse
{
    public bool IsFavorite { get; set; }
}

public class UserRatingResponse
{
    public double? Rating { get; set; }
} 
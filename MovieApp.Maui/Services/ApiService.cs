using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MovieApp.Maui.Models;
using Microsoft.Maui.Controls;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using BCrypt.Net;

namespace MovieApp.Maui.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private string? _authToken;
    private const string TOKEN_KEY = "auth_token";
    private const string USER_ID_KEY = "user_id";
    private static string? _staticToken; // Windows fallback
    private static string? _staticUserId; // Windows fallback

    public ApiService()
    {
        _baseUrl = GetBaseUrl();
        Debug.WriteLine($"API Base URL: {_baseUrl}");
        
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_baseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
        
        // Add default headers
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Load saved token if exists
        LoadSavedToken();
    }

    private async void LoadSavedToken()
    {
        try
        {
            var savedToken = await SecureStorage.GetAsync(TOKEN_KEY);
            if (!string.IsNullOrEmpty(savedToken))
            {
                SetToken(savedToken);
                Debug.WriteLine("Loaded saved token");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading saved token: {ex.Message}");
        }
    }

    public async Task<bool> IsAuthenticated()
    {
        var token = await GetToken();
        return !string.IsNullOrEmpty(token);
    }

    private async Task<string?> GetToken()
    {
        try
        {
            var token = await SecureStorage.GetAsync(TOKEN_KEY);
            if (!string.IsNullOrEmpty(token)) return token;
        }
        catch { }
        return _staticToken;
    }

    public async Task<int?> GetCurrentUserId()
    {
        try
        {
            var userId = await SecureStorage.GetAsync(USER_ID_KEY);
            if (!string.IsNullOrEmpty(userId)) return int.Parse(userId);
        }
        catch { }
        if (!string.IsNullOrEmpty(_staticUserId)) return int.Parse(_staticUserId);
        return null;
    }

    public async Task ClearToken()
    {
        _authToken = null;
        _staticToken = null;
        _staticUserId = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
        try { await SecureStorage.SetAsync(TOKEN_KEY, string.Empty); } catch { }
        try { await SecureStorage.SetAsync(USER_ID_KEY, string.Empty); } catch { }
    }

    private string GetBaseUrl()
    {
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            return "http://10.0.2.2:5175/api";
        }
        else if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            return "http://localhost:5175/api";
        }
        else
        {
            return "http://localhost:5175/api";
        }
    }

    public void SetToken(string token)
    {
        _authToken = token;
        _staticToken = token;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        try { SecureStorage.SetAsync(TOKEN_KEY, token).ConfigureAwait(false); } catch { /* ignore for Windows */ }
    }

    private StringContent SerializeContent(object obj) =>
        new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

    private async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        try
        {
            Debug.WriteLine($"Login attempt for user: {username}");
            var requestContent = new { Username = username, Password = password };
            var content = SerializeContent(requestContent);
            Debug.WriteLine($"Request URL: {_baseUrl}/api/users/login");
            Debug.WriteLine($"Request Content: {await content.ReadAsStringAsync()}");
            
            var response = await _httpClient.PostAsync("/api/users/login", content);
            Debug.WriteLine($"Response Status: {response.StatusCode}");
            
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await DeserializeResponse<LoginResponse>(response);
                Debug.WriteLine($"Login successful for user: {username}");
                if (!string.IsNullOrEmpty(loginResponse?.Token))
                {
                    SetToken(loginResponse.Token);
                    if (loginResponse.User?.Id != null)
                    {
                        try { await SecureStorage.SetAsync(USER_ID_KEY, loginResponse.User.Id.ToString()); } catch { }
                        _staticUserId = loginResponse.User.Id.ToString();
                        Debug.WriteLine($"Saved user ID: {loginResponse.User.Id}");
                    }
                    Debug.WriteLine("Token and user ID saved successfully");
                }
                return loginResponse?.User;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Login failed. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Login error: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        return null;
    }

    public async Task<User?> RegisterAsync(User user)
    {
        try
        {
            Debug.WriteLine($"Register attempt for user: {user.Username}");
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = null;
            }
            var content = SerializeContent(user);
            Debug.WriteLine($"Request URL: {_baseUrl}/api/users/register");
            Debug.WriteLine($"Request Content: {await content.ReadAsStringAsync()}");
            
            var response = await _httpClient.PostAsync("/api/users/register", content);
            Debug.WriteLine($"Response Status: {response.StatusCode}");
            
            if (response.IsSuccessStatusCode)
            {
                var registeredUser = await DeserializeResponse<User>(response);
                Debug.WriteLine($"Registration successful for user: {user.Username}");
                return registeredUser;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Registration failed. Status: {response.StatusCode}, Error: {errorContent}");
                throw new Exception($"Registration failed: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Register error: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    public async Task<bool> UpdatePasswordAsync(string email, string currentPassword, string newPassword)
    {
        try
        {
            Debug.WriteLine($"Updating password for user: {email}");
            var requestContent = new { Email = email, CurrentPassword = currentPassword, NewPassword = newPassword };
            var content = SerializeContent(requestContent);
            
            var response = await _httpClient.PutAsync("/api/users/update-password", content);
            Debug.WriteLine($"Update password response status: {response.StatusCode}");
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Update password failed. Error: {errorContent}");
            }
            
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UpdatePassword error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        try
        {
            Debug.WriteLine($"Sending forgot password request for: {email}");
            var requestContent = new { Email = email };
            var content = SerializeContent(requestContent);
            
            var response = await _httpClient.PostAsync("/api/users/forgot-password", content);
            Debug.WriteLine($"Forgot password response status: {response.StatusCode}");
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Forgot password failed. Error: {errorContent}");
            }
            
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ForgotPassword error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ResetPasswordAsync(string email, string resetCode, string newPassword)
    {
        try
        {
            Debug.WriteLine($"Resetting password for user: {email}");
            var requestContent = new { Email = email, ResetCode = resetCode, NewPassword = newPassword };
            var content = SerializeContent(requestContent);
            
            var response = await _httpClient.PostAsync("/api/users/reset-password", content);
            Debug.WriteLine($"Reset password response status: {response.StatusCode}");
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Reset password failed. Error: {errorContent}");
            }
            
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ResetPassword error: {ex.Message}");
            return false;
        }
    }

    public async Task<List<Movie>> GetMoviesAsync()
    {
        var response = await _httpClient.GetAsync("/api/movies");
        return response.IsSuccessStatusCode ? (await DeserializeResponse<List<Movie>>(response)) ?? new() : new();
    }

    public async Task<List<Movie>> SearchMoviesAsync(string query)
    {
        var response = await _httpClient.GetAsync($"/api/movies/search?query={Uri.EscapeDataString(query)}");
        return response.IsSuccessStatusCode ? (await DeserializeResponse<List<Movie>>(response)) ?? new() : new();
    }

    public async Task<Movie?> GetMovieByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/movies/{id}");
            return response.IsSuccessStatusCode ? await DeserializeResponse<Movie>(response) : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"GetMovieById error: {ex.Message}");
            return null;
        }
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/{id}");
            return response.IsSuccessStatusCode ? await DeserializeResponse<User>(response) : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"GetUserById error: {ex.Message}");
            return null;
        }
    }

    public async Task<UserRating?> GetUserRatingAsync(int userId, int movieId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/{userId}/ratings/{movieId}");
            return response.IsSuccessStatusCode ? await DeserializeResponse<UserRating>(response) : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"GetUserRating error: {ex.Message}");
            return null;
        }
    }

    public async Task<UserRating?> CreateUserRatingAsync(UserRating rating)
    {
        try
        {
            var response = await _httpClient.PostAsync($"/api/users/{rating.UserId}/ratings", SerializeContent(rating));
            return response.IsSuccessStatusCode ? await DeserializeResponse<UserRating>(response) : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"CreateUserRating error: {ex.Message}");
            return null;
        }
    }

    public async Task<UserRating?> UpdateUserRatingAsync(UserRating rating)
    {
        try
        {
            var response = await _httpClient.PutAsync($"/api/users/{rating.UserId}/ratings/{rating.MovieId}", SerializeContent(rating));
            return response.IsSuccessStatusCode ? await DeserializeResponse<UserRating>(response) : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UpdateUserRating error: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            Debug.WriteLine($"Testing connection to: {_baseUrl}");
            
            // Test users endpoint
            var usersResponse = await _httpClient.GetAsync("/api/users");
            Debug.WriteLine($"Users endpoint response: {usersResponse.StatusCode}");
            var usersContent = await usersResponse.Content.ReadAsStringAsync();
            Debug.WriteLine($"Users response content: {usersContent}");

            return usersResponse.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Test connection error: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            return false;
        }
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        try
        {
            Debug.WriteLine($"Update user attempt for user: {user.Username}");
            var content = SerializeContent(user);
            Debug.WriteLine($"Request URL: {_baseUrl}/api/users/{user.Id}");
            Debug.WriteLine($"Request Content: {await content.ReadAsStringAsync()}");
            
            var response = await _httpClient.PutAsync($"/api/users/{user.Id}", content);
            Debug.WriteLine($"Response Status: {response.StatusCode}");
            
            if (response.IsSuccessStatusCode)
            {
                var updatedUser = await DeserializeResponse<User>(response);
                Debug.WriteLine($"User update successful for user: {user.Username}");
                return updatedUser;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"User update failed. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UpdateUser error: {ex.Message}");
            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        return null;
    }

    public async Task<List<Movie>> GetTopRatedMoviesAsync(int count)
    {
        var response = await _httpClient.GetAsync($"/api/movies/top-rated?count={count}");
        return response.IsSuccessStatusCode ? (await DeserializeResponse<List<Movie>>(response)) ?? new() : new();
    }

    public async Task<List<Movie>> GetNewestMoviesAsync(int count)
    {
        var response = await _httpClient.GetAsync($"/api/movies/newest?count={count}");
        return response.IsSuccessStatusCode ? (await DeserializeResponse<List<Movie>>(response)) ?? new() : new();
    }

    public async Task<List<Movie>> GetPopularMoviesAsync(int count)
    {
        var response = await _httpClient.GetAsync($"/api/movies/popular?count={count}");
        return response.IsSuccessStatusCode ? (await DeserializeResponse<List<Movie>>(response)) ?? new() : new();
    }

    public async Task<List<Movie>> GetMoviesByGenreAsync(string genre)
    {
        var response = await _httpClient.GetAsync($"/api/movies/genre/{genre}");
        return response.IsSuccessStatusCode ? (await DeserializeResponse<List<Movie>>(response)) ?? new() : new();
    }

    public async Task<List<Movie>> GetMoviesByYearAsync(int year)
    {
        var response = await _httpClient.GetAsync($"/api/movies/year/{year}");
        return response.IsSuccessStatusCode ? (await DeserializeResponse<List<Movie>>(response)) ?? new() : new();
    }

    public async Task<List<Movie>> GetFamilyMoviesAsync()
    {
        var response = await _httpClient.GetAsync($"/api/movies/family");
        return response.IsSuccessStatusCode ? (await DeserializeResponse<List<Movie>>(response)) ?? new() : new();
    }

    public async Task<List<Movie>> GetAdultMoviesAsync()
    {
        var response = await _httpClient.GetAsync($"/api/movies/adult");
        return response.IsSuccessStatusCode ? (await DeserializeResponse<List<Movie>>(response)) ?? new() : new();
    }

    public async Task<List<Movie>> GetFavoriteMoviesAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"/api/users/{userId}/favorites");
        return response.IsSuccessStatusCode ? (await DeserializeResponse<List<Movie>>(response)) ?? new() : new();
    }

    public async Task<bool> AddToFavoritesAsync(int userId, int movieId)
    {
        var response = await _httpClient.PostAsync($"/api/users/{userId}/favorites/{movieId}", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveFromFavoritesAsync(int userId, int movieId)
    {
        var response = await _httpClient.DeleteAsync($"/api/users/{userId}/favorites/{movieId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> IsFavoriteAsync(int userId, int movieId)
    {
        var response = await _httpClient.GetAsync($"/api/users/{userId}/favorites/{movieId}/status");
        if (response.IsSuccessStatusCode)
        {
            var result = await DeserializeResponse<bool>(response);
            return result;
        }
        return false;
    }

    public async Task<List<UserRating>> GetUserRatingsAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"/api/users/{userId}/ratings");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<UserRating>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<UserRating>();
    }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public User? User { get; set; }
}

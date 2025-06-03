using MovieApp.Api.Models;

namespace MovieApp.Api.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> ValidatePasswordAsync(string username, string password);
    Task UpdateLastLoginAsync(int userId);
    Task<IEnumerable<User>> GetAllUsersAsync();
} 
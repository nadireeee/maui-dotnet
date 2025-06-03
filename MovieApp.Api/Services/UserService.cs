using Microsoft.EntityFrameworkCore;
using MovieApp.Api.Data;
using MovieApp.Api.Models;
using MovieApp.Api.Services.Interfaces;

namespace MovieApp.Api.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    private bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            return false;

        bool hasUpperCase = password.Any(char.IsUpper);
        bool hasLowerCase = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);

        return hasUpperCase && hasLowerCase && hasDigit;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        // Check if user with same email exists
        var existingUser = await GetUserByEmailAsync(user.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        // Check if user with same username exists
        existingUser = await GetUserByUsernameAsync(user.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with this username already exists.");
        }

        // Validate password complexity
        if (!IsValidPassword(user.PasswordHash))
        {
            throw new InvalidOperationException("Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, and one number.");
        }

        // Only hash the password if it's not already hashed
        if (!user.PasswordHash.StartsWith("$2"))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        }
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        // Check if password needs to be hashed
        if (!string.IsNullOrEmpty(user.PasswordHash) && !user.PasswordHash.StartsWith("$2"))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        }

        user.UpdatedAt = DateTime.UtcNow;
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ValidatePasswordAsync(string username, string password)
    {
        // Try to find user by username or email
        var user = await _context.Users.FirstOrDefaultAsync(u => 
            u.Username == username || u.Email == username);
        
        if (user == null)
            return false;

        // Use BCrypt to verify the password
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public async Task UpdateLastLoginAsync(int userId)
    {
        var user = await GetUserByIdAsync(userId);
        if (user != null)
        {
            user.LastLogin = DateTime.UtcNow;
            await UpdateUserAsync(user);
        }
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
} 
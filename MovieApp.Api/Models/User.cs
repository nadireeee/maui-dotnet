using System.ComponentModel.DataAnnotations;

namespace MovieApp.Api.Models;

/// <summary>
/// Represents a user in the system
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier for the user
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Username for login
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email address of the user
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password of the user
    /// </summary>
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// First name of the user
    /// </summary>
    [StringLength(50)]
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name of the user
    /// </summary>
    [StringLength(50)]
    public string? LastName { get; set; }

    /// <summary>
    /// Date when the user was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the user was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Date of the user's last login
    /// </summary>
    public DateTime? LastLogin { get; set; }

    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Whether the user is an administrator
    /// </summary>
    public bool IsAdmin { get; set; } = false;
} 
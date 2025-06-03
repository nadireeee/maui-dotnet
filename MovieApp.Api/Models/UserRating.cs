using System.ComponentModel.DataAnnotations;

namespace MovieApp.Api.Models;

/// <summary>
/// Represents a user's rating for a movie
/// </summary>
public class UserRating
{
    /// <summary>
    /// Unique identifier for the rating
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// ID of the user who gave the rating
    /// </summary>
    [Required]
    public int UserId { get; set; }
    
    /// <summary>
    /// User who gave the rating
    /// </summary>
    public virtual User? User { get; set; }
    
    /// <summary>
    /// ID of the movie being rated
    /// </summary>
    [Required]
    public int MovieId { get; set; }
    
    /// <summary>
    /// Movie being rated
    /// </summary>
    public virtual Movie? Movie { get; set; }
    
    /// <summary>
    /// Rating value (typically 1-5 or 1-10)
    /// </summary>
    [Required]
    [Range(1, 10)]
    public int Rating { get; set; }
    
    /// <summary>
    /// Optional review text
    /// </summary>
    [StringLength(1000)]
    public string? Review { get; set; }
    
    /// <summary>
    /// Date when the rating was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Date when the rating was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 
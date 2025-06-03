namespace MovieApp.Maui.Models;

public class UserRating
{
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public int Rating { get; set; }
    public string? Review { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public Movie? Movie { get; set; }
} 
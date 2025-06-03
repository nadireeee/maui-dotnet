namespace MovieApp.Api.Models;

public class FavoriteMovie
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual Movie? Movie { get; set; }
} 
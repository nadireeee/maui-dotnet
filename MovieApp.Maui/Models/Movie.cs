using System;

namespace MovieApp.Maui.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PosterUrl { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Duration { get; set; }
    public double Rating { get; set; }
    public string Overview { get; set; } = string.Empty;
    public string? PosterPath { get; set; }
    public DateTime ReleaseDate { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public string? OriginalLanguage { get; set; }
    public string? OriginalTitle { get; set; }
    public bool IsLocal { get; set; }
    public string? Director { get; set; }
    public string? Cast { get; set; }
    public string? Genres { get; set; }
    public int? Runtime { get; set; }
    public string? Tagline { get; set; }
    public string? Status { get; set; }
    public decimal? Budget { get; set; }
    public decimal? Revenue { get; set; }
    public string? ProductionCompanies { get; set; }
    public string? ProductionCountries { get; set; }
    public string? SpokenLanguages { get; set; }
    public string? Keywords { get; set; }
    public string? TrailerUrl { get; set; }
    public string? Homepage { get; set; }
    public bool IsAdult { get; set; }
    public double? Popularity { get; set; }
    public double? UserRatingAverage { get; set; }
    public int? UserRatingCount { get; set; }
    public int CreatedById { get; set; }
    public int? UpdatedById { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsSelected { get; set; }
} 
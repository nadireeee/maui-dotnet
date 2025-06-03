using MovieApp.Api.Models;

namespace MovieApp.Api.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Check if there are any users
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    Email = "admin@isubuflix.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Username = "user1",
                    Email = "user1@isubuflix.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Username = "user2",
                    Email = "user2@isubuflix.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User456!"),
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        // Check if there are any movies
        if (!context.Movies.Any())
        {
            var movies = new Movie[]
            {
                new Movie
                {
                    Title = "The Shawshank Redemption",
                    Overview = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                    ReleaseDate = new DateTime(1994, 9, 23),
                    VoteAverage = 8.7,
                    VoteCount = 24000,
                    Popularity = 83.5,
                    Budget = 25000000,
                    Revenue = 58800000,
                    Runtime = 142,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "The Shawshank Redemption",
                    Tagline = "Fear can hold you prisoner. Hope can set you free.",
                    Director = "Frank Darabont",
                    Cast = "Tim Robbins,Morgan Freeman",
                    Genres = "Drama",
                    Keywords = "prison,redemption,friendship,hope",
                    ProductionCompanies = "Castle Rock Entertainment",
                    ProductionCountries = "USA",
                    SpokenLanguages = "English",
                    Homepage = "https://www.warnerbros.com/movies/shawshank-redemption",
                    TrailerUrl = "https://www.youtube.com/watch?v=6hB3S9bIaco",
                    CreatedById = context.Users.First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "The Godfather",
                    Overview = "The aging patriarch of an organized crime dynasty transfers control to his son, who expands the family business.",
                    ReleaseDate = new DateTime(1972, 3, 14),
                    VoteAverage = 8.9,
                    VoteCount = 18000,
                    Popularity = 103.5,
                    Budget = 6000000,
                    Revenue = 245066411,
                    Runtime = 175,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "The Godfather",
                    Tagline = "An offer you can't refuse.",
                    Director = "Francis Ford Coppola",
                    Cast = "Marlon Brando,Al Pacino",
                    Genres = "Crime,Drama",
                    Keywords = "mafia,crime,family,power",
                    ProductionCompanies = "Paramount Pictures",
                    ProductionCountries = "USA",
                    SpokenLanguages = "English,Italian",
                    Homepage = "https://www.paramountmovies.com/movies/the-godfather",
                    TrailerUrl = "https://www.youtube.com/watch?v=sY1S34973zA",
                    CreatedById = context.Users.First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Eşkıya",
                    Overview = "A former bandit is released from prison after 35 years and returns to his village to find his girlfriend married to his best friend.",
                    ReleaseDate = new DateTime(1996, 1, 12),
                    VoteAverage = 8.5,
                    VoteCount = 5000,
                    Popularity = 65.2,
                    Budget = 1000000,
                    Revenue = 5000000,
                    Runtime = 128,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = true,
                    OriginalLanguage = "tr",
                    OriginalTitle = "Eşkıya",
                    Tagline = "35 yıl sonra...",
                    Director = "Yavuz Turgul",
                    Cast = "Şener Şen,Uğur Yücel",
                    Genres = "Drama,Crime",
                    Keywords = "bandit,prison,revenge,village,turkey",
                    ProductionCompanies = "Fida Film",
                    ProductionCountries = "Turkey",
                    SpokenLanguages = "Turkish",
                    Homepage = "https://example.com/eskıya",
                    TrailerUrl = "https://www.youtube.com/watch?v=example_eskıya",
                    CreatedById = context.Users.Skip(1).First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Babam ve Oğlum",
                    Overview = "A father and son reunite after years of separation due to political differences.",
                    ReleaseDate = new DateTime(2005, 11, 18),
                    VoteAverage = 8.7,
                    VoteCount = 6000,
                    Popularity = 68.5,
                    Budget = 1500000,
                    Revenue = 8000000,
                    Runtime = 112,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = true,
                    OriginalLanguage = "tr",
                    OriginalTitle = "Babam ve Oğlum",
                    Tagline = "Bazen hayat, beklediğiniz gibi olmayabilir...",
                    Director = "Çağan Irmak",
                    Cast = "Fikret Kuşkan,Çetin Tekindor",
                    Genres = "Drama,Family",
                    Keywords = "family,father,son,reconciliation,turkey",
                    ProductionCompanies = "Most Production",
                    ProductionCountries = "Turkey",
                    SpokenLanguages = "Turkish",
                    Homepage = "https://example.com/babamveoglum",
                    TrailerUrl = "https://www.youtube.com/watch?v=example_babamveoglum",
                    CreatedById = context.Users.Skip(1).First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Vizontele",
                    Overview = "The arrival of the first television in a small Turkish village causes excitement and chaos.",
                    ReleaseDate = new DateTime(2001, 2, 9),
                    VoteAverage = 8.3,
                    VoteCount = 4500,
                    Popularity = 62.8,
                    Budget = 2000000,
                    Revenue = 10000000,
                    Runtime = 110,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = true,
                    OriginalLanguage = "tr",
                    OriginalTitle = "Vizontele",
                    Tagline = "Bir köyde televizyonun gelişi...",
                    Director = "Yılmaz Erdoğan",
                    Cast = "Yılmaz Erdoğan,Demet Akbaş",
                    Genres = "Comedy,Drama",
                    Keywords = "television,village,comedy,turkey,1960s",
                    ProductionCompanies = "BKM",
                    ProductionCountries = "Turkey",
                    SpokenLanguages = "Turkish",
                    Homepage = "https://example.com/vizontele",
                    TrailerUrl = "https://www.youtube.com/watch?v=example_vizontele",
                    CreatedById = context.Users.Skip(2).First().Id,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Movies.AddRange(movies);
            context.SaveChanges();

            // Add sample favorite movies
            var favoriteMovies = new FavoriteMovie[]
            {
                new FavoriteMovie
                {
                    UserId = context.Users.First().Id,
                    MovieId = movies[0].Id,
                    CreatedAt = DateTime.UtcNow
                },
                new FavoriteMovie
                {
                    UserId = context.Users.Skip(1).First().Id,
                    MovieId = movies[1].Id,
                    CreatedAt = DateTime.UtcNow
                },
                new FavoriteMovie
                {
                    UserId = context.Users.Skip(2).First().Id,
                    MovieId = movies[2].Id,
                    CreatedAt = DateTime.UtcNow
                },
                new FavoriteMovie
                {
                    UserId = context.Users.Skip(2).First().Id,
                    MovieId = movies[3].Id,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.FavoriteMovies.AddRange(favoriteMovies);
            context.SaveChanges();

            // Add sample user ratings
            var userRatings = new UserRating[]
            {
                new UserRating
                {
                    UserId = context.Users.First().Id,
                    MovieId = movies[0].Id,
                    Rating = 5,
                    Review = "One of the best movies ever made!",
                    CreatedAt = DateTime.UtcNow
                },
                new UserRating
                {
                    UserId = context.Users.Skip(1).First().Id,
                    MovieId = movies[1].Id,
                    Rating = 5,
                    Review = "A masterpiece of cinema.",
                    CreatedAt = DateTime.UtcNow
                },
                new UserRating
                {
                    UserId = context.Users.Skip(2).First().Id,
                    MovieId = movies[2].Id,
                    Rating = 5,
                    Review = "A classic Turkish film with great performances.",
                    CreatedAt = DateTime.UtcNow
                },
                new UserRating
                {
                    UserId = context.Users.Skip(2).First().Id,
                    MovieId = movies[3].Id,
                    Rating = 5,
                    Review = "A touching story about family relationships.",
                    CreatedAt = DateTime.UtcNow
                },
                new UserRating
                {
                    UserId = context.Users.First().Id,
                    MovieId = movies[4].Id,
                    Rating = 4,
                    Review = "A funny and nostalgic look at Turkish village life.",
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.UserRatings.AddRange(userRatings);
            context.SaveChanges();
        }
    }
} 
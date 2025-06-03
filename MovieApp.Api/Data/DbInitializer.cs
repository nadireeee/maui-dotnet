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
                    PosterPath = "https://image.tmdb.org/t/p/w500/q6y0Go1tsGEsmtFryDOJo3dEmqu.jpg",
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
                    PosterPath = "https://image.tmdb.org/t/p/w500/3bhkrj58Vtu7enYsRolD1fZdja1.jpg",
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
                    PosterPath = "https://upload.wikimedia.org/wikipedia/tr/f/f5/Eskiya_film.jpg",
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
                    PosterPath = "https://upload.wikimedia.org/wikipedia/tr/e/e5/Babam_ve_O%C4%9Flum.jpg",
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
                    Title = "The Prestige",
                    Overview = "After a tragic accident, two stage magicians engage in a battle to create the ultimate illusion while sacrificing everything they have to outwit each other.",
                    PosterPath = "https://image.tmdb.org/t/p/w500/5MXyQfz8xUP3dIFPTubhTsbFY6N.jpg",
                    ReleaseDate = new DateTime(2006, 10, 20),
                    VoteAverage = 8.5,
                    VoteCount = 13000,
                    Popularity = 70.0,
                    Budget = 40000000,
                    Revenue = 109676311,
                    Runtime = 130,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "The Prestige",
                    Tagline = "Are You Watching Closely?",
                    Director = "Christopher Nolan",
                    Cast = "Christian Bale,Hugh Jackman,Scarlett Johansson",
                    Genres = "Drama,Mystery,Sci-Fi",
                    Keywords = "magic,illusion,obsession,competition",
                    ProductionCompanies = "Touchstone Pictures,Warner Bros.",
                    ProductionCountries = "USA,UK",
                    SpokenLanguages = "English",
                    Homepage = "https://www.warnerbros.com/movies/prestige",
                    TrailerUrl = "https://www.youtube.com/watch?v=o4gHCmTQDVI",
                    CreatedById = context.Users.First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Inception",
                    Overview = "A thief who steals corporate secrets through dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                    PosterPath = "https://image.tmdb.org/t/p/w500/9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg",
                    ReleaseDate = new DateTime(2010, 7, 16),
                    VoteAverage = 8.8,
                    VoteCount = 32000,
                    Popularity = 95.5,
                    Budget = 160000000,
                    Revenue = 836836967,
                    Runtime = 148,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "Inception",
                    Tagline = "Your mind is the scene of the crime.",
                    Director = "Christopher Nolan",
                    Cast = "Leonardo DiCaprio,Joseph Gordon-Levitt",
                    Genres = "Action,Sci-Fi,Thriller",
                    Keywords = "dreams,subconscious,mind,heist",
                    ProductionCompanies = "Warner Bros. Pictures",
                    ProductionCountries = "USA,UK",
                    SpokenLanguages = "English,Japanese,French",
                    Homepage = "https://www.warnerbros.com/movies/inception",
                    TrailerUrl = "https://www.youtube.com/watch?v=YoHD9XEInc0",
                    CreatedById = context.Users.First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "The Dark Knight",
                    Overview = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                    PosterPath = "https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg",
                    ReleaseDate = new DateTime(2008, 7, 18),
                    VoteAverage = 9.0,
                    VoteCount = 28000,
                    Popularity = 98.5,
                    Budget = 185000000,
                    Revenue = 1004558444,
                    Runtime = 152,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "The Dark Knight",
                    Tagline = "Why So Serious?",
                    Director = "Christopher Nolan",
                    Cast = "Christian Bale,Heath Ledger",
                    Genres = "Action,Crime,Drama",
                    Keywords = "batman,joker,crime,gotham",
                    ProductionCompanies = "Warner Bros. Pictures",
                    ProductionCountries = "USA,UK",
                    SpokenLanguages = "English",
                    Homepage = "https://www.warnerbros.com/movies/dark-knight",
                    TrailerUrl = "https://www.youtube.com/watch?v=EXeTwQWrcwY",
                    CreatedById = context.Users.First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Pulp Fiction",
                    Overview = "The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption.",
                    PosterPath = "https://image.tmdb.org/t/p/w500/d5iIlFn5s0ImszYzBPb8JPIfbXD.jpg",
                    ReleaseDate = new DateTime(1994, 10, 14),
                    VoteAverage = 8.9,
                    VoteCount = 22000,
                    Popularity = 88.5,
                    Budget = 8000000,
                    Revenue = 213928762,
                    Runtime = 154,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "Pulp Fiction",
                    Tagline = "Just because you are a character doesn't mean you have character.",
                    Director = "Quentin Tarantino",
                    Cast = "John Travolta,Samuel L. Jackson",
                    Genres = "Crime,Drama",
                    Keywords = "violence,crime,redemption,nonlinear",
                    ProductionCompanies = "Miramax Films",
                    ProductionCountries = "USA",
                    SpokenLanguages = "English,Spanish,French",
                    Homepage = "https://www.miramax.com/movie/pulp-fiction/",
                    TrailerUrl = "https://www.youtube.com/watch?v=s7EdQ4FqbhY",
                    CreatedById = context.Users.First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Forrest Gump",
                    Overview = "The presidencies of Kennedy and Johnson, the Vietnam War, the Watergate scandal and other historical events unfold from the perspective of an Alabama man with an IQ of 75, whose only desire is to be reunited with his childhood sweetheart.",
                    PosterPath = "https://image.tmdb.org/t/p/w500/arw2vcBveWOVZr6pxd9XTd1TdQa.jpg",
                    ReleaseDate = new DateTime(1994, 7, 6),
                    VoteAverage = 8.8,
                    VoteCount = 24000,
                    Popularity = 85.5,
                    Budget = 55000000,
                    Revenue = 677945399,
                    Runtime = 142,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "Forrest Gump",
                    Tagline = "The world will never be the same once you've seen it through the eyes of Forrest Gump.",
                    Director = "Robert Zemeckis",
                    Cast = "Tom Hanks,Robin Wright",
                    Genres = "Drama,Romance",
                    Keywords = "history,war,disability,america",
                    ProductionCompanies = "Paramount Pictures",
                    ProductionCountries = "USA",
                    SpokenLanguages = "English",
                    Homepage = "https://www.paramountmovies.com/movies/forrest-gump",
                    TrailerUrl = "https://www.youtube.com/watch?v=bLvqoHBptjg",
                    CreatedById = context.Users.First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "The Matrix",
                    Overview = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                    PosterPath = "https://image.tmdb.org/t/p/w500/f89U3ADr1oiB1s9GkdPOEpXUk5H.jpg",
                    ReleaseDate = new DateTime(1999, 3, 31),
                    VoteAverage = 8.7,
                    VoteCount = 22000,
                    Popularity = 82.5,
                    Budget = 63000000,
                    Revenue = 463517383,
                    Runtime = 136,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "The Matrix",
                    Tagline = "Welcome to the Real World.",
                    Director = "Lana Wachowski",
                    Cast = "Keanu Reeves,Laurence Fishburne",
                    Genres = "Action,Sci-Fi",
                    Keywords = "virtual reality,computer,reality,philosophy",
                    ProductionCompanies = "Warner Bros. Pictures",
                    ProductionCountries = "USA,Australia",
                    SpokenLanguages = "English",
                    Homepage = "https://www.warnerbros.com/movies/matrix",
                    TrailerUrl = "https://www.youtube.com/watch?v=vKQi3bBA1y8",
                    CreatedById = context.Users.First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Goodfellas",
                    Overview = "The story of Henry Hill and his life in the mob, covering his relationship with his wife Karen Hill and his mob partners Jimmy Conway and Tommy DeVito in the Italian-American crime syndicate.",
                    PosterPath = "https://image.tmdb.org/t/p/w500/aKuFiU82s5ISJpGZp7YkIr3kCUd.jpg",
                    ReleaseDate = new DateTime(1990, 9, 19),
                    VoteAverage = 8.7,
                    VoteCount = 11000,
                    Popularity = 75.5,
                    Budget = 25000000,
                    Revenue = 46836194,
                    Runtime = 146,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "Goodfellas",
                    Tagline = "Three Decades of Life in the Mafia.",
                    Director = "Martin Scorsese",
                    Cast = "Robert De Niro,Ray Liotta",
                    Genres = "Crime,Drama",
                    Keywords = "mafia,crime,gangster,biography",
                    ProductionCompanies = "Warner Bros. Pictures",
                    ProductionCountries = "USA",
                    SpokenLanguages = "English,Italian",
                    Homepage = "https://www.warnerbros.com/movies/goodfellas",
                    TrailerUrl = "https://www.youtube.com/watch?v=2ilzidi_J8Q",
                    CreatedById = context.Users.First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "The Silence of the Lambs",
                    Overview = "A young F.B.I. cadet must receive the help of an incarcerated and manipulative cannibal killer to help catch another serial killer, a madman who skins his victims.",
                    PosterPath = "https://image.tmdb.org/t/p/w500/rplLJ2hPcOQmkFhTqUte0MkEaO2.jpg",
                    ReleaseDate = new DateTime(1991, 2, 14),
                    VoteAverage = 8.6,
                    VoteCount = 13000,
                    Popularity = 78.5,
                    Budget = 19000000,
                    Revenue = 272742922,
                    Runtime = 118,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "The Silence of the Lambs",
                    Tagline = "To enter the mind of a killer she must challenge the mind of a madman.",
                    Director = "Jonathan Demme",
                    Cast = "Jodie Foster,Anthony Hopkins",
                    Genres = "Crime,Drama,Thriller",
                    Keywords = "serial killer,fbi,psychology,cannibalism",
                    ProductionCompanies = "Orion Pictures",
                    ProductionCountries = "USA",
                    SpokenLanguages = "English",
                    Homepage = "https://www.mgm.com/movies/the-silence-of-the-lambs",
                    TrailerUrl = "https://www.youtube.com/watch?v=W6Mm8Sbe__o",
                    CreatedById = context.Users.First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Fight Club",
                    Overview = "An insomniac office worker and a devil-may-care soapmaker form an underground fight club that evolves into something much, much more.",
                    PosterPath = "https://image.tmdb.org/t/p/w500/pB8BM7pdSp6B6Ih7QZ4DrQ3PmJK.jpg",
                    ReleaseDate = new DateTime(1999, 10, 15),
                    VoteAverage = 8.8,
                    VoteCount = 24000,
                    Popularity = 88.5,
                    Budget = 63000000,
                    Revenue = 100853753,
                    Runtime = 139,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "Fight Club",
                    Tagline = "Mischief. Mayhem. Soap.",
                    Director = "David Fincher",
                    Cast = "Brad Pitt,Edward Norton",
                    Genres = "Drama",
                    Keywords = "violence,psychology,identity,rebellion",
                    ProductionCompanies = "20th Century Fox",
                    ProductionCountries = "USA,Germany",
                    SpokenLanguages = "English",
                    Homepage = "https://www.foxmovies.com/movies/fight-club",
                    TrailerUrl = "https://www.youtube.com/watch?v=SUXWAEX2jlg",
                    CreatedById = context.Users.First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "The Lord of the Rings: The Fellowship of the Ring",
                    Overview = "A meek Hobbit from the Shire and eight companions set out on a journey to destroy the powerful One Ring and save Middle-earth from the Dark Lord Sauron.",
                    PosterPath = "https://image.tmdb.org/t/p/w500/6oom5QYQ2yQTMJIbnvbkBL9cHo6.jpg",
                    ReleaseDate = new DateTime(2001, 12, 19),
                    VoteAverage = 8.8,
                    VoteCount = 22000,
                    Popularity = 92.5,
                    Budget = 93000000,
                    Revenue = 871530324,
                    Runtime = 178,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "The Lord of the Rings: The Fellowship of the Ring",
                    Tagline = "One ring to rule them all",
                    Director = "Peter Jackson",
                    Cast = "Elijah Wood,Ian McKellen",
                    Genres = "Adventure,Fantasy,Action",
                    Keywords = "fantasy,adventure,quest,middle-earth",
                    ProductionCompanies = "New Line Cinema",
                    ProductionCountries = "USA,New Zealand",
                    SpokenLanguages = "English",
                    Homepage = "https://www.warnerbros.com/movies/lord-rings-fellowship-ring",
                    TrailerUrl = "https://www.youtube.com/watch?v=V75dMMIW2B4",
                    CreatedById = context.Users.First().Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Movie
                {
                    Title = "Interstellar",
                    Overview = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
                    PosterPath = "https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg",
                    ReleaseDate = new DateTime(2014, 11, 7),
                    VoteAverage = 8.6,
                    VoteCount = 28000,
                    Popularity = 90.5,
                    Budget = 165000000,
                    Revenue = 701729206,
                    Runtime = 169,
                    Status = "Released",
                    IsAdult = false,
                    IsLocal = false,
                    OriginalLanguage = "en",
                    OriginalTitle = "Interstellar",
                    Tagline = "Mankind was born on Earth. It was never meant to die here.",
                    Director = "Christopher Nolan",
                    Cast = "Matthew McConaughey,Anne Hathaway",
                    Genres = "Adventure,Drama,Sci-Fi",
                    Keywords = "space,time,wormhole,apocalypse",
                    ProductionCompanies = "Paramount Pictures",
                    ProductionCountries = "USA,UK,Canada",
                    SpokenLanguages = "English",
                    Homepage = "https://www.paramountmovies.com/movies/interstellar",
                    TrailerUrl = "https://www.youtube.com/watch?v=zSWdZVtXT7E",
                    CreatedById = context.Users.First().Id,
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
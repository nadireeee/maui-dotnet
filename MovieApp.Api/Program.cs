using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieApp.Api.Data;
using MovieApp.Api.Models;
using MovieApp.Api.Services;
using MovieApp.Api.Services.Interfaces;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using BCrypt.Net;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMauiApp",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "MovieApp API", 
        Version = "v1",
        Description = "A comprehensive movie management API that provides functionality for user management, movie browsing, favorites, and ratings.",
        Contact = new OpenApiContact
        {
            Name = "MovieApp Support",
            Email = "support@movieapp.com"
        },
        License = new OpenApiLicense
        {
            Name = "MovieApp License"
        }
    });

    // Add security definition for JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Add XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Add operation filters for better documentation
    c.OperationFilter<SwaggerOperationFilter>();
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IFavoriteMovieService, FavoriteMovieService>();
builder.Services.AddScoped<IUserRatingService, UserRatingService>();

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieApp API V1");
    });
}

// Add test endpoints
app.MapGet("/test", () => "API is running!");
app.MapGet("/api/test", () => "API test endpoint is working!");

// Use CORS before other middleware
app.UseCors("AllowMauiApp");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// User endpoints
app.MapPost("/api/users/register", async (User user, IUserService userService) =>
{
    try
    {
        var createdUser = await userService.CreateUserAsync(user);
        return Results.Created($"/api/users/{createdUser.Id}", createdUser);
    }
    catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Users_Email") == true)
    {
        return Results.BadRequest(new { Error = "A user with this email already exists." });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { Error = "An error occurred while registering the user." });
    }
});

app.MapPost("/api/users/login", async (LoginRequest request, IUserService userService, IConfiguration configuration) =>
{
    var isValid = await userService.ValidatePasswordAsync(request.Username, request.Password);
    if (!isValid)
        return Results.Unauthorized();

    var user = await userService.GetUserByUsernameAsync(request.Username);
    if (user == null)
        return Results.NotFound();

    await userService.UpdateLastLoginAsync(user.Id);

    // Generate JWT Token
    var token = new JwtSecurityToken(
        issuer: configuration["Jwt:Issuer"],
        audience: configuration["Jwt:Audience"],
        claims: new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        },
        expires: DateTime.UtcNow.AddDays(7),
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
            SecurityAlgorithms.HmacSha256)
    );

    return Results.Ok(new
    {
        Token = new JwtSecurityTokenHandler().WriteToken(token),
        User = user
    });
});

// Add new user management endpoints
app.MapGet("/api/users", async (IUserService userService) =>
{
    try
    {
        Console.WriteLine("GET /api/users endpoint called");
        var users = await userService.GetAllUsersAsync();
        Console.WriteLine($"Found {users.Count()} users");
        return Results.Ok(users);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GET /api/users: {ex.Message}");
        return Results.BadRequest(new { Error = "An error occurred while retrieving users.", Details = ex.Message });
    }
});

app.MapGet("/api/users/{id}", async (int id, IUserService userService) =>
{
    try
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user == null)
            return Results.NotFound();
        return Results.Ok(user);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GET /api/users/{id}: {ex.Message}");
        return Results.BadRequest(new { Error = "An error occurred while retrieving the user.", Details = ex.Message });
    }
});

app.MapPut("/api/users/{id}", async (int id, User user, IUserService userService) =>
{
    try
    {
        if (id != user.Id)
            return Results.BadRequest(new { Error = "ID mismatch" });
            
        var updatedUser = await userService.UpdateUserAsync(user);
        return Results.Ok(updatedUser);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in PUT /api/users/{id}: {ex.Message}");
        return Results.BadRequest(new { Error = "An error occurred while updating the user.", Details = ex.Message });
    }
});

app.MapDelete("/api/users/{id}", async (int id, IUserService userService) =>
{
    try
    {
        var result = await userService.DeleteUserAsync(id);
        if (!result)
            return Results.NotFound();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in DELETE /api/users/{id}: {ex.Message}");
        return Results.BadRequest(new { Error = "An error occurred while deleting the user.", Details = ex.Message });
    }
});

app.MapPut("/api/users/update-password", async (UpdatePasswordRequest request, IUserService userService) =>
{
    try
    {
        var user = await userService.GetUserByEmailAsync(request.Email);
        if (user == null)
            return Results.NotFound(new { Error = "User not found" });
            
        var isValid = await userService.ValidatePasswordAsync(user.Username, request.CurrentPassword);
        if (!isValid)
            return Results.BadRequest(new { Error = "Current password is incorrect" });
            
        // Hash the new password before saving
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await userService.UpdateUserAsync(user);
        return Results.Ok(new { Message = "Password updated successfully" });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in PUT /api/users/update-password: {ex.Message}");
        return Results.BadRequest(new { Error = "An error occurred while updating the password.", Details = ex.Message });
    }
});

// Movie endpoints
app.MapGet("/api/movies", async (IMovieService movieService) =>
{
    try
    {
        Console.WriteLine("GET /api/movies endpoint called");
        var movies = await movieService.GetAllMoviesAsync();
        Console.WriteLine($"Found {movies.Count()} movies");
        return Results.Ok(movies);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GET /api/movies: {ex.Message}");
        return Results.BadRequest(new { Error = "An error occurred while retrieving movies.", Details = ex.Message });
    }
});

app.MapGet("/api/movies/{id}", async (int id, IMovieService movieService) =>
{
    try
    {
        var movie = await movieService.GetMovieByIdAsync(id);
        if (movie == null)
            return Results.NotFound();
        return Results.Ok(movie);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GET /api/movies/{id}: {ex.Message}");
        return Results.BadRequest(new { Error = "An error occurred while retrieving the movie.", Details = ex.Message });
    }
});

app.MapPost("/api/movies", async (Movie movie, IUserService userService, IMovieService movieService) =>
{
    try
    {
        // Get user from token
        var userId = 1; // This should come from the token
        var createdMovie = await movieService.CreateMovieAsync(movie, userId);
        return Results.Created($"/api/movies/{createdMovie.Id}", createdMovie);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in POST /api/movies: {ex.Message}");
        return Results.BadRequest(new { Error = "An error occurred while creating the movie.", Details = ex.Message });
    }
});

app.MapPut("/api/movies/{id}", async (int id, Movie movie, IUserService userService, IMovieService movieService) =>
{
    try
    {
        // Get user from token
        var userId = 1; // This should come from the token
        var updatedMovie = await movieService.UpdateMovieAsync(movie, userId);
        return Results.Ok(updatedMovie);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in PUT /api/movies/{id}: {ex.Message}");
        return Results.BadRequest(new { Error = "An error occurred while updating the movie.", Details = ex.Message });
    }
});

app.MapDelete("/api/movies/{id}", async (int id, IMovieService movieService) =>
{
    try
    {
        var result = await movieService.DeleteMovieAsync(id);
        if (!result)
            return Results.NotFound();
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in DELETE /api/movies/{id}: {ex.Message}");
        return Results.BadRequest(new { Error = "An error occurred while deleting the movie.", Details = ex.Message });
    }
});

// Favorite movies endpoints
app.MapGet("/api/users/{userId}/favorites", async (int userId, IMovieService movieService) =>
{
    var favorites = await movieService.GetFavoriteMoviesAsync(userId);
    return Results.Ok(favorites);
});

app.MapPost("/api/users/{userId}/favorites/{movieId}", async (int userId, int movieId, IMovieService movieService) =>
{
    var result = await movieService.AddToFavoritesAsync(userId, movieId);
    if (!result)
        return Results.BadRequest();
    return Results.NoContent();
});

app.MapDelete("/api/users/{userId}/favorites/{movieId}", async (int userId, int movieId, IMovieService movieService) =>
{
    var result = await movieService.RemoveFromFavoritesAsync(userId, movieId);
    if (!result)
        return Results.NotFound();
    return Results.NoContent();
});

// User rating endpoints
app.MapGet("/api/movies/{movieId}/ratings", async (int movieId, IUserRatingService userRatingService) =>
{
    var ratings = await userRatingService.GetMovieRatingsAsync(movieId);
    return Results.Ok(ratings);
});

app.MapGet("/api/users/{userId}/ratings", async (int userId, IUserRatingService userRatingService) =>
{
    var ratings = await userRatingService.GetUserRatingsAsync(userId);
    return Results.Ok(ratings);
});

app.MapGet("/api/users/{userId}/ratings/{movieId}", async (int userId, int movieId, IUserRatingService userRatingService) =>
{
    var rating = await userRatingService.GetUserRatingAsync(userId, movieId);
    if (rating == null)
        return Results.NotFound();
    return Results.Ok(rating);
});

app.MapPost("/api/users/{userId}/ratings", async (int userId, UserRating userRating, IUserRatingService userRatingService) =>
{
    userRating.UserId = userId;
    var createdRating = await userRatingService.CreateUserRatingAsync(userRating);
    return Results.Created($"/api/users/{userId}/ratings/{createdRating.MovieId}", createdRating);
});

app.MapPut("/api/users/{userId}/ratings/{movieId}", async (int userId, int movieId, UserRating userRating, IUserRatingService userRatingService) =>
{
    userRating.UserId = userId;
    userRating.MovieId = movieId;
    var updatedRating = await userRatingService.UpdateUserRatingAsync(userRating);
    return Results.Ok(updatedRating);
});

app.MapDelete("/api/users/{userId}/ratings/{movieId}", async (int userId, int movieId, IUserRatingService userRatingService) =>
{
    var result = await userRatingService.DeleteUserRatingAsync(userId, movieId);
    if (!result)
        return Results.NotFound();
    return Results.NoContent();
});

// Advanced movie filtering endpoints
app.MapGet("/api/movies/genre/{genre}", async (string genre, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByGenreAsync(genre);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/local", async (IMovieService movieService) =>
{
    var movies = await movieService.GetLocalMoviesAsync();
    return Results.Ok(movies);
});

app.MapGet("/api/movies/foreign", async (IMovieService movieService) =>
{
    var movies = await movieService.GetForeignMoviesAsync();
    return Results.Ok(movies);
});

app.MapGet("/api/movies/year/{year}", async (int year, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByYearAsync(year);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/rating", async (double minRating, double maxRating, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByRatingRangeAsync(minRating, maxRating);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/director/{director}", async (string director, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByDirectorAsync(director);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/actor/{actor}", async (string actor, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByActorAsync(actor);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/top-rated", async (int count, IMovieService movieService) =>
{
    var movies = await movieService.GetTopRatedMoviesAsync(count);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/newest", async (int count, IMovieService movieService) =>
{
    var movies = await movieService.GetNewestMoviesAsync(count);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/popular", async (int count, IMovieService movieService) =>
{
    var movies = await movieService.GetPopularMoviesAsync(count);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/language/{language}", async (string language, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByLanguageAsync(language);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/adult", async (IMovieService movieService) =>
{
    var movies = await movieService.GetAdultMoviesAsync();
    return Results.Ok(movies);
});

app.MapGet("/api/movies/family", async (IMovieService movieService) =>
{
    var movies = await movieService.GetFamilyMoviesAsync();
    return Results.Ok(movies);
});

app.MapGet("/api/movies/keywords", async (string[] keywords, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByKeywordsAsync(keywords);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/company/{company}", async (string company, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByProductionCompanyAsync(company);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/country/{country}", async (string country, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByProductionCountryAsync(country);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/status/{status}", async (string status, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByStatusAsync(status);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/budget", async (decimal minBudget, decimal maxBudget, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByBudgetRangeAsync(minBudget, maxBudget);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/revenue", async (decimal minRevenue, decimal maxRevenue, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByRevenueRangeAsync(minRevenue, maxRevenue);
    return Results.Ok(movies);
});

app.MapGet("/api/movies/runtime", async (int minRuntime, int maxRuntime, IMovieService movieService) =>
{
    var movies = await movieService.GetMoviesByRuntimeRangeAsync(minRuntime, maxRuntime);
    return Results.Ok(movies);
});

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();

public record LoginRequest(string Username, string Password);

public class SwaggerOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation == null || context == null)
            return;

        // Add descriptions for each operation
        if (context.ApiDescription.RelativePath?.StartsWith("api/users/register") == true)
        {
            operation.Summary = "Register a new user";
            operation.Description = "Creates a new user account with the provided information.";
        }
        else if (context.ApiDescription.RelativePath?.StartsWith("api/users/login") == true)
        {
            operation.Summary = "User login";
            operation.Description = "Authenticates a user and returns a JWT token for subsequent requests.";
        }
        else if (context.ApiDescription.RelativePath?.StartsWith("api/movies") == true)
        {
            operation.Summary = "Movie operations";
            operation.Description = "Operations related to movie management, including CRUD operations and advanced filtering.";
        }
        else if (context.ApiDescription.RelativePath?.Contains("favorites") == true)
        {
            operation.Summary = "Favorite movies";
            operation.Description = "Operations for managing user's favorite movies.";
        }
        else if (context.ApiDescription.RelativePath?.Contains("ratings") == true)
        {
            operation.Summary = "Movie ratings";
            operation.Description = "Operations for managing user ratings of movies.";
        }

        // Add response descriptions
        if (!operation.Responses.ContainsKey("200"))
        {
            operation.Responses.Add("200", new OpenApiResponse { Description = "Success" });
        }
        if (!operation.Responses.ContainsKey("400"))
        {
            operation.Responses.Add("400", new OpenApiResponse { Description = "Bad Request" });
        }
        if (!operation.Responses.ContainsKey("401"))
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        }
        if (!operation.Responses.ContainsKey("404"))
        {
            operation.Responses.Add("404", new OpenApiResponse { Description = "Not Found" });
        }
    }
}

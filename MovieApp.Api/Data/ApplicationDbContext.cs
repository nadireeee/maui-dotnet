using Microsoft.EntityFrameworkCore;
using MovieApp.Api.Models;

namespace MovieApp.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<FavoriteMovie> FavoriteMovies { get; set; }
    public DbSet<UserRating> UserRatings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure decimal precision for Movie properties
        modelBuilder.Entity<Movie>()
            .Property(m => m.Budget)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Movie>()
            .Property(m => m.Revenue)
            .HasPrecision(18, 2);

        // Configure unique constraints
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Configure FavoriteMovie relationships
        modelBuilder.Entity<FavoriteMovie>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Movie)
                .WithMany()
                .HasForeignKey(e => e.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.UserId, e.MovieId })
                .IsUnique();
        });

        // Configure UserRating relationships
        modelBuilder.Entity<UserRating>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Movie)
                .WithMany()
                .HasForeignKey(e => e.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.UserId, e.MovieId })
                .IsUnique();
        });

        // Configure Movie relationships
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasOne(m => m.CreatedBy)
                .WithMany()
                .HasForeignKey(m => m.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.UpdatedBy)
                .WithMany()
                .HasForeignKey(m => m.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
} 
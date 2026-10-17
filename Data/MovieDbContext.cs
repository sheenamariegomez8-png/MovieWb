using Microsoft.EntityFrameworkCore;
using movieweb.Models;

namespace movieweb.Data;

public sealed class MovieDbContext(DbContextOptions<MovieDbContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movies", "dbo");
            entity.HasKey(movie => movie.MovieID);
            entity.Property(movie => movie.MovieTitle).HasMaxLength(200).IsRequired();
            entity.Property(movie => movie.Genre).HasMaxLength(100).IsRequired();
            entity.Property(movie => movie.DatePublished).HasColumnType("date").IsRequired();
            entity.Property(movie => movie.Director).HasMaxLength(150).IsRequired();
            entity.Property(movie => movie.Duration).IsRequired();
            entity.Property(movie => movie.Rating).HasPrecision(3, 1).IsRequired();
            entity.Property(movie => movie.Synopsis).HasColumnType("nvarchar(max)").IsRequired();
        });
    }
}

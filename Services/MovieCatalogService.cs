using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using movieweb.Data;
using movieweb.Models;

namespace movieweb.Services;

public sealed class MovieCatalogService(
    MovieDbContext dbContext,
    ILogger<MovieCatalogService> logger)
{
    private static readonly IReadOnlyDictionary<string, string> PosterLookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        [NormalizeMovieTitle("Midnight Sun")] = "/Midnight%20Sun.jpg",
        [NormalizeMovieTitle("Alive")] = "/Alive.jpg",
        [NormalizeMovieTitle("Five Feet Apart")] = "/Five%20Feet%20Apart.jpg",
        [NormalizeMovieTitle("Love Untangled")] = "/Love%20Untangled.jpg",
        [NormalizeMovieTitle("Me Before You")] = "/Me%20Before%20You.jpg",
        [NormalizeMovieTitle("Pawn")] = "/Pawn.jpg",
        [NormalizeMovieTitle("Titanic")] = "/Titanic.jpg",
        [NormalizeMovieTitle("The Notebook")] = "/The%20Notebook.jpg",
        [NormalizeMovieTitle("People We Meet on Vacation")] = "/PEOPLE-WE-MEET-ON-VACATION-.jpg",
        [NormalizeMovieTitle("The Fault in Our Stars")] = "/lib/THE%20Fault%20In%20Our%20Stars.jpg"
    };

    public string? GetPosterUrl(string? movieTitle)
    {
        if (string.IsNullOrWhiteSpace(movieTitle))
        {
            return null;
        }

        var normalizedTitle = NormalizeMovieTitle(movieTitle);
        return PosterLookup.TryGetValue(normalizedTitle, out var posterPath)
            ? posterPath
            : null;
    }

    private static string NormalizeMovieTitle(string value)
    {
        var characters = value.Where(char.IsLetterOrDigit).ToArray();
        return new string(characters).ToLowerInvariant();
    }

    public async Task<IReadOnlyList<Movie>> GetMoviesAsync(
        string? title = null,
        string? genre = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = dbContext.Movies.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(title))
            {
                var normalizedTitle = title.Trim();
                query = query.Where(movie => movie.MovieTitle.Contains(normalizedTitle));
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                var normalizedGenre = genre.Trim();
                query = query.Where(movie => movie.Genre.Contains(normalizedGenre));
            }

            return await query
                .OrderBy(movie => movie.MovieTitle)
                .ToListAsync(cancellationToken);
        }
        catch (SqlException exception)
        {
            logger.LogError(exception, "Unable to load movies from SQL Server.");
            throw new MovieDataAccessException(
                "MovieVerse couldn't load the movie catalog. Check that SQL Server is running and the Movies table is available.",
                exception);
        }
    }

    public async Task<Movie?> GetMovieByIdAsync(int movieId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await dbContext.Movies
                .AsNoTracking()
                .SingleOrDefaultAsync(movie => movie.MovieID == movieId, cancellationToken);
        }
        catch (SqlException exception)
        {
            logger.LogError(exception, "Unable to load movie {MovieId} from SQL Server.", movieId);
            throw new MovieDataAccessException(
                "MovieVerse couldn't load this movie. Check that SQL Server is running and the Movies table is available.",
                exception);
        }
    }

    public async Task<IReadOnlyList<string>> GetGenresAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var genreValues = await dbContext.Movies
                .AsNoTracking()
                .Select(movie => movie.Genre)
                .Distinct()
                .ToListAsync(cancellationToken);

            return genreValues
                .SelectMany(value => value.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(value => value)
                .ToArray();
        }
        catch (SqlException exception)
        {
            logger.LogError(exception, "Unable to load movie genres from SQL Server.");
            throw new MovieDataAccessException(
                "MovieVerse couldn't load the movie genres. Check that SQL Server is running and the Movies table is available.",
                exception);
        }
    }
}

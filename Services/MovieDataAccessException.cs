namespace movieweb.Services;

public sealed class MovieDataAccessException(string message, Exception innerException)
    : Exception(message, innerException)
{
}

namespace Resultron;

/// <summary>
/// Represents a detailed error reason used within the Result pattern, 
/// supporting error codes, descriptions, exception chaining, and custom metadata.
/// </summary>
/// <param name="Code">A unique machine-readable identifier code for the error.</param>
/// <param name="Description">A human-readable description explaining the error.</param>
/// <param name="Exception">The underlying exception that triggered this error, if applicable.</param>
public record Error(string Code, string Description, Exception? Exception = null) : IReason
{
    /// <summary>
    /// Represents a default, empty error state (indicates the absence of an error).
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// Gets the human-readable message associated with this error (maps to <see cref="Description"/>).
    /// </summary>
    string IReason.Message => Description;

    /// <summary>
    /// Gets or initializes additional contextual metadata key-value pairs for the error.
    /// </summary>
    public Dictionary<string, object> Metadata { get; init; } =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Creates a new <see cref="Error"/> instance chained with the specified underlying exception.
    /// </summary>
    /// <param name="exception">The exception that caused this error.</param>
    /// <returns>A new <see cref="Error"/> instance with the exception attached via record with-expression.</returns>
    public Error CausedBy(Exception exception) => this with { Exception = exception };
}
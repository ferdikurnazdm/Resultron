namespace Resultron;

/// <summary>
/// Represents a detailed error used within the Result pattern,
/// supporting error codes, descriptions, underlying exceptions, and custom metadata.
/// </summary>
/// <param name="Code">A unique machine-readable identifier for the error.</param>
/// <param name="Description">A human-readable description of the error.</param>
/// <param name="Exception">The underlying exception associated with the error, if any.</param>
public record Error(string Code, string Description, Exception? Exception = null)
{
    /// <summary>
    /// Represents the absence of an error.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// Gets additional contextual metadata associated with the error.
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata { get; init; } =
        new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the specified metadata entry added or updated.
    /// </summary>
    /// <param name="key">The metadata key.</param>
    /// <param name="value">The metadata value.</param>
    /// <returns>A new <see cref="Error"/> instance containing the updated metadata.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="key"/> is null or empty.
    /// </exception>
    public Error WithMetadata(string key, object value)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);

        var newMetadata = new Dictionary<string, object>(
            Metadata.Count,
            StringComparer.OrdinalIgnoreCase);

        foreach (var item in Metadata)
        {
            newMetadata[item.Key] = item.Value;
        }

        newMetadata[key] = value;

        return this with { Metadata = newMetadata };
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> instance associated with the specified exception.
    /// </summary>
    /// <param name="exception">The exception associated with this error.</param>
    /// <returns>A new <see cref="Error"/> instance containing the specified exception.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="exception"/> is null.
    /// </exception>
    public Error CausedBy(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return this with { Exception = exception };
    }
}
namespace Resultron;

/// <summary>
/// Represents a successful outcome reason used within the Result pattern, 
/// supporting success messages and custom metadata.
/// </summary>
/// <param name="Message">A human-readable message describing the success.</param>
public record Success(string Message) : IReason
{
    /// <summary>
    /// Represents a default, empty success state (indicates the absence of a specific message).
    /// </summary>
    public static readonly Success None = new(string.Empty);

    /// <summary>
    /// Gets or initializes additional contextual metadata key-value pairs for the success reason.
    /// </summary>
    public Dictionary<string, object> Metadata { get; init; } =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Creates a new <see cref="Success"/> instance containing an additional or updated metadata key-value pair.
    /// </summary>
    /// <param name="key">The metadata key.</param>
    /// <param name="value">The metadata value.</param>
    /// <returns>A new <see cref="Success"/> instance with the updated metadata.</returns>
    public Success WithMetadata(string key, object value)
    {
        var newMetadata = new Dictionary<string, object>(
            dictionary: Metadata,
            comparer: StringComparer.OrdinalIgnoreCase)
        {
            [key] = value
        };
        return this with { Metadata = newMetadata };
    }
}

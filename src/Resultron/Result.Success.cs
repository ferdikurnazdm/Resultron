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
    public IReadOnlyDictionary<string, object> Metadata { get; init; } =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);


    /// <summary>
    /// Creates a new <see cref="Success"/> instance containing an additional or updated metadata key-value pair.
    /// </summary>
    /// <param name="key">The metadata key.</param>
    /// <param name="value">The metadata value.</param>
    /// <returns>A new <see cref="Success"/> instance with the updated metadata.</returns>
    public Success WithMetadata(string key, object value)
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
}

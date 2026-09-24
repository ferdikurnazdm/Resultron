namespace Resultron;

/// <summary>
/// Defines the contract for result reasons (such as errors or successes),
/// providing a human-readable message and associated contextual metadata.
/// </summary>
public interface IReason
{
    /// <summary>
    /// Gets the human-readable message describing the reason.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Gets the contextual metadata key-value pairs associated with this reason.
    /// </summary>
    IReadOnlyDictionary<string, object> Metadata { get; }
}

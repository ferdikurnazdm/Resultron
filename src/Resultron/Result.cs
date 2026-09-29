namespace Resultron;

/// <summary>
/// Represents the outcome of an operation that does not return a value,
/// providing success or failure state information through <see cref="BaseResult"/>.
/// </summary>
public sealed partial class Result : BaseResult
{
    private Result(bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        
    }

    /// <summary>
    /// Creates a successful <see cref="Result"/>.
    /// </summary>
    /// <returns>A successful <see cref="Result"/>.</returns>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// Creates a failed <see cref="Result"/> containing the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A failed <see cref="Result"/> containing the specified error.</returns>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// Converts an <see cref="Error"/> into a failed <see cref="Result"/>.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>A failed <see cref="Result"/> containing the specified error.</returns>
    public static implicit operator Result(Error error) => Failure(error);
}
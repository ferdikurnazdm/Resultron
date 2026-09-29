namespace Resultron;

/// <summary>
/// Defines the core contract for all result types in the Resultron library,
/// exposing the operation status and the associated error.
/// </summary>
public interface IBaseResult
{
    /// <summary>
    /// Gets a value indicating whether the operation represented by this result was successful.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation represented by this result failed.
    /// </summary>
    bool IsFailure { get; }

    /// <summary>
    /// Gets the error associated with this result.
    /// Returns <see cref="Error.None"/> when the result is successful.
    /// </summary>
    Error Error { get; }
}



/// <summary>
/// Provides the base implementation for all result types in the Resultron library,
/// managing the success state and associated error.
/// </summary>
public abstract class BaseResult : IBaseResult
{
    /// <summary>
    /// Gets the error associated with this result.
    /// Returns <see cref="Error.None"/> when the result is successful.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Gets a value indicating whether the result represents a successful outcome.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result represents a failed outcome.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResult"/> class
    /// with the specified success state and error.
    /// </summary>
    /// <param name="isSuccess">
    /// A value indicating whether the result represents a successful outcome.
    /// </param>
    /// <param name="error">
    /// The error associated with the result.
    /// Use <see cref="Error.None"/> for successful results.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when a successful result contains an error or a failed result
    /// does not contain an error.
    /// </exception>
    protected BaseResult(bool isSuccess, Error error)
    {
        if (IsInvalidResultState(isSuccess, error))
            throw new ArgumentException(
                "A successful result cannot contain an error, and a failed result must contain an error.",
                nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }

    private static bool IsInvalidResultState(bool isSuccess, Error error)
    {
        return (isSuccess && error != Error.None) ||
               (!isSuccess && error == Error.None);
    }
}

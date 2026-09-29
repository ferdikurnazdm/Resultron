using System.Diagnostics.CodeAnalysis;

namespace Resultron;

/// <summary>
/// Represents the outcome of an operation that returns a value of type
/// <typeparamref name="T"/>, providing success or failure state information
/// and type-safe access to the associated value.
/// </summary>
/// <typeparam name="T">The type of value returned by the operation.</typeparam>
public sealed partial class Result<T> : BaseResult
{
    /// <summary>
    /// Gets the value associated with this result.
    /// The value may be <see langword="null"/> or the default value of
    /// <typeparamref name="T"/> when the result represents a failure.
    /// </summary>
    [AllowNull]
    public T Value { get; }

    private Result(bool isSuccess, Error error, T? value)
        : base(isSuccess, error)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a successful <see cref="Result{T}"/> containing the specified value.
    /// </summary>
    /// <param name="value">The value associated with the successful result.</param>
    /// <returns>
    /// A successful <see cref="Result{T}"/> containing the specified value.
    /// </returns>
    public static Result<T> Success(T value) => new(
        isSuccess: true,
        error: Error.None,
        value: value);

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> containing the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>
    /// A failed <see cref="Result{T}"/> containing the specified error.
    /// </returns>
    public static Result<T> Failure(Error error) => new(
        isSuccess: false,
        error: error,
        value: default);

    /// <summary>
    /// Converts a value of type <typeparamref name="T"/> into a successful
    /// <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>
    /// A successful <see cref="Result{T}"/> containing the specified value.
    /// </returns>
    public static implicit operator Result<T>(T value) => Success(value);

    /// <summary>
    /// Converts an <see cref="Error"/> into a failed <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>
    /// A failed <see cref="Result{T}"/> containing the specified error.
    /// </returns>
    public static implicit operator Result<T>(Error error) => Failure(error);
}
using System.Diagnostics.CodeAnalysis;

namespace Resultron;

/// <summary>
/// Represents the outcome of an operation that returns a value of type <typeparamref name="T"/>, 
/// inheriting from <see cref="BaseResult"/> to provide success/failure states, reasons, and type-safe data access.
/// </summary>
/// <typeparam name="T">The type of the value returned by the operation.</typeparam>
public sealed partial class Result<T> : BaseResult
{
    /// <summary>
    /// Gets the underlying value associated with this result.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the result is in a failed state (<see cref="BaseResult.IsFailure"/> is true).</exception>
    [AllowNull]
    public T Value => IsSuccess
        ? field!
        : throw new InvalidOperationException("Cannot access value of a failed result.");

    private Result(bool isSuccess, T? value, IEnumerable<IReason>? reasons = null)
        : base(isSuccess, reasons) => Value = value;

    /// <summary>
    /// Creates a new successful <see cref="Result{T}"/> instance containing the specified value and no specific reasons.
    /// </summary>
    /// <param name="value">The success value to wrap.</param>
    /// <returns>A successful <see cref="Result{T}"/> with the given value.</returns>
    public static Result<T> Success(T value) => new(true, value);

    /// <summary>
    /// Creates a new successful <see cref="Result{T}"/> instance containing the specified value and success reason.
    /// </summary>
    /// <param name="value">The success value to wrap.</param>
    /// <param name="success">The success reason (<see cref="Resultron.Success"/>) to attach.</param>
    /// <returns>A successful <see cref="Result{T}"/> with the given value and reason.</returns>
    public static Result<T> Success(T value, Success success) => new(true, value, [success]);

    /// <summary>
    /// Creates a new successful <see cref="Result{T}"/> instance containing the specified value and a collection of reasons.
    /// </summary>
    /// <param name="value">The success value to wrap.</param>
    /// <param name="reasons">A collection of reasons (<see cref="IReason"/>) to attach.</param>
    /// <returns>A successful <see cref="Result{T}"/> with the given value and reasons.</returns>
    public static Result<T> Success(T value, IEnumerable<IReason> reasons) => new(true, value, reasons);

    /// <summary>
    /// Creates a new failed <see cref="Result{T}"/> instance containing the specified error.
    /// </summary>
    /// <param name="error">The error (<see cref="Error"/>) that caused the failure.</param>
    /// <returns>A failed <see cref="Result{T}"/> containing the error.</returns>
    public static Result<T> Failure(Error error) => new(false, default, [error]);

    /// <summary>
    /// Creates a new failed <see cref="Result{T}"/> instance containing a collection of errors.
    /// </summary>
    /// <param name="errors">A collection of errors (<see cref="Error"/>) that caused the failure.</param>
    /// <returns>A failed <see cref="Result{T}"/> containing the errors.</returns>
    public static Result<T> Failure(IEnumerable<Error> errors) => new(false, default, errors);

    /// <summary>
    /// Creates a new failed <see cref="Result{T}"/> instance containing a collection of failure reasons.
    /// </summary>
    /// <param name="reasons">A collection of reasons (<see cref="IReason"/>) associated with the failure.</param>
    /// <returns>A failed <see cref="Result{T}"/> containing the reasons.</returns>
    public static Result<T> Failure(IEnumerable<IReason> reasons) => new(false, default, reasons);


    public static implicit operator Result<T>(T value)
        => Success(value);

    public static implicit operator Result<T>(Error error)
        => Failure(error);
}
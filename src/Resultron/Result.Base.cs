using System.Diagnostics.Contracts;

namespace Resultron;

/// <summary>
/// Defines the core contract for all result types within the Resultron library,
/// providing status flags and collections of success or failure reasons.
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
    /// Gets a read-only list of all reasons (<see cref="IReason"/>), including both successes and errors, associated with this result.
    /// </summary>
    IReadOnlyList<IReason> Reasons { get; }

    /// <summary>
    /// Gets a read-only list of all error reasons (<see cref="Error"/>) associated with this result.
    /// </summary>
    IReadOnlyList<Error> Errors { get; }

    /// <summary>
    /// Gets a read-only list of all success reasons (<see cref="Success"/>) associated with this result.
    /// </summary>
    IReadOnlyList<Success> Successes { get; }
}



/// <summary>
/// Serves as the foundational abstract class for all result types in the Resultron library,
/// managing success states, reason collections, and conditional side effects.
/// </summary>
public abstract class BaseResult : IBaseResult
{
    private readonly List<IReason> _reasons = [];

    /// <summary>
    /// Gets a read-only list of all reasons (<see cref="IReason"/>), including both successes and errors, associated with this result.
    /// </summary>
    public IReadOnlyList<IReason> Reasons => _reasons.AsReadOnly();

    /// <summary>
    /// Gets a read-only list of all error reasons (<see cref="Error"/>) associated with this result.
    /// </summary>
    public IReadOnlyList<Error> Errors => _reasons.OfType<Error>().ToList().AsReadOnly();

    /// <summary>
    /// Gets a read-only list of all success reasons (<see cref="Success"/>) associated with this result.
    /// </summary>
    public IReadOnlyList<Success> Successes => _reasons.OfType<Success>().ToList().AsReadOnly();

    /// <summary>
    /// Gets the primary error associated with this result. Returns <see cref="Error.None"/> if no errors exist.
    /// </summary>
    public Error Error => Errors.FirstOrDefault() ?? Error.None;

    /// <summary>
    /// Gets a value indicating whether the result represents a successful outcome.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result represents a failed outcome (the inverse of <see cref="IsSuccess"/>).
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResult"/> class with the specified success status and optional reasons.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the result is successful.</param>
    /// <param name="reasons">An optional collection of initial reasons (<see cref="IReason"/>).</param>
    protected BaseResult(bool isSuccess, IEnumerable<IReason>? reasons = null)
    {
        IsSuccess = isSuccess;

        if (reasons is not null)
        {
            _reasons.AddRange(reasons);
        }
    }

    /// <summary>
    /// Executes the specified action if the result is currently in a successful state, without breaking the fluent chain.
    /// </summary>
    /// <param name="action">The action to execute on success.</param>
    /// <returns>The current <see cref="BaseResult"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is <c>null</c>.</exception>
    public BaseResult SuccessIf(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (IsSuccess)
        {
            action();
        }

        return this;
    }

    /// <summary>
    /// Executes the specified action with the primary error if the result is currently in a failed state, without breaking the fluent chain.
    /// </summary>
    /// <param name="action">The action to execute on failure, receiving the primary <see cref="Error"/>.</param>
    /// <returns>The current <see cref="BaseResult"/> instance for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is <c>null</c>.</exception>
    public BaseResult FailureIf(Action<Error> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (IsFailure)
        {
            action(Error);
        }

        return this;
    }
}

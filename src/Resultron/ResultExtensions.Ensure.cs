namespace Resultron;

public static partial class ResultExtensions
{
    /// <summary>
    /// Ensures that a successful <see cref="Result"/> satisfies the specified
    /// predicate condition.
    /// If the predicate fails, returns a failure result with the given error.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="predicate">The condition to evaluate.</param>
    /// <param name="error">
    /// The error to return if the predicate evaluates to <c>false</c>.
    /// </param>
    /// <returns>
    /// The original result if successful and the predicate passes;
    /// otherwise, a failed result with the specified error.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="predicate"/>,
    /// or <paramref name="error"/> is <c>null</c>.
    /// </exception>
    public static Result Ensure(
        this Result result,
        Func<bool> predicate,
        Error error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        if (result.IsFailure)
        {
            return result;
        }

        return predicate()
            ? result
            : Result.Failure(error);
    }

    /// <summary>
    /// Asynchronously evaluates the specified predicate against a successful
    /// <see cref="Result"/>.
    /// If the predicate fails, returns a failure result with the given error.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="predicate">The asynchronous condition to evaluate.</param>
    /// <param name="error">
    /// The error to return if the predicate evaluates to <c>false</c>.
    /// </param>
    /// <returns>A task containing the validated result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="predicate"/>,
    /// or <paramref name="error"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> EnsureAsync(
        this Result result,
        Func<Task<bool>> predicate,
        Error error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        if (result.IsFailure)
        {
            return result;
        }

        var isValid = await predicate()
            .ConfigureAwait(false);

        return isValid
            ? result
            : Result.Failure(error);
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result"/> and ensures
    /// it satisfies the specified synchronous predicate condition.
    /// </summary>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="predicate">The condition to evaluate.</param>
    /// <param name="error">
    /// The error to return if the predicate evaluates to <c>false</c>.
    /// </param>
    /// <returns>A task containing the validated result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/>, <paramref name="predicate"/>,
    /// or <paramref name="error"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> EnsureAsync(
        this Task<Result> resultTask,
        Func<bool> predicate,
        Error error)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.Ensure(
            predicate,
            error);
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result"/> and ensures
    /// it satisfies the specified asynchronous predicate condition.
    /// </summary>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="predicate">The asynchronous condition to evaluate.</param>
    /// <param name="error">
    /// The error to return if the predicate evaluates to <c>false</c>.
    /// </param>
    /// <returns>A task containing the validated result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/>, <paramref name="predicate"/>,
    /// or <paramref name="error"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> EnsureAsync(
        this Task<Result> resultTask,
        Func<Task<bool>> predicate,
        Error error)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        var result = await resultTask
            .ConfigureAwait(false);

        return await result
            .EnsureAsync(predicate, error)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Ensures that the value of a successful <see cref="Result{T}"/>
    /// satisfies the specified predicate.
    /// If the predicate fails, returns a failure result with the given error.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="predicate">
    /// The condition to evaluate against the underlying value.
    /// </param>
    /// <param name="error">
    /// The error to return if the predicate evaluates to <c>false</c>.
    /// </param>
    /// <returns>
    /// The original result if successful and the predicate passes;
    /// otherwise, a failed result with the specified error.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="predicate"/>,
    /// or <paramref name="error"/> is <c>null</c>.
    /// </exception>
    public static Result<T> Ensure<T>(
        this Result<T> result,
        Func<T, bool> predicate,
        Error error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        if (result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value)
            ? result
            : Result<T>.Failure(error);
    }

    /// <summary>
    /// Ensures that the value of a successful <see cref="Result{T}"/>
    /// satisfies the specified asynchronous predicate.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="predicate">
    /// The asynchronous condition to evaluate against the underlying value.
    /// </param>
    /// <param name="error">
    /// The error to return if the predicate evaluates to <c>false</c>.
    /// </param>
    /// <returns>A task containing the validated result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="predicate"/>,
    /// or <paramref name="error"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<T>> EnsureAsync<T>(
        this Result<T> result,
        Func<T, Task<bool>> predicate,
        Error error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        if (result.IsFailure)
        {
            return result;
        }

        var isValid = await predicate(result.Value)
            .ConfigureAwait(false);

        return isValid
            ? result
            : Result<T>.Failure(error);
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result{T}"/> and
    /// ensures its value satisfies the specified synchronous predicate.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="predicate">
    /// The condition to evaluate against the underlying value.
    /// </param>
    /// <param name="error">
    /// The error to return if the predicate evaluates to <c>false</c>.
    /// </param>
    /// <returns>A task containing the validated result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/>, <paramref name="predicate"/>,
    /// or <paramref name="error"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<T>> EnsureAsync<T>(
        this Task<Result<T>> resultTask,
        Func<T, bool> predicate,
        Error error)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.Ensure(
            predicate,
            error);
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result{T}"/> and
    /// ensures its value satisfies the specified asynchronous predicate.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="predicate">
    /// The asynchronous condition to evaluate against the underlying value.
    /// </param>
    /// <param name="error">
    /// The error to return if the predicate evaluates to <c>false</c>.
    /// </param>
    /// <returns>A task containing the validated result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/>, <paramref name="predicate"/>,
    /// or <paramref name="error"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<T>> EnsureAsync<T>(
        this Task<Result<T>> resultTask,
        Func<T, Task<bool>> predicate,
        Error error)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        var result = await resultTask
            .ConfigureAwait(false);

        return await result
            .EnsureAsync(predicate, error)
            .ConfigureAwait(false);
    }
}
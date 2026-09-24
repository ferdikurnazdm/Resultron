namespace Resultron;

public static partial class ResultExtensions
{
    /// <summary>
    /// Combines a sequence of result instances into a single <see cref="Result"/>.
    /// If any of the results have failed, all accumulated errors are collected
    /// and returned in a single failure result.
    /// </summary>
    /// <param name="results">A sequence of results to combine.</param>
    /// <returns>
    /// A successful <see cref="Result"/> if all inputs succeed;
    /// otherwise, a failed <see cref="Result"/> containing all errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="results"/> is <c>null</c>.
    /// </exception>
    public static Result Combine(
        this IEnumerable<BaseResult> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        var resultsList = results.ToList();

        var failures = resultsList
            .Where(result => result.IsFailure)
            .SelectMany(result => result.Errors)
            .ToList();

        return failures.Count == 0
            ? Result.Success()
            : Result.Failure(failures);
    }

    /// <summary>
    /// Combines an array of result instances into a single <see cref="Result"/>.
    /// If any of the results have failed, all accumulated errors are collected
    /// and returned in a single failure result.
    /// </summary>
    /// <param name="results">An array of results to combine.</param>
    /// <returns>
    /// A successful <see cref="Result"/> if all inputs succeed;
    /// otherwise, a failed <see cref="Result"/> containing all errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="results"/> is <c>null</c>.
    /// </exception>
    public static Result Combine(
        this BaseResult[] results)
    {
        ArgumentNullException.ThrowIfNull(results);

        return ((IEnumerable<BaseResult>)results)
            .Combine();
    }

    /// <summary>
    /// Asynchronously awaits a sequence of task-wrapped results and combines
    /// them into a single <see cref="Result"/>.
    /// </summary>
    /// <param name="resultTasks">
    /// A sequence of result tasks to combine.
    /// </param>
    /// <returns>
    /// A task containing the combined <see cref="Result"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTasks"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> CombineAsync(
        this IEnumerable<Task<BaseResult>> resultTasks)
    {
        ArgumentNullException.ThrowIfNull(resultTasks);

        var results = await Task
            .WhenAll(resultTasks)
            .ConfigureAwait(false);

        return results.Combine();
    }

    /// <summary>
    /// Asynchronously awaits an array of task-wrapped results and combines
    /// them into a single <see cref="Result"/>.
    /// </summary>
    /// <param name="resultTasks">
    /// An array of result tasks to combine.
    /// </param>
    /// <returns>
    /// A task containing the combined <see cref="Result"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTasks"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> CombineAsync(
        this Task<BaseResult>[] resultTasks)
    {
        ArgumentNullException.ThrowIfNull(resultTasks);

        var results = await Task
            .WhenAll(resultTasks)
            .ConfigureAwait(false);

        return results.Combine();
    }

    /// <summary>
    /// Combines a sequence of typed result instances into a single
    /// <see cref="Result{T}"/> containing a read-only list of values.
    /// If any of the results have failed, all accumulated errors are
    /// collected and returned instead.
    /// </summary>
    /// <typeparam name="T">The value type of the source results.</typeparam>
    /// <param name="results">A sequence of typed results to combine.</param>
    /// <returns>
    /// A successful result containing all values if all inputs succeed;
    /// otherwise, a failed result containing all errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="results"/> is <c>null</c>.
    /// </exception>
    public static Result<IReadOnlyList<T>> Combine<T>(
        this IEnumerable<Result<T>> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        var resultsList = results.ToList();

        var failures = resultsList
            .Where(result => result.IsFailure)
            .SelectMany(result => result.Errors)
            .ToList();

        if (failures.Count > 0)
        {
            return Result<IReadOnlyList<T>>
                .Failure(failures);
        }

        var values = resultsList
            .Select(result => result.Value)
            .ToList();

        return Result<IReadOnlyList<T>>
            .Success(values);
    }

    /// <summary>
    /// Combines an array of typed result instances into a single
    /// result containing a read-only list of values.
    /// If any of the results have failed, all accumulated errors are
    /// collected and returned instead.
    /// </summary>
    /// <typeparam name="T">The value type of the source results.</typeparam>
    /// <param name="results">An array of typed results to combine.</param>
    /// <returns>
    /// A successful result containing all values if all inputs succeed;
    /// otherwise, a failed result containing all errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="results"/> is <c>null</c>.
    /// </exception>
    public static Result<IReadOnlyList<T>> Combine<T>(
        this Result<T>[] results)
    {
        ArgumentNullException.ThrowIfNull(results);

        return ((IEnumerable<Result<T>>)results)
            .Combine();
    }

    /// <summary>
    /// Asynchronously awaits a sequence of task-wrapped typed results and
    /// combines them into a single result containing a read-only list of values.
    /// </summary>
    /// <typeparam name="T">The value type of the source results.</typeparam>
    /// <param name="resultTasks">
    /// A sequence of typed result tasks to combine.
    /// </param>
    /// <returns>
    /// A task containing the combined result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTasks"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<IReadOnlyList<T>>> CombineAsync<T>(
        this IEnumerable<Task<Result<T>>> resultTasks)
    {
        ArgumentNullException.ThrowIfNull(resultTasks);

        var results = await Task
            .WhenAll(resultTasks)
            .ConfigureAwait(false);

        return results.Combine();
    }

    /// <summary>
    /// Asynchronously awaits an array of task-wrapped typed results and
    /// combines them into a single result containing a read-only list of values.
    /// </summary>
    /// <typeparam name="T">The value type of the source results.</typeparam>
    /// <param name="resultTasks">
    /// An array of typed result tasks to combine.
    /// </param>
    /// <returns>
    /// A task containing the combined result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTasks"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<IReadOnlyList<T>>> CombineAsync<T>(
        this Task<Result<T>>[] resultTasks)
    {
        ArgumentNullException.ThrowIfNull(resultTasks);

        var results = await Task
            .WhenAll(resultTasks)
            .ConfigureAwait(false);

        return results.Combine();
    }
}
namespace Resultron;

public static partial class ResultExtensions
{
    /// <summary>
    /// Converts a non-generic <see cref="Result"/> into a generic
    /// <see cref="Result{Unit}"/> containing <see cref="Unit.Value"/>.
    /// Preserves existing errors if the source result has failed.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <returns>
    /// A successful <see cref="Result{Unit}"/> if the source result is successful;
    /// otherwise, a failed result containing the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> is <c>null</c>.
    /// </exception>
    public static Result<Unit> ToUnitResult(
        this Result result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.IsSuccess
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(result.Errors);
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped non-generic
    /// <see cref="Result"/> and converts it into a generic
    /// <see cref="Result{Unit}"/>.
    /// </summary>
    /// <param name="resultTask">
    /// The task representing the source result.
    /// </param>
    /// <returns>
    /// A task containing the resulting <see cref="Result{Unit}"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<Unit>> ToUnitResultAsync(
        this Task<Result> resultTask)
    {
        ArgumentNullException.ThrowIfNull(resultTask);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.ToUnitResult();
    }

    /// <summary>
    /// Converts a typed <see cref="Result{T}"/> into a generic
    /// <see cref="Result{Unit}"/> containing <see cref="Unit.Value"/>.
    /// Preserves existing errors if the source result has failed.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <returns>
    /// A successful <see cref="Result{Unit}"/> if the source result is successful;
    /// otherwise, a failed result containing the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> is <c>null</c>.
    /// </exception>
    public static Result<Unit> ToUnitResult<T>(
        this Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.IsSuccess
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(result.Errors);
    }

    /// <summary>
    /// Converts a typed <see cref="Result{T}"/> into a non-generic
    /// <see cref="Result"/>.
    /// Preserves existing errors if the source result has failed.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <returns>
    /// A successful <see cref="Result"/> if the source result is successful;
    /// otherwise, a failed result containing the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> is <c>null</c>.
    /// </exception>
    public static Result ToNonGenericResult<T>(
        this Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.IsSuccess
            ? Result.Success()
            : Result.Failure(result.Errors);
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result{T}"/>
    /// and converts it into a generic <see cref="Result{Unit}"/>.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="resultTask">
    /// The task representing the source result.
    /// </param>
    /// <returns>
    /// A task containing the resulting <see cref="Result{Unit}"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<Unit>> ToUnitResultAsync<T>(
        this Task<Result<T>> resultTask)
    {
        ArgumentNullException.ThrowIfNull(resultTask);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.ToUnitResult();
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result{T}"/>
    /// and converts it into a non-generic <see cref="Result"/>.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="resultTask">
    /// The task representing the source result.
    /// </param>
    /// <returns>
    /// A task containing the resulting non-generic <see cref="Result"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> ToNonGenericResultAsync<T>(
        this Task<Result<T>> resultTask)
    {
        ArgumentNullException.ThrowIfNull(resultTask);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.ToNonGenericResult();
    }
}
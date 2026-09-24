namespace Resultron;

public static partial class ResultExtensions
{
    /// <summary>
    /// Asynchronously binds a successful <see cref="Result"/> to a new
    /// <see cref="Result"/> using the specified asynchronous binder function.
    /// If the source result has failed, the binder is not executed and the
    /// original failure is returned.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="binder">
    /// The asynchronous binder function to execute if the result is successful.
    /// </param>
    /// <returns>
    /// A task containing the result returned by the binder if successful;
    /// otherwise, the original failed <see cref="Result"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="binder"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> BindAsync(
        this Result result,
        Func<Task<Result>> binder)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(binder);

        if (result.IsFailure)
        {
            return result;
        }

        return await binder()
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Binds the value of a successful <see cref="Result{T}"/> to a
    /// non-generic <see cref="Result"/> using the specified binder function.
    /// Returns the prior failure if the source result has failed.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="binder">
    /// The binder function to apply to the underlying value if successful.
    /// </param>
    /// <returns>
    /// The result returned by the binder if successful;
    /// otherwise, a failed result containing the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="binder"/> is <c>null</c>.
    /// </exception>
    public static Result Bind<T>(
        this Result<T> result,
        Func<T, Result> binder)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(binder);

        if (result.IsFailure)
        {
            return Result.Failure(result.Errors);
        }

        return binder(result.Value);
    }

    /// <summary>
    /// Binds a successful <see cref="Result"/> to a new
    /// <see cref="Result"/> using the specified binder function.
    /// Returns the prior failure if the source result has failed.
    /// </summary>
    public static Result Bind(
        this Result result,
        Func<Result> binder)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(binder);

        if (result.IsFailure)
        {
            return result;
        }

        return binder();
    }

    /// <summary>
    /// Binds a successful <see cref="Result"/> to a new
    /// <see cref="Result{TOut}"/>.
    /// Returns the prior failure if the source result has failed.
    /// </summary>
    public static Result<TOut> Bind<TOut>(
        this Result result,
        Func<Result<TOut>> binder)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(binder);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        return binder();
    }

    /// <summary>
    /// Binds the value of a successful <see cref="Result{T}"/>
    /// to a new <see cref="Result{TOut}"/>.
    /// Returns the prior failure if the source result has failed.
    /// </summary>
    public static Result<TOut> Bind<T, TOut>(
        this Result<T> result,
        Func<T, Result<TOut>> binder)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(binder);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        return binder(result.Value);
    }

    /// <summary>
    /// Asynchronously binds the value of a successful
    /// <see cref="Result{T}"/> to a new
    /// <see cref="Result{TOut}"/>.
    /// </summary>
    public static async Task<Result<TOut>> BindAsync<T, TOut>(
        this Result<T> result,
        Func<T, Task<Result<TOut>>> binder)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(binder);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        return await binder(result.Value)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously waits for the source result and binds a
    /// successful result to another <see cref="Result"/>.
    /// </summary>
    public static async Task<Result> BindAsync(
        this Task<Result> resultTask,
        Func<Result> binder)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(binder);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.Bind(binder);
    }

    /// <summary>
    /// Asynchronously waits for the source result and invokes
    /// the asynchronous binder when successful.
    /// </summary>
    public static async Task<Result> BindAsync(
        this Task<Result> resultTask,
        Func<Task<Result>> binder)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(binder);

        var result = await resultTask
            .ConfigureAwait(false);

        if (result.IsFailure)
        {
            return result;
        }

        return await binder()
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously waits for a non-generic result and binds
    /// a successful result to <see cref="Result{TOut}"/>.
    /// </summary>
    public static async Task<Result<TOut>> BindAsync<TOut>(
        this Task<Result> resultTask,
        Func<Result<TOut>> binder)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(binder);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.Bind(binder);
    }

    /// <summary>
    /// Asynchronously waits for a non-generic result and invokes
    /// an asynchronous binder returning <see cref="Result{TOut}"/>.
    /// </summary>
    public static async Task<Result<TOut>> BindAsync<TOut>(
        this Task<Result> resultTask,
        Func<Task<Result<TOut>>> binder)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(binder);

        var result = await resultTask
            .ConfigureAwait(false);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        return await binder()
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously waits for <see cref="Result{T}"/> and
    /// applies the binder to its value when successful.
    /// </summary>
    public static async Task<Result<TOut>> BindAsync<T, TOut>(
        this Task<Result<T>> resultTask,
        Func<T, Result<TOut>> binder)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(binder);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.Bind(binder);
    }

    /// <summary>
    /// Asynchronously waits for <see cref="Result{T}"/> and
    /// applies an asynchronous binder to its value when successful.
    /// </summary>
    public static async Task<Result<TOut>> BindAsync<T, TOut>(
        this Task<Result<T>> resultTask,
        Func<T, Task<Result<TOut>>> binder)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(binder);

        var result = await resultTask
            .ConfigureAwait(false);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        return await binder(result.Value)
            .ConfigureAwait(false);
    }
}
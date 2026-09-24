namespace Resultron;

public static partial class ResultExtensions
{
    /// <summary>
    /// Asynchronously maps a successful <see cref="Result"/> to a new value
    /// of type <typeparamref name="TOut"/> using the specified asynchronous
    /// mapper function.
    /// If the source result has failed, the mapper is not executed and the
    /// prior errors are propagated.
    /// </summary>
    /// <typeparam name="TOut">The output value type after mapping.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="mapper">
    /// The asynchronous mapping function to execute if the result is successful.
    /// </param>
    /// <returns>
    /// A task containing a successful <see cref="Result{TOut}"/> with the mapped
    /// value, or a failed result containing the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="mapper"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<TOut>> MapAsync<TOut>(
        this Result result,
        Func<Task<TOut>> mapper)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(mapper);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        var mappedValue = await mapper()
            .ConfigureAwait(false);

        return Result<TOut>.Success(mappedValue);
    }

    /// <summary>
    /// Asynchronously executes the specified action with the underlying value
    /// of a successful <see cref="Result{T}"/> and returns the original result.
    /// If the source result has failed, the action is not executed and the
    /// original failure is returned.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="action">
    /// The asynchronous action to execute with the underlying value if successful.
    /// </param>
    /// <returns>
    /// A task containing the original <see cref="Result{T}"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<T>> MapAsync<T>(
        this Result<T> result,
        Func<T, Task> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (result.IsFailure)
        {
            return result;
        }

        await action(result.Value)
            .ConfigureAwait(false);

        return result;
    }

    /// <summary>
    /// Asynchronously maps the value of a successful <see cref="Result{T}"/>
    /// to a new value of type <typeparamref name="TOut"/> using the specified
    /// asynchronous mapper function.
    /// Returns the prior failure if the source result has failed.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <typeparam name="TOut">The output value type after mapping.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="mapper">
    /// The asynchronous mapping function to apply to the underlying value.
    /// </param>
    /// <returns>
    /// A task containing a successful <see cref="Result{TOut}"/> with the mapped
    /// value, or a failed result containing the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="mapper"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(
        this Result<T> result,
        Func<T, Task<TOut>> mapper)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(mapper);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        var mappedValue = await mapper(result.Value)
            .ConfigureAwait(false);

        return Result<TOut>.Success(mappedValue);
    }

    /// <summary>
    /// Maps a successful <see cref="Result"/> to a new value of type
    /// <typeparamref name="TOut"/> using the specified mapper function.
    /// Returns the prior failure if the source result has failed.
    /// </summary>
    /// <typeparam name="TOut">The output value type after mapping.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="mapper">The mapping function to invoke if successful.</param>
    /// <returns>
    /// A successful <see cref="Result{TOut}"/> containing the mapped value,
    /// or a failed result containing the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="mapper"/> is <c>null</c>.
    /// </exception>
    public static Result<TOut> Map<TOut>(
        this Result result,
        Func<TOut> mapper)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(mapper);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        return Result<TOut>.Success(mapper());
    }

    /// <summary>
    /// Maps the value of a successful <see cref="Result{T}"/> to a new
    /// value of type <typeparamref name="TOut"/>.
    /// Returns the prior failure if the source result has failed.
    /// </summary>
    /// <typeparam name="T">The source value type.</typeparam>
    /// <typeparam name="TOut">The output value type after mapping.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="mapper">The mapping function to apply if successful.</param>
    /// <returns>
    /// A successful <see cref="Result{TOut}"/> containing the mapped value,
    /// or a failed result containing the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="mapper"/> is <c>null</c>.
    /// </exception>
    public static Result<TOut> Map<T, TOut>(
        this Result<T> result,
        Func<T, TOut> mapper)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(mapper);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        return Result<TOut>.Success(
            mapper(result.Value));
    }

    /// <summary>
    /// Asynchronously waits for the source <see cref="Result"/> and maps
    /// a successful result to a value of type <typeparamref name="TOut"/>.
    /// </summary>
    /// <typeparam name="TOut">The output value type after mapping.</typeparam>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="mapper">The mapping function to invoke if successful.</param>
    /// <returns>
    /// A task containing the resulting <see cref="Result{TOut}"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/> or <paramref name="mapper"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<TOut>> MapAsync<TOut>(
        this Task<Result> resultTask,
        Func<TOut> mapper)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(mapper);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.Map(mapper);
    }

    /// <summary>
    /// Asynchronously waits for the source <see cref="Result"/> and maps
    /// a successful result using an asynchronous mapper function.
    /// </summary>
    /// <typeparam name="TOut">The output value type after mapping.</typeparam>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="mapper">
    /// The asynchronous mapping function to invoke if successful.
    /// </param>
    /// <returns>
    /// A task containing the resulting <see cref="Result{TOut}"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/> or <paramref name="mapper"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<TOut>> MapAsync<TOut>(
        this Task<Result> resultTask,
        Func<Task<TOut>> mapper)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(mapper);

        var result = await resultTask
            .ConfigureAwait(false);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        var mappedValue = await mapper()
            .ConfigureAwait(false);

        return Result<TOut>.Success(mappedValue);
    }

    /// <summary>
    /// Asynchronously waits for the source <see cref="Result{T}"/> and maps
    /// its value to a new value of type <typeparamref name="TOut"/>.
    /// </summary>
    /// <typeparam name="T">The source value type.</typeparam>
    /// <typeparam name="TOut">The output value type after mapping.</typeparam>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="mapper">The mapping function to apply if successful.</param>
    /// <returns>
    /// A task containing the resulting <see cref="Result{TOut}"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/> or <paramref name="mapper"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(
        this Task<Result<T>> resultTask,
        Func<T, TOut> mapper)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(mapper);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.Map(mapper);
    }

    /// <summary>
    /// Asynchronously waits for the source <see cref="Result{T}"/> and maps
    /// its underlying value using an asynchronous mapper function.
    /// </summary>
    /// <typeparam name="T">The source value type.</typeparam>
    /// <typeparam name="TOut">The output value type after mapping.</typeparam>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="mapper">
    /// The asynchronous function to apply to the underlying value if successful.
    /// </param>
    /// <returns>
    /// A task containing a successful <see cref="Result{TOut}"/> with the mapped
    /// value, or a failed result containing the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/> or <paramref name="mapper"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<TOut>> MapAsync<T, TOut>(
        this Task<Result<T>> resultTask,
        Func<T, Task<TOut>> mapper)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(mapper);

        var result = await resultTask
            .ConfigureAwait(false);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        var mappedValue = await mapper(result.Value)
            .ConfigureAwait(false);

        return Result<TOut>.Success(mappedValue);
    }
}
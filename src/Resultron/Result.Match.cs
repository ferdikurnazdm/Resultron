namespace Resultron;

public static partial class ResultExtensions
{
    /// <summary>
    /// Matches the outcome of a <see cref="Result"/>,
    /// executing the success action if successful,
    /// or the failure action with the primary error if failed.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">
    /// The action to execute if the result is successful.
    /// </param>
    /// <param name="onFailure">
    /// The action to execute if the result has failed,
    /// receiving the primary <see cref="Error"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="onSuccess"/>,
    /// or <paramref name="onFailure"/> is <c>null</c>.
    /// </exception>
    public static void Match(
        this Result result,
        Action onSuccess,
        Action<Error> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        if (result.IsSuccess)
        {
            onSuccess();
            return;
        }

        onFailure(result.Error);
    }

    /// <summary>
    /// Asynchronously matches the outcome of a <see cref="Result"/>,
    /// executing the asynchronous success action if successful,
    /// or the asynchronous failure action with the primary error if failed.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">
    /// The asynchronous action to execute if the result is successful.
    /// </param>
    /// <param name="onFailure">
    /// The asynchronous action to execute if the result has failed,
    /// receiving the primary <see cref="Error"/>.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="onSuccess"/>,
    /// or <paramref name="onFailure"/> is <c>null</c>.
    /// </exception>
    public static async Task MatchAsync(
        this Result result,
        Func<Task> onSuccess,
        Func<Error, Task> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        if (result.IsSuccess)
        {
            await onSuccess()
                .ConfigureAwait(false);

            return;
        }

        await onFailure(result.Error)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously matches the outcome of a <see cref="Result"/>,
    /// executing either the asynchronous success function or the asynchronous
    /// failure function with the primary error, and returning the resulting value.
    /// </summary>
    /// <typeparam name="TOut">The output return type of the match handlers.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">
    /// The asynchronous function to execute if the result is successful.
    /// </param>
    /// <param name="onFailure">
    /// The asynchronous function to execute if the result has failed,
    /// receiving the primary <see cref="Error"/>.
    /// </param>
    /// <returns>
    /// A task containing the value returned by either the success or failure handler.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="onSuccess"/>,
    /// or <paramref name="onFailure"/> is <c>null</c>.
    /// </exception>
    public static async Task<TOut> MatchAsync<TOut>(
        this Result result,
        Func<Task<TOut>> onSuccess,
        Func<Error, Task<TOut>> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        if (result.IsSuccess)
        {
            return await onSuccess()
                .ConfigureAwait(false);
        }

        return await onFailure(result.Error)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Matches the outcome of a <see cref="Result{T}"/>,
    /// executing the success action with the underlying value if successful,
    /// or the failure action with the primary error if failed.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">
    /// The action to execute if the result is successful,
    /// receiving the underlying value.
    /// </param>
    /// <param name="onFailure">
    /// The action to execute if the result has failed,
    /// receiving the primary <see cref="Error"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="onSuccess"/>,
    /// or <paramref name="onFailure"/> is <c>null</c>.
    /// </exception>
    public static void Match<T>(
        this Result<T> result,
        Action<T> onSuccess,
        Action<Error> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        if (result.IsSuccess)
        {
            onSuccess(result.Value);
            return;
        }

        onFailure(result.Error);
    }

    /// <summary>
    /// Asynchronously matches the outcome of a <see cref="Result{T}"/>,
    /// executing the success action with the underlying value if successful,
    /// or the failure action with the primary error if failed.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">
    /// The asynchronous action to execute if the result is successful,
    /// receiving the underlying value.
    /// </param>
    /// <param name="onFailure">
    /// The asynchronous action to execute if the result has failed,
    /// receiving the primary <see cref="Error"/>.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="onSuccess"/>,
    /// or <paramref name="onFailure"/> is <c>null</c>.
    /// </exception>
    public static async Task MatchAsync<T>(
        this Result<T> result,
        Func<T, Task> onSuccess,
        Func<Error, Task> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        if (result.IsSuccess)
        {
            await onSuccess(result.Value)
                .ConfigureAwait(false);

            return;
        }

        await onFailure(result.Error)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Matches the outcome of a <see cref="Result"/>, executing either
    /// the success function or the failure function with the primary error,
    /// and returning the resulting value of type <typeparamref name="TOut"/>.
    /// </summary>
    /// <typeparam name="TOut">The output return type of the match handlers.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailure">
    /// The function to execute if the result has failed, receiving the primary <see cref="Error"/>.
    /// </param>
    /// <returns>The result of evaluating either the success or failure function.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="onSuccess"/>,
    /// or <paramref name="onFailure"/> is <c>null</c>.
    /// </exception>
    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<Error, TOut> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return result.IsSuccess
            ? onSuccess()
            : onFailure(result.Error);
    }

    /// <summary>
    /// Matches the outcome of a <see cref="Result{T}"/>, executing either
    /// the success function with the value or the failure function with the
    /// primary error, and returning the resulting value of type
    /// <typeparamref name="TOut"/>.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <typeparam name="TOut">The output return type of the match handlers.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">
    /// The function to execute if the result is successful, receiving the underlying value.
    /// </param>
    /// <param name="onFailure">
    /// The function to execute if the result has failed, receiving the primary <see cref="Error"/>.
    /// </param>
    /// <returns>The result of evaluating either the success or failure function.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="onSuccess"/>,
    /// or <paramref name="onFailure"/> is <c>null</c>.
    /// </exception>
    public static TOut Match<T, TOut>(
        this Result<T> result,
        Func<T, TOut> onSuccess,
        Func<Error, TOut> onFailure)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return result.IsSuccess
            ? onSuccess(result.Value)
            : onFailure(result.Error);
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result"/> and matches
    /// its outcome using the corresponding synchronous handler function.
    /// </summary>
    /// <typeparam name="TOut">The output return type of the match handlers.</typeparam>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="onSuccess">The function to execute if successful.</param>
    /// <param name="onFailure">The function to execute if failed.</param>
    /// <returns>
    /// A task containing the evaluated outcome of type <typeparamref name="TOut"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/>, <paramref name="onSuccess"/>,
    /// or <paramref name="onFailure"/> is <c>null</c>.
    /// </exception>
    public static async Task<TOut> MatchAsync<TOut>(
        this Task<Result> resultTask,
        Func<TOut> onSuccess,
        Func<Error, TOut> onFailure)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.Match(
            onSuccess,
            onFailure);
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result"/> and matches
    /// its outcome using asynchronous handler functions.
    /// </summary>
    /// <typeparam name="TOut">The output return type of the match handlers.</typeparam>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="onSuccess">The asynchronous function to execute if successful.</param>
    /// <param name="onFailure">The asynchronous function to execute if failed.</param>
    /// <returns>
    /// A task containing the evaluated outcome of type <typeparamref name="TOut"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/>, <paramref name="onSuccess"/>,
    /// or <paramref name="onFailure"/> is <c>null</c>.
    /// </exception>
    public static async Task<TOut> MatchAsync<TOut>(
        this Task<Result> resultTask,
        Func<Task<TOut>> onSuccess,
        Func<Error, Task<TOut>> onFailure)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        var result = await resultTask
            .ConfigureAwait(false);

        if (result.IsSuccess)
        {
            return await onSuccess()
                .ConfigureAwait(false);
        }

        return await onFailure(result.Error)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result{T}"/> and
    /// matches its outcome using the corresponding synchronous handler function.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <typeparam name="TOut">The output return type of the match handlers.</typeparam>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="onSuccess">
    /// The function to execute if successful, receiving the underlying value.
    /// </param>
    /// <param name="onFailure">
    /// The function to execute if failed, receiving the primary error.
    /// </param>
    /// <returns>
    /// A task containing the evaluated outcome of type <typeparamref name="TOut"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/>, <paramref name="onSuccess"/>,
    /// or <paramref name="onFailure"/> is <c>null</c>.
    /// </exception>
    public static async Task<TOut> MatchAsync<T, TOut>(
        this Task<Result<T>> resultTask,
        Func<T, TOut> onSuccess,
        Func<Error, TOut> onFailure)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.Match(
            onSuccess,
            onFailure);
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result{T}"/> and
    /// matches its outcome using asynchronous handler functions.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <typeparam name="TOut">The output return type of the match handlers.</typeparam>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="onSuccess">
    /// The asynchronous function to execute if successful, receiving the underlying value.
    /// </param>
    /// <param name="onFailure">
    /// The asynchronous function to execute if failed, receiving the primary error.
    /// </param>
    /// <returns>
    /// A task containing the evaluated outcome of type <typeparamref name="TOut"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/>, <paramref name="onSuccess"/>,
    /// or <paramref name="onFailure"/> is <c>null</c>.
    /// </exception>
    public static async Task<TOut> MatchAsync<T, TOut>(
        this Task<Result<T>> resultTask,
        Func<T, Task<TOut>> onSuccess,
        Func<Error, Task<TOut>> onFailure)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        var result = await resultTask
            .ConfigureAwait(false);

        if (result.IsSuccess)
        {
            return await onSuccess(result.Value)
                .ConfigureAwait(false);
        }

        return await onFailure(result.Error)
            .ConfigureAwait(false);
    }
}
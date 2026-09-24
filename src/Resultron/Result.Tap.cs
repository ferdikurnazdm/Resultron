namespace Resultron;

public static partial class ResultExtensions
{
    /// <summary>
    /// Executes the specified action if the <see cref="Result"/> is successful,
    /// returning the original result unchanged for fluent chaining.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The action to execute if successful.</param>
    /// <returns>The original <see cref="Result"/> instance.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static Result Tap(
        this Result result,
        Action action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (result.IsSuccess)
        {
            action();
        }

        return result;
    }

    /// <summary>
    /// Executes the specified asynchronous action if the
    /// <see cref="Result"/> is successful, returning the original
    /// result unchanged.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The asynchronous action to execute if successful.</param>
    /// <returns>
    /// A task containing the original <see cref="Result"/> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> TapAsync(
        this Result result,
        Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (result.IsSuccess)
        {
            await action()
                .ConfigureAwait(false);
        }

        return result;
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result"/> and
    /// executes the specified action if successful, returning the
    /// original result unchanged.
    /// </summary>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="action">The action to execute if successful.</param>
    /// <returns>
    /// A task containing the original <see cref="Result"/> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> TapAsync(
        this Task<Result> resultTask,
        Action action)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(action);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.Tap(action);
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result"/> and
    /// executes the specified asynchronous action if successful,
    /// returning the original result unchanged.
    /// </summary>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="action">The asynchronous action to execute if successful.</param>
    /// <returns>
    /// A task containing the original <see cref="Result"/> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> TapAsync(
        this Task<Result> resultTask,
        Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(action);

        var result = await resultTask
            .ConfigureAwait(false);

        return await result
            .TapAsync(action)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the specified action with the underlying value if the
    /// <see cref="Result{T}"/> is successful, returning the original
    /// result unchanged for fluent chaining.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="action">
    /// The action to execute if successful, receiving the underlying value.
    /// </param>
    /// <returns>The original <see cref="Result{T}"/> instance.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static Result<T> Tap<T>(
        this Result<T> result,
        Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }

    /// <summary>
    /// Executes the specified asynchronous action with the underlying
    /// value if the <see cref="Result{T}"/> is successful, returning
    /// the original result unchanged.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="action">
    /// The asynchronous action to execute if successful,
    /// receiving the underlying value.
    /// </param>
    /// <returns>
    /// A task containing the original <see cref="Result{T}"/> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<T>> TapAsync<T>(
        this Result<T> result,
        Func<T, Task> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (result.IsSuccess)
        {
            await action(result.Value)
                .ConfigureAwait(false);
        }

        return result;
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result{T}"/> and
    /// executes the specified action if successful, returning the
    /// original result unchanged.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="action">
    /// The action to execute if successful, receiving the underlying value.
    /// </param>
    /// <returns>
    /// A task containing the original <see cref="Result{T}"/> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<T>> TapAsync<T>(
        this Task<Result<T>> resultTask,
        Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(action);

        var result = await resultTask
            .ConfigureAwait(false);

        return result.Tap(action);
    }

    /// <summary>
    /// Asynchronously awaits a task-wrapped <see cref="Result{T}"/> and
    /// executes the specified asynchronous action if successful,
    /// returning the original result unchanged.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <param name="resultTask">The task representing the source result.</param>
    /// <param name="action">
    /// The asynchronous action to execute if successful,
    /// receiving the underlying value.
    /// </param>
    /// <returns>
    /// A task containing the original <see cref="Result{T}"/> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="resultTask"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<T>> TapAsync<T>(
        this Task<Result<T>> resultTask,
        Func<T, Task> action)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(action);

        var result = await resultTask
            .ConfigureAwait(false);

        return await result
            .TapAsync(action)
            .ConfigureAwait(false);
    }
}
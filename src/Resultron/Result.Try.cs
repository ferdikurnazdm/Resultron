namespace Resultron;

public sealed partial class Result
{
    /// <summary>
    /// Executes the specified action and captures any thrown exceptions, converting them into a failed <see cref="Result"/> using the provided error handler.
    /// </summary>
    /// <param name="action">The action to execute safely.</param>
    /// <param name="errorHandler">A function to map a caught <see cref="Exception"/> to an <see cref="Error"/>.</param>
    /// <returns>A successful <see cref="Result"/> if execution completes without exceptions; otherwise, a failed result containing the mapped error.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> or <paramref name="errorHandler"/> is <c>null</c>.</exception>
    public static Result Try(Action action, Func<Exception, Error> errorHandler)
    {
        ArgumentNullException.ThrowIfNull(action);

        ArgumentNullException.ThrowIfNull(errorHandler);

        try
        {
            action();

            return Success();
        }
        catch (Exception ex)
        {
            var error = errorHandler(ex)
                .CausedBy(ex);

            return Failure(error);
        }
    }

    /// <summary>
    /// Executes the specified action and captures any thrown exceptions,
    /// converting them into a failed <see cref="Result"/> containing
    /// the exception type, message, and original exception.
    /// </summary>
    /// <param name="action">
    /// The action to execute safely.
    /// </param>
    /// <returns>
    /// A successful <see cref="Result"/> if execution completes without exceptions;
    /// otherwise, a failed result containing the exception details.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static Result Try(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        try
        {
            action();

            return Success();
        }
        catch (Exception ex)
        {
            var error = new Error(
                Code: ex.GetType().Name,
                Description: ex.Message,
                Exception: ex);

            return Failure(error);
        }
    }

    /// <summary>
    /// Asynchronously executes the specified task-producing action and captures any thrown exceptions, converting them into a failed <see cref="Result"/> using the provided error handler.
    /// </summary>
    /// <param name="action">The asynchronous action task factory to execute safely.</param>
    /// <param name="errorHandler">A function to map a caught <see cref="Exception"/> to an <see cref="Error"/>.</param>
    /// <returns>A task containing a successful <see cref="Result"/> if execution completes without exceptions; otherwise, a failed result containing the mapped error.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> or <paramref name="errorHandler"/> is <c>null</c>.</exception>
    public static async Task<Result> TryAsync(Func<Task> action, Func<Exception, Error> errorHandler)
    {
        ArgumentNullException.ThrowIfNull(action);

        ArgumentNullException.ThrowIfNull(errorHandler);

        try
        {
            await action()
                .ConfigureAwait(false);

            return Success();
        }
        catch (Exception ex)
        {
            var error = errorHandler(ex)
                .CausedBy(ex);

            return Failure(error);
        }
    }

    /// <summary>
    /// Asynchronously executes the specified action and captures any thrown
    /// exceptions, converting them into a failed <see cref="Result"/>
    /// containing the exception type, message, and original exception.
    /// </summary>
    /// <param name="action">
    /// The asynchronous action to execute safely.
    /// </param>
    /// <returns>
    /// A task containing a successful <see cref="Result"/> if execution
    /// completes without exceptions; otherwise, a failed result containing
    /// the exception details.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> TryAsync(Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        try
        {
            await action()
                .ConfigureAwait(false);

            return Success();
        }
        catch (Exception ex)
        {
            var error = new Error(
                Code: ex.GetType().Name,
                Description: ex.Message,
                Exception: ex);

            return Failure(error);
        }
    }
}

public sealed partial class Result<T>
{
    /// <summary>
    /// Executes the specified function and captures any thrown exceptions, converting them into a failed <see cref="Result{T}"/> using the provided error handler.
    /// </summary>
    /// <param name="func">The function to execute safely.</param>
    /// <param name="errorHandler">A function to map a caught <see cref="Exception"/> to an <see cref="Error"/>.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the produced value if execution completes without exceptions; otherwise, a failed result containing the mapped error.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="func"/> or <paramref name="errorHandler"/> is <c>null</c>.</exception>
    public static Result<T> Try(Func<T> func, Func<Exception, Error> errorHandler)
    {
        ArgumentNullException.ThrowIfNull(func);

        ArgumentNullException.ThrowIfNull(errorHandler);

        try
        {
            var value = func();

            return Success(value);
        }
        catch (Exception ex)
        {
            var error = errorHandler(ex)
                .CausedBy(ex);

            return Failure(error);
        }
    }

    /// <summary>
    /// Executes the specified function and captures any thrown exceptions,
    /// converting them into a failed <see cref="Result{T}"/> containing
    /// the exception type, message, and original exception.
    /// </summary>
    /// <param name="func">The function to execute safely.</param>
    /// <returns>
    /// A successful <see cref="Result{T}"/> containing the returned value
    /// if execution completes without exceptions; otherwise, a failed result
    /// containing the exception details.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="func"/> is <c>null</c>.
    /// </exception>
    public static Result<T> Try(Func<T> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        try
        {
            var value = func();

            return Success(value);
        }
        catch (Exception ex)
        {
            var error = new Error(
                Code: ex.GetType().Name,
                Description: ex.Message,
                Exception: ex);

            return Failure(error);
        }
    }

    /// <summary>
    /// Asynchronously executes the specified task-producing function and captures any thrown exceptions, converting them into a failed <see cref="Result{T}"/> using the provided error handler.
    /// </summary>
    /// <param name="func">The asynchronous function task factory to execute safely.</param>
    /// <param name="errorHandler">A function to map a caught <see cref="Exception"/> to an <see cref="Error"/>.</param>
    /// <returns>A task containing a successful <see cref="Result{T}"/> with the produced value if execution completes without exceptions; otherwise, a failed result containing the mapped error.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="func"/> or <paramref name="errorHandler"/> is <c>null</c>.</exception>
    public static async Task<Result<T>> TryAsync(Func<Task<T>> func, Func<Exception, Error> errorHandler)
    {
        ArgumentNullException.ThrowIfNull(func);

        ArgumentNullException.ThrowIfNull(errorHandler);

        try
        {
            var value = await func()
                .ConfigureAwait(false);

            return Success(value);
        }
        catch (Exception ex)
        {
            var error = errorHandler(ex)
                .CausedBy(ex);

            return Failure(error);
        }
    }

    /// <summary>
    /// Asynchronously executes the specified task-producing function and captures
    /// any thrown exceptions, converting them into a failed <see cref="Result{T}"/>
    /// containing the exception type, message, and original exception.
    /// </summary>
    /// <param name="func">
    /// The asynchronous function to execute safely.
    /// </param>
    /// <returns>
    /// A task containing a successful <see cref="Result{T}"/> with the produced
    /// value if execution completes without exceptions; otherwise, a failed result
    /// containing the exception details.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="func"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<T>> TryAsync(Func<Task<T>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        try
        {
            var value = await func()
                .ConfigureAwait(false);

            return Success(value);
        }
        catch (Exception ex)
        {
            var error = new Error(
                Code: ex.GetType().Name,
                Description: ex.Message,
                Exception: ex);

            return Failure(error);
        }
    }
}
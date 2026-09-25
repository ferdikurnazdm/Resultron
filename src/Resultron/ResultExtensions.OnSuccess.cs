using System;

namespace Resultron;

public static partial class ResultExtensions
{
    /// <summary>
    /// Executes the specified action if the <see cref="Result"/> is successful,
    /// while preserving the result instance for fluent chaining.
    /// </summary>
    /// <param name="result">The result to evaluate.</param>
    /// <param name="action">The action to execute when the result is successful.</param>
    /// <returns>
    /// The original <see cref="Result"/> instance, allowing further fluent operations.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static Result OnSuccess(
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
    /// Executes the specified action with the underlying value if the
    /// <see cref="Result{T}"/> is successful, while preserving the result
    /// instance for fluent chaining.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result to evaluate.</param>
    /// <param name="action">
    /// The action to execute when the result is successful, receiving the
    /// underlying value as its argument.
    /// </param>
    /// <returns>
    /// The original <see cref="Result{T}"/> instance, allowing further fluent operations.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static Result<T> OnSuccess<T>(
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
    /// Asynchronously executes the specified action if the
    /// <see cref="Result"/> is successful, while preserving the result
    /// instance for fluent chaining.
    /// </summary>
    /// <param name="result">The result to evaluate.</param>
    /// <param name="action">
    /// The asynchronous action to execute when the result is successful.
    /// </param>
    /// <returns>
    /// A task containing the original <see cref="Result"/> instance,
    /// allowing further fluent operations.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> OnSuccessAsync(
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
    /// Asynchronously executes the specified action with the underlying value
    /// if the <see cref="Result{T}"/> is successful, while preserving the
    /// result instance for fluent chaining.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result to evaluate.</param>
    /// <param name="action">
    /// The asynchronous action to execute when the result is successful,
    /// receiving the underlying value as its argument.
    /// </param>
    /// <returns>
    /// A task containing the original <see cref="Result{T}"/> instance,
    /// allowing further fluent operations.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<T>> OnSuccessAsync<T>(
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
}

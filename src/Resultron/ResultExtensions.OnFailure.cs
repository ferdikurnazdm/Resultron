using System;

namespace Resultron;

public static partial class ResultExtensions
{
    /// <summary>
    /// Asynchronously executes the specified action with the primary error if the
    /// <see cref="Result"/> has failed, while preserving the result instance
    /// for fluent chaining.
    /// </summary>
    /// <param name="result">The result to evaluate.</param>
    /// <param name="action">
    /// The asynchronous action to execute when the result has failed,
    /// receiving the primary <see cref="Error"/> as its argument.
    /// </param>
    /// <returns>
    /// A task containing the original <see cref="Result"/> instance,
    /// allowing further fluent operations.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result> OnFailureAsync(
        this Result result,
        Func<Error, Task> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (result.IsFailure)
        {
            await action(result.Error)
                .ConfigureAwait(false);
        }

        return result;
    }

    /// <summary>
    /// Asynchronously executes the specified action with the primary error if the
    /// <see cref="Result{T}"/> has failed, while preserving the result instance
    /// for fluent chaining.
    /// </summary>
    /// <typeparam name="T">The type of the value associated with the result.</typeparam>
    /// <param name="result">The result to evaluate.</param>
    /// <param name="action">
    /// The asynchronous action to execute when the result has failed,
    /// receiving the primary <see cref="Error"/> as its argument.
    /// </param>
    /// <returns>
    /// A task containing the original <see cref="Result{T}"/> instance,
    /// allowing further fluent operations.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static async Task<Result<T>> OnFailureAsync<T>(
        this Result<T> result,
        Func<Error, Task> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (result.IsFailure)
        {
            await action(result.Error)
                .ConfigureAwait(false);
        }

        return result;
    }

    /// <summary>
    /// Executes the specified action with the primary error if the
    /// <see cref="Result"/> has failed, while preserving the result
    /// instance for fluent chaining.
    /// </summary>
    /// <param name="result">The result to evaluate.</param>
    /// <param name="action">
    /// The action to execute when the result has failed, receiving the
    /// primary <see cref="Error"/> as its argument.
    /// </param>
    /// <returns>
    /// The original <see cref="Result"/> instance, allowing further fluent operations.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static Result OnFailure(
        this Result result,
        Action<Error> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (result.IsFailure)
        {
            action(result.Error);
        }

        return result;
    }

    /// <summary>
    /// Executes the specified action with the primary error if the
    /// <see cref="Result{T}"/> has failed, while preserving the result
    /// instance for fluent chaining.
    /// </summary>
    /// <typeparam name="T">The type of the value associated with the result.</typeparam>
    /// <param name="result">The result to evaluate.</param>
    /// <param name="action">
    /// The action to execute when the result has failed, receiving the
    /// primary <see cref="Error"/> as its argument.
    /// </param>
    /// <returns>
    /// The original <see cref="Result{T}"/> instance, allowing further fluent operations.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public static Result<T> OnFailure<T>(
        this Result<T> result,
        Action<Error> action)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(action);

        if (result.IsFailure)
        {
            action(result.Error);
        }

        return result;
    }
}

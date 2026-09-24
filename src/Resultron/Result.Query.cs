namespace Resultron;

public static partial class ResultExtensions
{
    /// <summary>
    /// Projects a successful <see cref="Result"/> into a new form using
    /// the specified selector function.
    /// Enables C# query comprehension syntax (<c>select</c>).
    /// </summary>
    /// <typeparam name="TOut">The projected output value type.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="selector">A transform function to invoke if successful.</param>
    /// <returns>
    /// A new successful <see cref="Result{TOut}"/> containing the projected value,
    /// or a failed result with the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="selector"/> is <c>null</c>.
    /// </exception>
    public static Result<TOut> Select<TOut>(
        this Result result,
        Func<TOut> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        return result.Map(selector);
    }

    /// <summary>
    /// Projects a successful <see cref="Result"/> to a
    /// <see cref="Result{TOut}"/> and flattens the resulting structure.
    /// Enables C# query comprehension syntax with multiple clauses.
    /// </summary>
    /// <typeparam name="TOut">The output value type of the inner result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="binder">
    /// A transform function to invoke if successful, returning a new result.
    /// </param>
    /// <returns>
    /// A flattened <see cref="Result{TOut}"/> resulting from the binder
    /// or prior failures.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="binder"/> is <c>null</c>.
    /// </exception>
    public static Result<TOut> SelectMany<TOut>(
        this Result result,
        Func<Result<TOut>> binder)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(binder);

        return result.Bind(binder);
    }

    /// <summary>
    /// Projects a successful <see cref="Result"/> through an intermediate
    /// result binder and projects the values into a final output type using
    /// the specified projector function.
    /// Enables C# query comprehension syntax with multiple <c>from</c> clauses.
    /// </summary>
    /// <typeparam name="TIntermediate">
    /// The value type of the intermediate result.
    /// </typeparam>
    /// <typeparam name="TOut">The final projected output type.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="binder">
    /// A transform function to obtain an intermediate result.
    /// </param>
    /// <param name="projector">
    /// A projection function to combine the unit value and intermediate value
    /// into the final output.
    /// </param>
    /// <returns>
    /// A successful <see cref="Result{TOut}"/> containing the projected
    /// combination, or a failed result with the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="binder"/>,
    /// or <paramref name="projector"/> is <c>null</c>.
    /// </exception>
    public static Result<TOut> SelectMany<TIntermediate, TOut>(
        this Result result,
        Func<Result<TIntermediate>> binder,
        Func<Unit, TIntermediate, TOut> projector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(binder);
        ArgumentNullException.ThrowIfNull(projector);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        var intermediate = binder();

        if (intermediate.IsFailure)
        {
            return Result<TOut>.Failure(intermediate.Errors);
        }

        return Result<TOut>.Success(
            projector(
                Unit.Value,
                intermediate.Value));
    }

    /// <summary>
    /// Filters a successful <see cref="Result"/> using the specified
    /// predicate condition.
    /// Enables C# query comprehension syntax (<c>where</c>).
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
    public static Result Where(
        this Result result,
        Func<bool> predicate,
        Error error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        return result.Ensure(
            predicate,
            error);
    }

    /// <summary>
    /// Projects the value of a successful <see cref="Result{T}"/> into
    /// a new form using the specified selector function.
    /// Enables C# query comprehension syntax (<c>select</c>).
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <typeparam name="TOut">The projected output value type.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="selector">
    /// A transform function to apply to the source value.
    /// </param>
    /// <returns>
    /// A new successful <see cref="Result{TOut}"/> containing the projected
    /// value, or a failed result with the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="selector"/> is <c>null</c>.
    /// </exception>
    public static Result<TOut> Select<T, TOut>(
        this Result<T> result,
        Func<T, TOut> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        return result.Map(selector);
    }

    /// <summary>
    /// Projects the value of a successful <see cref="Result{T}"/> to a
    /// <see cref="Result{TOut}"/> and flattens the resulting structure.
    /// Enables C# query comprehension syntax with multiple clauses.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <typeparam name="TOut">The output value type of the inner result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="binder">
    /// A transform function to apply to the source value, returning a new result.
    /// </param>
    /// <returns>
    /// A flattened <see cref="Result{TOut}"/> resulting from the binder
    /// or prior failures.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/> or <paramref name="binder"/> is <c>null</c>.
    /// </exception>
    public static Result<TOut> SelectMany<T, TOut>(
        this Result<T> result,
        Func<T, Result<TOut>> binder)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(binder);

        return result.Bind(binder);
    }

    /// <summary>
    /// Projects the value of a successful <see cref="Result{T}"/> through
    /// an intermediate result binder and projects both values into a final
    /// output type using the specified projector function.
    /// Enables C# query comprehension syntax with multiple <c>from</c> clauses.
    /// </summary>
    /// <typeparam name="T">The value type of the source result.</typeparam>
    /// <typeparam name="TIntermediate">
    /// The value type of the intermediate result.
    /// </typeparam>
    /// <typeparam name="TOut">The final projected output type.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="binder">
    /// A transform function to obtain an intermediate result from the source value.
    /// </param>
    /// <param name="projector">
    /// A projection function to combine the source value and intermediate
    /// value into the final output.
    /// </param>
    /// <returns>
    /// A successful <see cref="Result{TOut}"/> containing the projected
    /// combination, or a failed result with the prior errors.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="result"/>, <paramref name="binder"/>,
    /// or <paramref name="projector"/> is <c>null</c>.
    /// </exception>
    public static Result<TOut> SelectMany<T, TIntermediate, TOut>(
        this Result<T> result,
        Func<T, Result<TIntermediate>> binder,
        Func<T, TIntermediate, TOut> projector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(binder);
        ArgumentNullException.ThrowIfNull(projector);

        if (result.IsFailure)
        {
            return Result<TOut>.Failure(result.Errors);
        }

        var intermediateResult = binder(result.Value);

        if (intermediateResult.IsFailure)
        {
            return Result<TOut>.Failure(
                intermediateResult.Errors);
        }

        return Result<TOut>.Success(
            projector(
                result.Value,
                intermediateResult.Value));
    }

    /// <summary>
    /// Filters a successful <see cref="Result{T}"/> using the specified
    /// predicate condition.
    /// Enables C# query comprehension syntax (<c>where</c>).
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
    public static Result<T> Where<T>(
        this Result<T> result,
        Func<T, bool> predicate,
        Error error)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(error);

        return result.Ensure(
            predicate,
            error);
    }
}
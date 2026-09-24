using System.Diagnostics.Contracts;

namespace Resultron;

/// <summary>
/// Represents an optional value of type <typeparamref name="T"/>, encapsulating the presence or absence of a value 
/// in a type-safe and functional manner.
/// </summary>
/// <typeparam name="T">The underlying type of the value.</typeparam>
public readonly struct Maybe<T>
{
    private readonly T? _value;

    /// <summary>
    /// Gets a value indicating whether this instance contains a valid value.
    /// </summary>
    public bool HasValue { get; }

    /// <summary>
    /// Gets a value indicating whether this instance does not contain a value.
    /// </summary>
    public bool HasNoValue => !HasValue;

    /// <summary>
    /// Gets the underlying value if present.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the instance has no value.
    /// </exception>
    public T Value => HasValue
        ? _value!
        : throw new InvalidOperationException("Maybe has no value.");

    private Maybe(T? value, bool hasValue)
    {
        _value = value;
        HasValue = hasValue;
    }

    /// <summary>
    /// Creates a new <see cref="Maybe{T}"/> instance from the specified value.
    /// Returns a none instance if the value is <c>null</c>; otherwise, returns a some instance.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <returns>A <see cref="Maybe{T}"/> instance representing the value.</returns>
    public static Maybe<T> From(T? value) =>
        value is null
            ? None()
            : Some(value);

    /// <summary>
    /// Creates a new <see cref="Maybe{T}"/> instance representing the presence of the specified value.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <returns>A <see cref="Maybe{T}"/> instance containing the value.</returns>
    public static Maybe<T> Some(T value) =>
        new(value, true);

    /// <summary>
    /// Creates a new <see cref="Maybe{T}"/> instance representing the absence of a value.
    /// </summary>
    /// <returns>A <see cref="Maybe{T}"/> instance with no value.</returns>
    public static Maybe<T> None() =>
        new(default, false);

    /// <summary>
    /// Gets the underlying value if present; otherwise, returns the default value of <typeparamref name="T"/>.
    /// </summary>
    /// <returns>The contained value or the default value of <typeparamref name="T"/>.</returns>
    [Pure]
    public T? GetValueOrDefault() =>
        _value;

    /// <summary>
    /// Gets the underlying value if present; otherwise, returns the specified fallback value.
    /// </summary>
    /// <param name="defaultValue">The value to return when no value is present.</param>
    /// <returns>The contained value or <paramref name="defaultValue"/>.</returns>
    [Pure]
    public T GetValueOrDefault(T defaultValue) =>
        HasValue
            ? _value!
            : defaultValue;

    /// <summary>
    /// Gets the underlying value if present; otherwise, invokes the specified factory to create a fallback value.
    /// </summary>
    /// <param name="factory">The factory used to create the fallback value.</param>
    /// <returns>The contained value or the value returned by <paramref name="factory"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="factory"/> is <c>null</c>.
    /// </exception>
    [Pure]
    public T GetValueOrElse(Func<T> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        return HasValue
            ? _value!
            : factory();
    }

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="T"/> (or null) to a <see cref="Maybe{T}"/> instance.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Maybe<T>(T? value) =>
        From(value);

    /// <summary>
    /// Maps the internal value to a new type using the specified mapping function, if a value is present.
    /// </summary>
    /// <typeparam name="TResult">The result type of the mapping function.</typeparam>
    /// <param name="func">The mapping function to apply.</param>
    /// <returns>
    /// A new <see cref="Maybe{TResult}"/> containing the mapped value,
    /// or a none instance if no value was present.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="func"/> is <c>null</c>.
    /// </exception>
    [Pure]
    public Maybe<TResult> Map<TResult>(Func<T, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return HasValue
            ? Maybe<TResult>.From(func(_value!))
            : Maybe<TResult>.None();
    }

    /// <summary>
    /// Binds the internal value to another <see cref="Maybe{TResult}"/> using the specified function,
    /// if a value is present.
    /// </summary>
    /// <typeparam name="TResult">The result type of the binding function.</typeparam>
    /// <param name="func">The binding function to apply.</param>
    /// <returns>
    /// The result returned by <paramref name="func"/>,
    /// or a none instance if no value was present.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="func"/> is <c>null</c>.
    /// </exception>
    [Pure]
    public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return HasValue
            ? func(_value!)
            : Maybe<TResult>.None();
    }

    /// <summary>
    /// Executes the appropriate function depending on whether a value is present.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="some">The function to execute when a value is present.</param>
    /// <param name="none">The function to execute when no value is present.</param>
    /// <returns>The result produced by the selected function.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="some"/> or <paramref name="none"/> is <c>null</c>.
    /// </exception>
    [Pure]
    public TResult Match<TResult>(
        Func<T, TResult> some,
        Func<TResult> none)
    {
        ArgumentNullException.ThrowIfNull(some);
        ArgumentNullException.ThrowIfNull(none);

        return HasValue
            ? some(_value!)
            : none();
    }

    /// <summary>
    /// Executes the specified action if a value is present.
    /// </summary>
    /// <param name="action">The action to execute when a value is present.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public void IfSome(Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (HasValue)
        {
            action(_value!);
        }
    }

    /// <summary>
    /// Executes the specified action if no value is present.
    /// </summary>
    /// <param name="action">The action to execute when no value is present.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="action"/> is <c>null</c>.
    /// </exception>
    public void IfNone(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (HasNoValue)
        {
            action();
        }
    }
}

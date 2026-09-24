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
    /// Gets a value indicating whether this instance does not contain a value (the inverse of <see cref="HasValue"/>).
    /// </summary>
    public bool HasNoValue => !HasValue;

    /// <summary>
    /// Gets the underlying value if present.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the instance has no value (<see cref="HasNoValue"/> is true).</exception>
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
    public static Maybe<T> From(T? value) => value is null ? None() : Some(value);

    /// <summary>
    /// Creates a new <see cref="Maybe{T}"/> instance representing the presence of the specified value.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <returns>A <see cref="Maybe{T}"/> instance containing the value.</returns>
    public static Maybe<T> Some(T value) => new(value, true);

    /// <summary>
    /// Creates a new <see cref="Maybe{T}"/> instance representing the absence of a value.
    /// </summary>
    /// <returns>A <see cref="Maybe{T}"/> instance with no value.</returns>
    public static Maybe<T> None() => new(default, false);

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="T"/> (or null) to a <see cref="Maybe{T}"/> instance.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Maybe<T>(T? value) => From(value);

    /// <summary>
    /// Maps the internal value to a new type using the specified mapping function, if a value is present.
    /// </summary>
    /// <typeparam name="TResult">The result type of the mapping function.</typeparam>
    /// <param name="func">The mapping function to apply.</param>
    /// <returns>A new <see cref="Maybe{TResult}"/> containing the mapped value, or a none instance if no value was present.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="func"/> is <c>null</c>.</exception>
    [Pure]
    public Maybe<TResult> Map<TResult>(Func<T, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return HasValue
            ? Maybe<TResult>.From(func(_value!))
            : Maybe<TResult>.None();
    }
}

namespace Resultron;

/// <summary>
/// Represents a void type, since <see cref="void"/> cannot be used as a generic type parameter in C#.
/// Used primarily in functional result patterns (e.g., <see cref="Result{Unit}"/>) to indicate operations that succeed without returning a value.
/// </summary>
public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
    /// <summary>
    /// Represents the single allowed instance of the <see cref="Unit"/> struct.
    /// </summary>
    public static readonly Unit Value = new();

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code (always 0).</returns>
    public override int GetHashCode() => 0;

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Unit"/> instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified object is a <see cref="Unit"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) =>
        obj is Unit;

    /// <summary>
    /// Determines whether the specified <see cref="Unit"/> is equal to the current <see cref="Unit"/> instance.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>Always <c>true</c>.</returns>
    public bool Equals(Unit other) => true;

    /// <summary>
    /// Compares the current instance with another <see cref="Unit"/> instance.
    /// </summary>
    /// <param name="other">The <see cref="Unit"/> instance to compare with.</param>
    /// <returns>Always 0, as all <see cref="Unit"/> instances are equal.</returns>
    public int CompareTo(Unit other) => 0;

    /// <summary>
    /// Compares the current instance with a specified object.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>Always 0 when <paramref name="obj"/> is a <see cref="Unit"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="obj"/> is not a <see cref="Unit"/>.
    /// </exception>
    public int CompareTo(object? obj) =>
        obj is Unit
            ? 0
            : throw new ArgumentException(
                "Object must be of type Unit.",
                nameof(obj));

    /// <summary>
    /// Determines whether two <see cref="Unit"/> instances are equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>Always <c>true</c>.</returns>
    public static bool operator ==(Unit left, Unit right) => true;

    /// <summary>
    /// Determines whether two <see cref="Unit"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>Always <c>false</c>.</returns>
    public static bool operator !=(Unit left, Unit right) => false;

    /// <summary>
    /// Returns a string representation of the <see cref="Unit"/> instance.
    /// </summary>
    /// <returns>The string "()".</returns>
    public override string ToString() => "()";
}

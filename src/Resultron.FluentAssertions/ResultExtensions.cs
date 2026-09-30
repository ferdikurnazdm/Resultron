using System;

namespace Resultron.FluentAssertions;

/// <summary>
/// Provides assertions for <see cref="Result"/> objects.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Returns a <see cref="ResultAssertions"/> object that can be used to assert the current <see cref="Result"/>.
    /// </summary>
    /// <param name="instance">The result instance to be tested.</param>
    /// <returns>An instance of <see cref="ResultAssertions"/> to execute fluent assertions.</returns>
    public static ResultAssertions Should(this Result instance)
    {
        return new ResultAssertions(instance);
    }

    /// <summary>
    /// Returns a <see cref="ResultAssertions{T}"/> object that can be used to assert the current <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="instance">The generic result instance to be tested.</param>
    /// <returns>An instance of <see cref="ResultAssertions{T}"/> to execute fluent assertions.</returns>
    public static ResultAssertions<T> Should<T>(this Result<T> instance)
    {
        return new ResultAssertions<T>(instance);
    }
}

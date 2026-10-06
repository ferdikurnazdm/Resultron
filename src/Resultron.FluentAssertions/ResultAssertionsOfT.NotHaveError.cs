using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions<T>
{
    /// <summary>
    /// Asserts that the generic result does not contain an error.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> NotHaveError()
    {
        return NotHaveError(string.Empty, Array.Empty<object>());
    }

    /// <summary>
    /// Asserts that the generic result does not contain an error.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> NotHaveError(
        string because, 
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);

        if (Subject.IsSuccess)
        {
            return new AndConstraint<ResultAssertions<T>>(this);
        }

        Subject.Error.Should().BeNull(
            "Expected result not to have an error, but found error with code '{0}'.",
            Subject.Error?.Code ?? "Unknown");

        return new AndConstraint<ResultAssertions<T>>(this);
    }
}

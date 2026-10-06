using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions<T>
{
    /// <summary>
    /// Asserts that the generic result does not contain a value (typically when it's a failure).
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> NotHaveValue()
    {
        return NotHaveValue(string.Empty, Array.Empty<object>());
    }

    /// <summary>
    /// Asserts that the generic result does not contain a value (typically when it's a failure).
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> NotHaveValue(
        string because, 
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);

        Subject.IsSuccess.Should().BeFalse(
            "Expected result not to have a value, but it succeeded with value: {0}",
            Subject.Value);

        return new AndConstraint<ResultAssertions<T>>(this);
    }
}
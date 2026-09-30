using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions
{
    /// <summary>
    /// Asserts that the result is a failure.
    /// </summary>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format the because parameter.</param>
    /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain more assertions.</returns>
    [CustomAssertion]
    public AndConstraint<ResultAssertions> BeFailure(
        string because = "", 
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);

        Subject.IsSuccess.Should().BeFalse(
            "Expected result to be a failure, but it succeeded.",
            becauseArgs);

        return new AndConstraint<ResultAssertions>(this);
    }
}

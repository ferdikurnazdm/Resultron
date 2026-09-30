using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions
{
    /// <summary>
    /// Asserts that the result does not contain an error.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions> NotHaveError(
        string because = "", 
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);

        if (Subject.IsSuccess)
        {
            return new AndConstraint<ResultAssertions>(this);
        }

        Subject.Error.Should().BeNull(
            "Expected result not to have an error, but found error with code '{0}'.",
            Subject.Error?.Code ?? "Unknown");

        return new AndConstraint<ResultAssertions>(this);
    }
}

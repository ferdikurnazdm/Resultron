using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions
{
    /// <summary>
    /// Asserts that the result is successful.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions> BeSuccessful(
        string because = "", 
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);

        Subject.IsSuccess.Should().BeTrue(
            "Expected result to be successful, but it failed with error: {0}",
            Subject.Error?.Description ?? "Unknown error");

        return new AndConstraint<ResultAssertions>(this);
    }
}

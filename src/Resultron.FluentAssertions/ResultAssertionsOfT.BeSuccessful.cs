using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions<T>
{
    /// <summary>
    /// Asserts that the generic result is successful.
    /// </summary>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied. If the phrase is empty, the assertion will fail with a default message.</param>
    /// <param name="becauseArgs">Zero or more objects to format the <paramref name="because"/> parameter.</param>
    /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain more assertions.</returns>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> BeSuccessful(
        string because = "", 
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);

        Subject.IsSuccess.Should().BeTrue(
            "Expected result to be successful, but it failed with error: {0}",
            Subject.Error?.Description ?? "Unknown error");

        return new AndConstraint<ResultAssertions<T>>(this);
    }
}

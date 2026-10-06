using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions
{
    /// <summary>
    /// Asserts that the result is successful.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions> BeSuccessful()
    {
        return BeSuccessful(string.Empty, Array.Empty<object>());
    }

    /// <summary>
    /// Asserts that the result is successful.
    /// </summary>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format the <paramref name="because"/> parameter.</param>
    [CustomAssertion]
    public AndConstraint<ResultAssertions> BeSuccessful(
        string because, 
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);

        var failureDetails = $"result failed with error '{Subject.Error?.Description ?? "Unknown error"}'";
        
        var effectiveBecause = string.IsNullOrWhiteSpace(because)
            ? failureDetails
            : $"{because}; {failureDetails}";

        Subject.IsSuccess.Should().BeTrue(effectiveBecause, becauseArgs);

        return new AndConstraint<ResultAssertions>(this);
    }
}

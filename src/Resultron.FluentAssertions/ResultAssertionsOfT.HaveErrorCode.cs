using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions<T>
{
    /// <summary>
    /// Asserts that the generic result contains an error with the specified error code.
    /// </summary>
    /// <param name="expectedCode">The expected error code.</param>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveErrorCode(string expectedCode)
    {
        return HaveErrorCode(expectedCode, string.Empty, Array.Empty<object>());
    }

    /// <summary>
    /// Asserts that the generic result contains an error with the specified error code.
    /// </summary>
    /// <param name="expectedCode">The expected error code.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format the because parameter.</param>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveErrorCode(
        string expectedCode,
        string because, 
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);

        Subject.IsSuccess.Should().BeFalse(
            "Expected result to have error code '{0}', but it succeeded with value: {1}",
            expectedCode,
            Subject.Value);

        Subject.Error.Should().NotBeNull(
            "Expected result to have an error with code '{0}', but Error was null.",
            expectedCode);

        Subject.Error!.Code.Should().Be(
            expectedCode,
            because,
            becauseArgs);

        return new AndConstraint<ResultAssertions<T>>(this);
    }
}

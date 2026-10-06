using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions<T>
{
    /// <summary>
    /// Asserts that the generic result is a failure with the specified error.
    /// </summary>
    /// <param name="expectedError">The expected error record.</param>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> BeFailureWithError(Error expectedError)
    {
        return BeFailureWithError(expectedError, string.Empty, Array.Empty<object>());
    }

    /// <summary>
    /// Asserts that the generic result is a failure with the specified error.
    /// </summary>
    /// <param name="expectedError">The expected error record.</param>
    /// <param name="because">A formatted phrase explaining why the assertion should be satisfied.</param>
    /// <param name="becauseArgs">Zero or more objects to format the <paramref name="because"/> parameter.</param>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> BeFailureWithError(
        Error expectedError,
        string because,
        params object[] becauseArgs)
    {
        expectedError.Should().NotBeNull("an expected error is required");

        BeFailure(because, becauseArgs);

        Subject.Error.Should().NotBeNull(because, becauseArgs);
        
        Subject.Error.Should().Be(expectedError, because, becauseArgs);

        return new AndConstraint<ResultAssertions<T>>(this);
    }
}

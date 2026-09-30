using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions<T>
{
    /// <summary>
    /// Asserts that the generic result contains an error.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveError()
    {
        return HaveError(string.Empty, Array.Empty<object>());
    }

    /// <summary>
    /// Asserts that the generic result contains an error.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveError(
        string because, 
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);

        Subject.IsSuccess.Should().BeFalse(
            "Expected result to have an error because it is successful, but it succeeded with value: {0}",
            Subject.Value);

        Subject.Error.Should().NotBeNull(
            "Expected result to have an error object, but Error was null.", 
            becauseArgs);

        return new AndConstraint<ResultAssertions<T>>(this);
    }

    /// <summary>
    /// Asserts that the generic result contains an error with the specified description.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveError(
        string expectedErrorMessage)
    {
        return HaveError(expectedErrorMessage, string.Empty, Array.Empty<object>());
    }

    /// <summary>
    /// Asserts that the generic result contains an error with the specified description.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveError(
        string expectedErrorMessage,
        string because, 
        params object[] becauseArgs)
    {
        HaveError(because, becauseArgs);

        Subject.Error!.Description.Should().Be(
            expectedErrorMessage,
            because, 
            becauseArgs);

        return new AndConstraint<ResultAssertions<T>>(this);
    }
}
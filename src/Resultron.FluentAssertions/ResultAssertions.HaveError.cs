using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions
{
    /// <summary>
    /// Asserts that the result contains an error.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions> HaveError()
    {
        return HaveError(string.Empty, Array.Empty<object>());
    }

    /// <summary>
    /// Asserts that the result contains an error.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions> HaveError(
        string because, 
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);

        Subject.IsSuccess.Should().BeFalse(
            "Expected result to have an error because it is successful, but it succeeded.", 
            becauseArgs);

        Subject.Error.Should().NotBeNull(
            "Expected result to have an error object, but Error was null.", 
            becauseArgs);

        return new AndConstraint<ResultAssertions>(this);
    }

    /// <summary>
    /// Asserts that the result contains an error with the specified description or code.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions> HaveError(
        string expectedErrorMessage)
    {
        return HaveError(expectedErrorMessage, string.Empty, Array.Empty<object>());
    }

    /// <summary>
    /// Asserts that the result contains an error with the specified description or code.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions> HaveError(
        string expectedErrorMessage,
        string because, 
        params object[] becauseArgs)
    {
        HaveError(because, becauseArgs);

        Subject.Error!.Description.Should().Be(
            expectedErrorMessage,
            because, 
            becauseArgs);

        return new AndConstraint<ResultAssertions>(this);
    }
}
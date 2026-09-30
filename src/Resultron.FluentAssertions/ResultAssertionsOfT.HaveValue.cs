using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions<T>
{
    /// <summary>
    /// Asserts that the generic result is successful and contains a value.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveValue()
    {
        return HaveValue(string.Empty);
    }

    /// <summary>
    /// Asserts that the generic result is successful and contains a value.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveValue(
        string because,
        params object[] becauseArgs)
    {
        Subject.Should().NotBeNull(because, becauseArgs);

        Subject.IsSuccess.Should().BeTrue(
            "Expected result to have a value, but it failed with error: {0}",
            Subject.Error?.Description ?? "Unknown error");

        return new AndConstraint<ResultAssertions<T>>(this);
    }

    /// <summary>
    /// Asserts that the generic result is successful and contains the expected value.
    /// </summary>
    /// <param name="expectedValue">The expected value to be contained in the result.</param>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveValue(T expectedValue)
    {
        return HaveValue(expectedValue, string.Empty);
    }

    /// <summary>
    /// Asserts that the generic result is successful and contains the expected value.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveValue(
        T expectedValue,
        string because,
        params object[] becauseArgs)
    {
        HaveValue(because, becauseArgs);

        Subject.Value.Should().Be(
            expectedValue,
            because,
            becauseArgs);

        return new AndConstraint<ResultAssertions<T>>(this);
    }

    /// <summary>
    /// Asserts that the generic result is successful and its value satisfies the specified assertion action.
    /// </summary>
    /// <param name="valueAssertion">An assertion action to be performed on the contained value.</param>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveValue(Action<T> valueAssertion)
    {
        return HaveValue(valueAssertion, string.Empty);
    }

    /// <summary>
    /// Asserts that the generic result is successful and its value satisfies the specified assertion action.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> HaveValue(
        Action<T> valueAssertion,
        string because,
        params object[] becauseArgs)
    {
        ArgumentNullException.ThrowIfNull(valueAssertion);

        HaveValue(because, becauseArgs);

        valueAssertion(Subject.Value);

        return new AndConstraint<ResultAssertions<T>>(this);
    }
}
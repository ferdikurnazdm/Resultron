using System;
using FluentAssertions;

namespace Resultron.FluentAssertions;

public partial class ResultAssertions<T>
{
    /// <summary>
    /// Asserts that the result is successful and contains a value.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> BeSuccessfulWithValue()
    {
        return HaveValue();
    }

    /// <summary>
    /// Asserts that the result is successful and contains a value.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> BeSuccessfulWithValue(
        string because,
        params object[] becauseArgs)
    {
        return HaveValue(because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the result is successful and contains the expected value.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> BeSuccessfulWithValue(T expectedValue)
    {
        return HaveValue(expectedValue);
    }

    /// <summary>
    /// Asserts that the result is successful and contains the expected value.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> BeSuccessfulWithValue(
        T expectedValue,
        string because,
        params object[] becauseArgs)
    {
        return HaveValue(expectedValue, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the result is successful and its value satisfies the specified assertion action.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> BeSuccessfulWithValue(Action<T> valueAssertion)
    {
        return HaveValue(valueAssertion);
    }

    /// <summary>
    /// Asserts that the result is successful and its value satisfies the specified assertion action.
    /// </summary>
    [CustomAssertion]
    public AndConstraint<ResultAssertions<T>> BeSuccessfulWithValue(
        Action<T> valueAssertion,
        string because,
        params object[] becauseArgs)
    {
        return HaveValue(valueAssertion, because, becauseArgs);
    }
}


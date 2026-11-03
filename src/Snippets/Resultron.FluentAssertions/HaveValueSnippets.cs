using System;
using FluentAssertions;
using Resultron;
using Resultron.FluentAssertions;

namespace Snippets.Resultron.FluentAssertions;

public sealed class HaveValueSnippets
{
    // Generic Result<T>

    public void HaveValue()
    {
        // Arrange
        Result<int> result = Result<int>.Success(42);

        // Assert
        result.Should().HaveValue();
    }

    public void HaveValueWithReason()
    {
        // Arrange
        Result<int> result = Result<int>.Success(42);

        // Assert
        result.Should().HaveValue(
            because: "the operation should return a valid value");
    }

    public void HaveValueWithExpectedValue()
    {
        // Arrange
        Result<int> result = Result<int>.Success(42);

        // Assert
        result.Should().HaveValue(
            expectedValue: 42);
    }

    public void HaveValueWithExpectedValueAndReason()
    {
        // Arrange
        int userId = 1;

        Result<int> result = Result<int>.Success(userId);

        // Assert
        result.Should().HaveValue(
            expectedValue: userId,
            because: "user ID {0} should be returned successfully",
            becauseArgs: userId);
    }

    public void HaveValueWithValueAssertion()
    {
        // Arrange
        Result<string> result = Result<string>.Success(
            "Operation completed.");

        // Assert
        result.Should().HaveValue(
            valueAssertion: value =>
                value.Should().StartWith("Operation"));
    }

    public void HaveValueWithValueAssertionAndReason()
    {
        // Arrange
        int minimumValue = 10;

        Result<int> result = Result<int>.Success(42);

        // Assert
        result.Should().HaveValue(
            valueAssertion: value =>
                value.Should().BeGreaterThan(minimumValue),
            because: "the result value should be greater than {0}",
            becauseArgs: minimumValue);
    }
}

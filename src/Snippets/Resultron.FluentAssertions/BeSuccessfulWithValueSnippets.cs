using System;
using FluentAssertions;
using Resultron;
using Resultron.FluentAssertions;

namespace Snippets.Resultron.FluentAssertions;

public sealed class BeSuccessfulWithValueSnippets
{
    // Generic Result<T>

    public void BeSuccessfulWithValue()
    {
        // Arrange
        Result<int> result = Result<int>.Success(42);

        // Assert
        result.Should().BeSuccessfulWithValue();
    }

    public void BeSuccessfulWithValueWithReason()
    {
        // Arrange
        Result<string> result = Result<string>.Success(
            "Operation completed.");

        // Assert
        result.Should().BeSuccessfulWithValue(
            because: "the operation should return a valid value");
    }

    public void BeSuccessfulWithExpectedValue()
    {
        // Arrange
        Result<int> result = Result<int>.Success(42);

        // Assert
        result.Should().BeSuccessfulWithValue(
            expectedValue: 42);
    }

    public void BeSuccessfulWithExpectedValueAndReason()
    {
        // Arrange
        int userId = 1;

        Result<int> result = Result<int>.Success(userId);

        // Assert
        result.Should().BeSuccessfulWithValue(
            expectedValue: userId,
            because: "user ID {0} should be returned successfully",
            becauseArgs: userId);
    }

    public void BeSuccessfulWithValueAssertion()
    {
        // Arrange
        Result<string> result = Result<string>.Success(
            "Operation completed.");

        // Assert
        result.Should().BeSuccessfulWithValue(
            valueAssertion: value =>
                value.Should().StartWith("Operation"));
    }

    public void BeSuccessfulWithValueAssertionAndReason()
    {
        // Arrange
        int minimumValue = 10;

        Result<int> result = Result<int>.Success(42);

        // Assert
        result.Should().BeSuccessfulWithValue(
            valueAssertion: value =>
                value.Should().BeGreaterThan(minimumValue),
            because: "the result value should be greater than {0}",
            becauseArgs: minimumValue);
    }
}

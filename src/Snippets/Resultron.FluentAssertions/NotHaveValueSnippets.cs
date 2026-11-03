using System;
using FluentAssertions;
using Resultron;
using Resultron.FluentAssertions;

namespace Snippets.Resultron.FluentAssertions;

public sealed class NotHaveValueSnippets
{
    // Generic Result<T>

    public void NotHaveValue()
    {
        // Arrange
        Result<int> result = Result<int>.Failure(new Error(
            Code: "Code",
            Description: "Description"));

        // Assert
        result.Should().NotHaveValue();
    }

    public void NotHaveValueWithReason()
    {
        // Arrange
        Result<string> result = Result<string>.Failure(new Error(
            Code: "InvalidOperation",
            Description: "Invalid operation."));

        // Assert
        result.Should().NotHaveValue(
            "the operation should not return a value when it fails");
    }

    public void NotHaveValueWithFormattedReason()
    {
        // Arrange
        int userId = -1;

        Result<int> result = Result<int>.Failure(new Error(
            Code: "InvalidUserId",
            Description: "Invalid user ID."));

        // Assert
        result.Should().NotHaveValue(
            "user ID {0} should not produce a value",
            userId);
    }
}

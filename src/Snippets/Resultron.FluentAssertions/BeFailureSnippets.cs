using System;
using Resultron;
using FluentAssertions;
using Resultron.FluentAssertions;

namespace Snippets.Resultron.FluentAssertions;

public sealed class BeFailureSnippets
{
    // Non-Generic Result

    public void BeFailure()
    {
        // Arrange
        Result result = Result.Failure(new Error(
            Code: "Code",
            Description: "Description"));

        // Assert
        result.Should().BeFailure();
    }

    public void BeFailureWithReason()
    {
        // Arrange
        Result result = Result.Failure(new Error(
            Code: "InvalidOperation",
            Description: "Invalid operation."));

        // Assert
        result.Should().BeFailure(
            "the operation should fail when the input is invalid");
    }

    public void BeFailureWithFormattedReason()
    {
        // Arrange
        int userId = -1;

        Result result = Result.Failure(new Error(
            Code: "InvalidUserId",
            Description: "Invalid user ID."));

        // Assert
        result.Should().BeFailure(
            "user ID {0} should not be accepted",
            userId);
    }

    // Generic Result<T>

    public void GenericBeFailure()
    {
        // Arrange
        Result<int> result = Result<int>.Failure(new Error(
            Code: "Code",
            Description: "Description"));

        // Assert
        result.Should().BeFailure();
    }

    public void GenericBeFailureWithReason()
    {
        // Arrange
        Result<string> result = Result<string>.Failure(new Error(
            Code: "InvalidOperation",
            Description: "Invalid operation."));

        // Assert
        result.Should().BeFailure(
            "the operation should fail when the input is invalid");
    }

    public void GenericBeFailureWithFormattedReason()
    {
        // Arrange
        int userId = -1;

        Result<int> result = Result<int>.Failure(new Error(
            Code: "InvalidUserId",
            Description: "Invalid user ID."));

        // Assert
        result.Should().BeFailure(
            "user ID {0} should not be accepted",
            userId);
    }
}
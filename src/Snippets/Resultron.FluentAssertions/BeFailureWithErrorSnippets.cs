using System;
using FluentAssertions;
using Resultron;
using Resultron.FluentAssertions;

namespace Snippets.Resultron.FluentAssertions;

public sealed class BeFailureWithErrorSnippets
{
    // Non-Generic Result

    public void BeFailureWithError()
    {
        // Arrange
        Error expectedError = new(
            Code: "Code",
            Description: "Description");

        Result result = Result.Failure(expectedError);

        // Assert
        result.Should().BeFailureWithError(expectedError);
    }

    public void BeFailureWithErrorWithReason()
    {
        // Arrange
        Error expectedError = new(
            Code: "InvalidOperation",
            Description: "Invalid operation.");

        Result result = Result.Failure(expectedError);

        // Assert
        result.Should().BeFailureWithError(
            expectedError,
            "the operation should fail with the expected error");
    }

    public void BeFailureWithErrorWithFormattedReason()
    {
        // Arrange
        int userId = -1;

        Error expectedError = new(
            Code: "InvalidUserId",
            Description: "Invalid user ID.");

        Result result = Result.Failure(expectedError);

        // Assert
        result.Should().BeFailureWithError(
            expectedError,
            "user ID {0} should produce the expected error",
            userId);
    }

    // Generic Result<T>

    public void GenericBeFailureWithError()
    {
        // Arrange
        Error expectedError = new(
            Code: "Code",
            Description: "Description");

        Result<int> result = Result<int>.Failure(expectedError);

        // Assert
        result.Should().BeFailureWithError(expectedError);
    }

    public void GenericBeFailureWithErrorWithReason()
    {
        // Arrange
        Error expectedError = new(
            Code: "InvalidOperation",
            Description: "Invalid operation.");

        Result<string> result = Result<string>.Failure(expectedError);

        // Assert
        result.Should().BeFailureWithError(
            expectedError,
            "the operation should fail with the expected error");
    }

    public void GenericBeFailureWithErrorWithFormattedReason()
    {
        // Arrange
        int userId = -1;

        Error expectedError = new(
            Code: "InvalidUserId",
            Description: "Invalid user ID.");

        Result<int> result = Result<int>.Failure(expectedError);

        // Assert
        result.Should().BeFailureWithError(
            expectedError,
            "user ID {0} should produce the expected error",
            userId);
    }
}

using System;
using FluentAssertions;
using Resultron;
using Resultron.FluentAssertions;

namespace Snippets.Resultron.FluentAssertions;

public sealed class HaveErrorCodeSnippets
{
    // Non-Generic Result

    public void HaveErrorCode()
    {
        // Arrange
        Result result = Result.Failure(new Error(
            Code: "Code",
            Description: "Description"));

        // Assert
        result.Should().HaveErrorCode("Code");
    }

    public void HaveErrorCodeWithReason()
    {
        // Arrange
        Result result = Result.Failure(new Error(
            Code: "InvalidOperation",
            Description: "Invalid operation."));

        // Assert
        result.Should().HaveErrorCode(
            "InvalidOperation",
            "the operation should fail with the expected error code");
    }

    public void HaveErrorCodeWithFormattedReason()
    {
        // Arrange
        int userId = -1;

        Result result = Result.Failure(new Error(
            Code: "InvalidUserId",
            Description: "Invalid user ID."));

        // Assert
        result.Should().HaveErrorCode(
            "InvalidUserId",
            "user ID {0} should produce the expected error code",
            userId);
    }

    // Generic Result<T>

    public void GenericHaveErrorCode()
    {
        // Arrange
        Result<int> result = Result<int>.Failure(new Error(
            Code: "Code",
            Description: "Description"));

        // Assert
        result.Should().HaveErrorCode("Code");
    }

    public void GenericHaveErrorCodeWithReason()
    {
        // Arrange
        Result<string> result = Result<string>.Failure(new Error(
            Code: "InvalidOperation",
            Description: "Invalid operation."));

        // Assert
        result.Should().HaveErrorCode(
            "InvalidOperation",
            "the operation should fail with the expected error code");
    }

    public void GenericHaveErrorCodeWithFormattedReason()
    {
        // Arrange
        int userId = -1;

        Result<int> result = Result<int>.Failure(new Error(
            Code: "InvalidUserId",
            Description: "Invalid user ID."));

        // Assert
        result.Should().HaveErrorCode(
            "InvalidUserId",
            "user ID {0} should produce the expected error code",
            userId);
    }
}

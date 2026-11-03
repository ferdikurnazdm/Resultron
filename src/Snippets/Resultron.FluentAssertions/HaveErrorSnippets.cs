using System;
using FluentAssertions;
using Resultron;
using Resultron.FluentAssertions;

namespace Snippets.Resultron.FluentAssertions;

public sealed class HaveErrorSnippets
{
    // Non-Generic Result

    public void HaveError()
    {
        // Arrange
        Result result = Result.Failure(new Error(
            Code: "Code",
            Description: "Description"));

        // Assert
        result.Should().HaveError();
    }

    public void HaveErrorWithReason()
    {
        // Arrange
        Result result = Result.Failure(new Error(
            Code: "InvalidOperation",
            Description: "Invalid operation."));

        // Assert
        result.Should().HaveError(
            because: "the operation should produce an error");
    }

    public void HaveErrorWithExpectedMessage()
    {
        // Arrange
        Result result = Result.Failure(new Error(
            Code: "ValidationError",
            Description: "Validation failed."));

        // Assert
        result.Should().HaveError("Validation failed.");
    }

    public void HaveErrorWithExpectedMessageAndReason()
    {
        // Arrange
        int userId = -1;

        Result result = Result.Failure(new Error(
            Code: "InvalidUserId",
            Description: "Invalid user ID."));

        // Assert
        result.Should().HaveError(
            expectedErrorMessage: "Invalid user ID.",
            because: "user ID {0} should produce a validation error",
            becauseArgs: userId);
    }

    // Generic Result<T>

    public void GenericHaveError()
    {
        // Arrange
        Result<int> result = Result<int>.Failure(new Error(
            Code: "Code",
            Description: "Description"));

        // Assert
        result.Should().HaveError();
    }

    public void GenericHaveErrorWithReason()
    {
        // Arrange
        Result<string> result = Result<string>.Failure(new Error(
            Code: "InvalidOperation",
            Description: "Invalid operation."));

        // Assert
        result.Should().HaveError(
            because: "the operation should produce an error");
    }

    public void GenericHaveErrorWithExpectedMessage()
    {
        // Arrange
        Result<int> result = Result<int>.Failure(new Error(
            Code: "ValidationError",
            Description: "Validation failed."));

        // Assert
        result.Should().HaveError("Validation failed.");
    }

    public void GenericHaveErrorWithExpectedMessageAndReason()
    {
        // Arrange
        int userId = -1;

        Result<int> result = Result<int>.Failure(new Error(
            Code: "InvalidUserId",
            Description: "Invalid user ID."));

        // Assert
        result.Should().HaveError(
            expectedErrorMessage: "Invalid user ID.",
            because: "user ID {0} should produce a validation error",
            becauseArgs: userId);
    }
}

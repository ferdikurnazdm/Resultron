using System;
using FluentAssertions;
using Resultron;
using Resultron.FluentAssertions;

namespace Snippets.Resultron.FluentAssertions;

public sealed class ErrorDetailsSnippets
{
    // Non-Generic Result

    public void HaveException()
    {
        // Arrange
        Exception exception = new InvalidOperationException(
            "Invalid operation.");

        Error error = new Error(
            Code: "InvalidOperation",
            Description: "Invalid operation.")
            .CausedBy(exception);

        Result result = Result.Failure(error);

        // Assert
        result.Should().HaveException<InvalidOperationException>();
    }

    public void HaveExceptionWithReason()
    {
        // Arrange
        Exception exception = new ArgumentException(
            "Invalid argument.");

        Error error = new Error(
            Code: "InvalidArgument",
            Description: "Invalid argument.")
            .CausedBy(exception);

        Result result = Result.Failure(error);

        // Assert
        result.Should().HaveException<ArgumentException>(
            "the operation should contain an argument exception");
    }

    public void HaveExceptionWithFormattedReason()
    {
        // Arrange
        int userId = -1;

        Exception exception = new ArgumentOutOfRangeException(
            nameof(userId),
            "User ID must be positive.");

        Error error = new Error(
            Code: "InvalidUserId",
            Description: "Invalid user ID.")
            .CausedBy(exception);

        Result result = Result.Failure(error);

        // Assert
        result.Should().HaveException<ArgumentOutOfRangeException>(
            "user ID {0} should produce an argument out of range exception",
            userId);
    }

    // Generic Result<T>

    public void GenericHaveException()
    {
        // Arrange
        Exception exception = new InvalidOperationException(
            "Invalid operation.");

        Error error = new Error(
            Code: "InvalidOperation",
            Description: "Invalid operation.")
            .CausedBy(exception);

        Result<int> result = Result<int>.Failure(error);

        // Assert
        result.Should().HaveException<InvalidOperationException>();
    }

    public void GenericHaveExceptionWithReason()
    {
        // Arrange
        Exception exception = new ArgumentException(
            "Invalid argument.");

        Error error = new Error(
            Code: "InvalidArgument",
            Description: "Invalid argument.")
            .CausedBy(exception);

        Result<string> result = Result<string>.Failure(error);

        // Assert
        result.Should().HaveException<ArgumentException>(
            "the operation should contain an argument exception");
    }

    public void GenericHaveExceptionWithFormattedReason()
    {
        // Arrange
        int userId = -1;

        Exception exception = new ArgumentOutOfRangeException(
            nameof(userId),
            "User ID must be positive.");

        Error error = new Error(
            Code: "InvalidUserId",
            Description: "Invalid user ID.")
            .CausedBy(exception);

        Result<int> result = Result<int>.Failure(error);

        // Assert
        result.Should().HaveException<ArgumentOutOfRangeException>(
            "user ID {0} should produce an argument out of range exception",
            userId);
    }
}

using System;
using FluentAssertions;
using Resultron;
using Resultron.FluentAssertions;

namespace Snippets.Resultron.FluentAssertions;

public sealed class CausedBySnippets
{
    // Non-Generic Result

    public void HaveCausedBy()
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
        result.Should().HaveCausedBy<InvalidOperationException>();
    }

    public void HaveCausedByWithReason()
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
        result.Should().HaveCausedBy<ArgumentException>(
            "the operation should fail due to an invalid argument");
    }

    public void HaveCausedByWithFormattedReason()
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
        result.Should().HaveCausedBy<ArgumentOutOfRangeException>(
            "user ID {0} should cause an argument out of range exception",
            userId);
    }

    // Generic Result<T>

    public void GenericHaveCausedBy()
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
        result.Should().HaveCausedBy<InvalidOperationException>();
    }

    public void GenericHaveCausedByWithReason()
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
        result.Should().HaveCausedBy<ArgumentException>(
            "the operation should fail due to an invalid argument");
    }

    public void GenericHaveCausedByWithFormattedReason()
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
        result.Should().HaveCausedBy<ArgumentOutOfRangeException>(
            "user ID {0} should cause an argument out of range exception",
            userId);
    }
}

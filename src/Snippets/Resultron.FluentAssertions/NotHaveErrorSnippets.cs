using System;
using FluentAssertions;
using Resultron;
using Resultron.FluentAssertions;

namespace Snippets.Resultron.FluentAssertions;

public sealed class NotHaveErrorSnippets
{
    // Non-Generic Result

    public void NotHaveError()
    {
        // Arrange
        Result result = Result.Success();

        // Assert
        result.Should().NotHaveError();
    }

    public void NotHaveErrorWithReason()
    {
        // Arrange
        Result result = Result.Success();

        // Assert
        result.Should().NotHaveError(
            "the operation should complete without errors");
    }

    public void NotHaveErrorWithFormattedReason()
    {
        // Arrange
        int userId = 1;

        Result result = Result.Success();

        // Assert
        result.Should().NotHaveError(
            "user ID {0} should be processed without errors",
            userId);
    }

    // Generic Result<T>

    public void GenericNotHaveError()
    {
        // Arrange
        Result<int> result = Result<int>.Success(42);

        // Assert
        result.Should().NotHaveError();
    }

    public void GenericNotHaveErrorWithReason()
    {
        // Arrange
        Result<string> result = Result<string>.Success(
            "Operation completed.");

        // Assert
        result.Should().NotHaveError(
            "the operation should complete without errors");
    }

    public void GenericNotHaveErrorWithFormattedReason()
    {
        // Arrange
        int userId = 1;

        Result<int> result = Result<int>.Success(userId);

        // Assert
        result.Should().NotHaveError(
            "user ID {0} should be processed without errors",
            userId);
    }
}

using System;
using FluentAssertions;
using Resultron;
using Resultron.FluentAssertions;

namespace Snippets.Resultron.FluentAssertions;

public sealed class BeSuccessfulSnippets
{
    // Non-Generic Result

    public void BeSuccessful()
    {
        // Arrange
        Result result = Result.Success();

        // Assert
        result.Should().BeSuccessful();
    }

    public void BeSuccessfulWithReason()
    {
        // Arrange
        Result result = Result.Success();

        // Assert
        result.Should().BeSuccessful(
            "the operation should complete successfully");
    }

    public void BeSuccessfulWithFormattedReason()
    {
        // Arrange
        int userId = 1;

        Result result = Result.Success();

        // Assert
        result.Should().BeSuccessful(
            "user ID {0} should be processed successfully",
            userId);
    }

    // Generic Result<T>

    public void GenericBeSuccessful()
    {
        // Arrange
        Result<int> result = Result<int>.Success(42);

        // Assert
        result.Should().BeSuccessful();
    }

    public void GenericBeSuccessfulWithReason()
    {
        // Arrange
        Result<string> result = Result<string>.Success("Operation completed.");

        // Assert
        result.Should().BeSuccessful(
            "the operation should return a successful result");
    }

    public void GenericBeSuccessfulWithFormattedReason()
    {
        // Arrange
        int userId = 1;

        Result<int> result = Result<int>.Success(userId);

        // Assert
        result.Should().BeSuccessful(
            "user ID {0} should be processed successfully",
            userId);
    }
}

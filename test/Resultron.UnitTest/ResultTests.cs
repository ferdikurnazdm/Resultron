using FluentAssertions;

namespace Resultron.UnitTest;

public sealed class ResultTests
{
    [Fact]
    public void Success_WhenCalled_ShouldReturnSuccessfulResult()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Failure_WhenCalledWithError_ShouldReturnFailedResultWithError()
    {
        // Arrange
        var error = new Error("E001", "Something went wrong");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Map_WhenResultIsSuccess_ShouldTransformValue()
    {
        // Arrange & Act
        var result = Result.Success()
            .Map(() => 1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(1);
    }

    [Fact]
    public void Map_WhenResultIsFailure_ShouldPropagateError()
    {
        // Arrange
        var error = new Error("E006");

        // Act
        var result = Result.Failure(error)
            .Map(() => 1);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Match_WhenResultIsSuccess_ShouldInvokeOnSuccess()
    {
        // Arrange
        var result = Result.Success();
        var successCalled = false;
        var failureCalled = false;

        // Act
        result.Match(
            onSuccess: () => successCalled = true,
            onFailure: _ => failureCalled = true
        );

        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
    }

    [Fact]
    public void Match_WhenResultIsFailure_ShouldInvokeOnFailureWithError()
    {
        // Arrange
        var error = new Error("E002");
        var result = Result.Failure(error);
        var successCalled = false;
        var failureCalled = false;
        Error? capturedError = null;

        // Act
        result.Match(
            onSuccess: () => successCalled = true,
            onFailure: e =>
            {
                failureCalled = true;
                capturedError = e;
            }
        );

        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
        capturedError.Should().Be(error);
    }

    [Fact]
    public void Match_WhenResultIsSuccess_ShouldReturnOnSuccessValue()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var value = result.Match(
            onSuccess: () => "success",
            onFailure: _ => "failure"
        );

        // Assert
        value.Should().Be("success");
    }

    [Fact]
    public void Match_WhenResultIsFailure_ShouldReturnOnFailureValue()
    {
        // Arrange
        var result = Result.Failure(new Error("E005"));

        // Act
        var value = result.Match(
            onSuccess: () => "success",
            onFailure: _ => "failure"
        );

        // Assert
        value.Should().Be("failure");
    }

    [Fact]
    public void Try_WhenActionDoesNotThrow_ShouldReturnSuccess()
    {
        // Arrange & Act
        var result = Result.Try(() => { });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Try_WhenActionThrowsException_ShouldReturnFailureWithExceptionDetails()
    {
        // Arrange & Act
        var result = Result.Try(() => throw new InvalidOperationException("fail"));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(nameof(InvalidOperationException));
        result.Error.Description.Should().Be("fail");
    }

    [Fact]
    public void Try_WhenActionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange & Act
        var act = () => Result.Try(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task TryAsync_WhenActionIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange & Act
        var act = async () => await Result.TryAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task TryAsync_WhenActionDoesNotThrow_ShouldReturnSuccess()
    {
        // Arrange & Act
        var result = await Result.TryAsync(async () => await Task.CompletedTask);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public async Task TryAsync_WhenActionThrowsException_ShouldReturnFailureWithExceptionDetails()
    {
        // Arrange & Act
        var result = await Result.TryAsync(async () =>
        {
            await Task.CompletedTask;
            throw new InvalidOperationException("async fail");
        });

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(nameof(InvalidOperationException));
        result.Error.Description.Should().Be("async fail");
    }

    [Fact]
    public void Bind_WhenAllStepsSucceed_ShouldReturnLastResult()
    {
        // Arrange & Act
        var result = Result.Success()
            .Bind(Result.Success)
            .Bind(() => Result.Failure(new Error("E003")));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("E003");
    }

    [Fact]
    public void Bind_WhenInitialResultIsFailure_ShouldNotExecuteNextStepAndPropagateError()
    {
        // Arrange
        var error = new Error("E004");

        // Act
        var result = Result.Failure(error)
            .Bind(Result.Success);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void BindT_WhenResultIsSuccess_ShouldReturnTypedResult()
    {
        // Arrange & Act
        var result = Result.Success()
            .Bind(() => Result<int>.Success(42));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void BindT_WhenResultIsFailure_ShouldPropagateError()
    {
        // Arrange
        var error = new Error("E007");

        // Act
        var result = Result.Failure(error)
            .Bind(() => Result<int>.Success(42));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

}

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

    [Fact]
    public void ImplicitConversion_FromError_ShouldReturnFailureResult()
    {
        // Arrange
        var error = new Error("E008");

        // Act
        Result result = error;

        // Assert
        result.IsSuccess.Should().BeFalse();
        
        result.Error.Should().Be(error);
    }

        [Fact]
    public async Task MatchAsync_Should_Call_Success_Callback_When_Result_Is_Success()
    {
        // Arrange
        var successCalled = false;

        var result = Result.Success();

        // Act
        await result.MatchAsync(
            () =>
            {
                successCalled = true;
                return Task.CompletedTask;
            },
            _ => Task.CompletedTask);

        // Assert
        successCalled.Should().BeTrue();
    }


    [Fact]
    public async Task MatchAsync_Should_Call_Failure_Callback_When_Result_Is_Failure()
    {
        // Arrange
        var error = new Error(
            "TEST_ERROR",
            "Test error");

        var result = Result.Failure(error);

        Error? receivedError = null;

        // Act
        await result.MatchAsync(
            () => Task.CompletedTask,
            e =>
            {
                receivedError = e;
                return Task.CompletedTask;
            });

        // Assert
        receivedError.Should().Be(error);
    }


    [Fact]
    public async Task MatchAsync_Should_Return_Success_Value_When_Result_Is_Success()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var response = await result.MatchAsync(
            () => Task.FromResult(10),
            _ => Task.FromResult(0));

        // Assert
        response.Should().Be(10);
    }


    [Fact]
    public async Task MapAsync_Should_Return_Mapped_Value_When_Result_Is_Success()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var mapped = await result.MapAsync(async () =>
        {
            await Task.Delay(1);
            return "success";
        });

        // Assert
        mapped.IsSuccess.Should().BeTrue();
        mapped.Value.Should().Be("success");
    }


    [Fact]
    public async Task MapAsync_Should_Return_Error_When_Result_Is_Failure()
    {
        // Arrange
        var error = new Error(
            "FAILED",
            "Something went wrong");

        var result = Result.Failure(error);

        // Act
        var mapped = await result.MapAsync(async () =>
        {
            await Task.Delay(1);
            return "value";
        });

        // Assert
        mapped.IsSuccess.Should().BeFalse();
        mapped.Error.Should().Be(error);
    }


    [Fact]
    public async Task BindAsync_Should_Return_New_Result_When_Result_Is_Success()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var response = await result.BindAsync(async () =>
        {
            await Task.Delay(1);
            return Result.Success();
        });

        // Assert
        response.IsSuccess.Should().BeTrue();
    }


    [Fact]
    public async Task BindAsync_Should_Not_Execute_Function_When_Result_Is_Failure()
    {
        // Arrange
        var error = new Error(
            "FAILED",
            "Something went wrong");

        var result = Result.Failure(error);

        var called = false;

        // Act
        var response = await result.BindAsync(() =>
        {
            called = true;
            return Task.FromResult(Result.Success());
        });

        // Assert
        called.Should().BeFalse();

        response.IsSuccess.Should().BeFalse();
        response.Error.Should().Be(error);
    }

}

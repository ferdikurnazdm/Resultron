using FluentAssertions;

namespace Resultron.UnitTest;

public sealed class ResultOfTTests
{
    [Fact]
    public void Success_WhenCalledWithValue_ShouldReturnSuccessfulResultWithValue()
    {
        // Arrange & Act
        var result = Result<int>.Success(42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Failure_WhenCalledWithError_ShouldReturnFailedResult()
    {
        // Arrange
        var error = new Error("E001", "Something went wrong");

        // Act
        var result = Result<int>.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Try_WhenFuncDoesNotThrow_ShouldReturnSuccessWithValue()
    {
        // Arrange & Act
        var result = Result<int>.Try(() => 42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Try_WhenFuncThrowsException_ShouldReturnFailureWithExceptionDetails()
    {
        // Arrange & Act
        var result = Result<int>.Try(() => throw new InvalidOperationException("fail"));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(nameof(InvalidOperationException));
        result.Error.Description.Should().Be("fail");
    }

    [Fact]
    public void Try_WhenFuncIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange & Act
        var act = () => Result<int>.Try(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task TryAsync_WhenFuncDoesNotThrow_ShouldReturnSuccessWithValue()
    {
        // Arrange & Act
        var result = await Result<int>.TryAsync(async () =>
        {
            await Task.CompletedTask;
            return 42;
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public async Task TryAsync_WhenFuncThrowsException_ShouldReturnFailureWithExceptionDetails()
    {
        // Arrange & Act
        var result = await Result<int>.TryAsync(async () =>
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
    public async Task TryAsync_WhenFuncIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange & Act
        var act = async () => await Result<int>.TryAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public void Match_WhenResultIsSuccess_ShouldInvokeOnSuccessWithValue()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var capturedValue = 0;
        var failureCalled = false;

        // Act
        result.Match(
            onSuccess: v => capturedValue = v,
            onFailure: _ => failureCalled = true
        );

        // Assert
        capturedValue.Should().Be(42);
        failureCalled.Should().BeFalse();
    }

    [Fact]
    public void Match_WhenResultIsFailure_ShouldInvokeOnFailureWithError()
    {
        // Arrange
        var error = new Error("E002");
        var result = Result<int>.Failure(error);
        var successCalled = false;
        Error? capturedError = null;

        // Act
        result.Match(
            onSuccess: _ => successCalled = true,
            onFailure: e => capturedError = e
        );

        // Assert
        successCalled.Should().BeFalse();
        capturedError.Should().Be(error);
    }

    [Fact]
    public void MatchT_WhenResultIsSuccess_ShouldReturnOnSuccessValue()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var value = result.Match(
            onSuccess: v => $"Value: {v}",
            onFailure: _ => "failure"
        );

        // Assert
        value.Should().Be("Value: 42");
    }

    [Fact]
    public void MatchT_WhenResultIsFailure_ShouldReturnOnFailureValue()
    {
        // Arrange
        var result = Result<int>.Failure(new Error("E003"));

        // Act
        var value = result.Match(
            onSuccess: v => $"Value: {v}",
            onFailure: _ => "failure"
        );

        // Assert
        value.Should().Be("failure");
    }

    [Fact]
    public void Map_WhenResultIsSuccess_ShouldExecuteActionAndReturnSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var capturedValue = 0;

        // Act
        var mappedResult = result.Map(v => capturedValue = v);

        // Assert
        mappedResult.IsSuccess.Should().BeTrue();
        capturedValue.Should().Be(42);
    }

    [Fact]
    public void Map_WhenResultIsFailure_ShouldNotExecuteActionAndPropagateError()
    {
        // Arrange
        var error = new Error("E004");
        var result = Result<int>.Failure(error);
        var actionCalled = false;

        // Act
        var mappedResult = result.Map(_ => actionCalled = true);

        // Assert
        mappedResult.IsSuccess.Should().BeFalse();
        mappedResult.Error.Should().Be(error);
        actionCalled.Should().BeFalse();
    }

    [Fact]
    public void MapT_WhenResultIsSuccess_ShouldTransformValue()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var mappedResult = result.Map(v => v.ToString());

        // Assert
        mappedResult.IsSuccess.Should().BeTrue();
        mappedResult.Value.Should().Be("42");
    }

    [Fact]
    public void MapT_WhenResultIsFailure_ShouldPropagateError()
    {
        // Arrange
        var error = new Error("E005");
        var result = Result<int>.Failure(error);

        // Act
        var mappedResult = result.Map(v => v.ToString());

        // Assert
        mappedResult.IsSuccess.Should().BeFalse();
        mappedResult.Error.Should().Be(error);
    }

    [Fact]
    public void Bind_WhenResultIsSuccess_ShouldChainAndReturnNextResult()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var boundResult = result.Bind(v => Result<string>.Success(v.ToString()));

        // Assert
        boundResult.IsSuccess.Should().BeTrue();
        boundResult.Value.Should().Be("42");
    }

    [Fact]
    public void Bind_WhenResultIsFailure_ShouldNotExecuteNextStepAndPropagateError()
    {
        // Arrange
        var error = new Error("E006");
        var result = Result<int>.Failure(error);
        var funcCalled = false;

        // Act
        var boundResult = result.Bind(v =>
        {
            funcCalled = true;
            return Result<string>.Success(v.ToString());
        });

        // Assert
        boundResult.IsSuccess.Should().BeFalse();
        boundResult.Error.Should().Be(error);
        funcCalled.Should().BeFalse();
    }

    [Fact]
    public void Bind_WhenChainedStepFails_ShouldReturnFailure()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var error = new Error("E007");

        // Act
        var boundResult = result.Bind(_ => Result<string>.Failure(error));

        // Assert
        boundResult.IsSuccess.Should().BeFalse();
        boundResult.Error.Should().Be(error);
    }

    [Fact]
    public void Bind_WhenResultIsSuccess_ShouldExecuteFuncAndReturnResult()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var boundResult = result.Bind(_ => Result.Success());

        // Assert
        boundResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Bind_WhenResultIsFailure_ShouldNotExecuteFuncAndPropagateError()
    {
        // Arrange
        var error = new Error("E008");
        var result = Result<int>.Failure(error);
        var funcCalled = false;

        // Act
        var boundResult = result.Bind(_ =>
        {
            funcCalled = true;
            return Result.Success();
        });

        // Assert
        boundResult.IsSuccess.Should().BeFalse();
        boundResult.Error.Should().Be(error);
        funcCalled.Should().BeFalse();
    }

    [Fact]
    public void Bind_WhenFuncReturnsFailure_ShouldPropagateError()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var error = new Error("E009");

        // Act
        var boundResult = result.Bind(_ => Result.Failure(error));

        // Assert
        boundResult.IsSuccess.Should().BeFalse();
        boundResult.Error.Should().Be(error);
    }

    [Fact]
    public void Bind_WhenFuncIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var act = () => result.Bind(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ImplicitConversion_FromError_ShouldReturnFailureResult()
    {
        // Arrange
        var error = new Error("E010");

        // Act
        Result<int> result = error;

        // Assert
        result.IsSuccess.Should().BeFalse();

        result.Error.Should().Be(error);
    }

    [Fact]
    public void ImplicitConversion_FromValue_ShouldReturnSuccessResult()
    {
        // Arrange
        var value = 42;

        // Act
        Result<int> result = value;

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().Be(value);
    }

       [Fact]
    public async Task MapAsync_Should_Map_Value_When_Result_Is_Success()
    {
        // Arrange
        Result<int> result = 10;

        // Act
        var mapped = await result.MapAsync(async value =>
        {
            await Task.Delay(1);
            return value * 2;
        });

        // Assert
        mapped.IsSuccess.Should().BeTrue();
        mapped.Value.Should().Be(20);
    }


    [Fact]
    public async Task MapAsync_Action_Should_Execute_When_Result_Is_Success()
    {
        // Arrange
        Result<int> result = 10;

        var calledValue = 0;

        // Act
        var response = await result.MapAsync(async value =>
        {
            await Task.Delay(1);
            calledValue = value;
        });

        // Assert
        response.IsSuccess.Should().BeTrue();
        calledValue.Should().Be(10);
    }


    [Fact]
    public async Task MapAsync_Should_Return_Error_When_Result_Is_Failure()
    {
        // Arrange
        var error = new Error(
            "FAILED",
            "Error");

        Result<int> result = error;

        // Act
        var mapped = await result.MapAsync(async value =>
        {
            await Task.Delay(1);
            return value * 2;
        });

        // Assert
        mapped.IsSuccess.Should().BeFalse();
        mapped.Error.Should().Be(error);
    }


    [Fact]
    public async Task BindAsync_Should_Return_New_ResultT_When_Result_Is_Success()
    {
        // Arrange
        Result<int> result = 10;

        // Act
        var response = await result.BindAsync(async value =>
        {
            await Task.Delay(1);
            return Result<string>.Success(value.ToString());
        });

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().Be("10");
    }


    [Fact]
    public async Task BindAsync_Should_Not_Call_Function_When_Result_Is_Failure()
    {
        // Arrange
        var error = new Error(
            "FAILED",
            "Error");

        Result<int> result = error;

        var called = false;

        // Act
        var response = await result.BindAsync(value =>
        {
            called = true;
            return Task.FromResult(Result<string>.Success(value.ToString()));
        });

        // Assert
        called.Should().BeFalse();

        response.IsSuccess.Should().BeFalse();
        response.Error.Should().Be(error);
    }


    [Fact]
    public async Task MatchAsync_Should_Pass_Value_To_Success_Callback()
    {
        // Arrange
        Result<int> result = 50;

        var receivedValue = 0;

        // Act
        await result.MatchAsync(
            value =>
            {
                receivedValue = value;
                return Task.CompletedTask;
            },
            _ => Task.CompletedTask);

        // Assert
        receivedValue.Should().Be(50);
    }
}

using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Resultron.UnitTest;

public sealed class OnSuccessTests
{
    private static readonly Error TestError =
        new("test.error", "Test error.");

    #region Result

    [Fact]
    public void OnSuccess_Should_Invoke_Action_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Action action =
            Substitute.For<Action>();

        // Act
        Result result = source.OnSuccess(action);

        // Assert
        action.Received(1).Invoke();

        result.Should().BeSameAs(source);
    }

    [Fact]
    public void OnSuccess_Should_Not_Invoke_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result source =
            Result.Failure(TestError);

        Action action =
            Substitute.For<Action>();

        // Act
        Result result = source.OnSuccess(action);

        // Assert
        action.DidNotReceive().Invoke();

        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void OnSuccess_Should_Preserve_Result_State()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result result =
            source.OnSuccess(() => { });

        // Assert
        result.Should().BeSameAs(source);
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    #endregion

    #region Result<T>

    [Fact]
    public void OnSuccess_Generic_Should_Invoke_Action_When_Result_Is_Success()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Action<int> action =
            Substitute.For<Action<int>>();

        // Act
        Result<int> result =
            source.OnSuccess(action);

        // Assert
        action.Received(1)
            .Invoke(42);

        result.Should().BeSameAs(source);
    }

    [Fact]
    public void OnSuccess_Generic_Should_Pass_Value_To_Action()
    {
        // Arrange
        Result<string> source =
            Result<string>.Success("Resultron");

        string? receivedValue = null;

        // Act
        Result<string> result = source.OnSuccess(
            value => receivedValue = value);

        // Assert
        receivedValue.Should().Be("Resultron");

        result.Should().BeSameAs(source);
        result.Value.Should().Be("Resultron");
    }

    [Fact]
    public void OnSuccess_Generic_Should_Not_Invoke_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Action<int> action =
            Substitute.For<Action<int>>();

        // Act
        Result<int> result =
            source.OnSuccess(action);

        // Assert
        action.DidNotReceive()
            .Invoke(Arg.Any<int>());

        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void OnSuccess_Generic_Should_Preserve_Value()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result =
            source.OnSuccess(_ => { });

        // Assert
        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    #endregion

    #region Result Async

    [Fact]
    public async Task OnSuccessAsync_Should_Invoke_Action_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Func<Task> action =
            Substitute.For<Func<Task>>();

        action()
            .Returns(Task.CompletedTask);

        // Act
        Result result =
            await source.OnSuccessAsync(action);

        // Assert
        await action.Received(1).Invoke();

        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task OnSuccessAsync_Should_Not_Invoke_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result source =
            Result.Failure(TestError);

        Func<Task> action =
            Substitute.For<Func<Task>>();

        // Act
        Result result =
            await source.OnSuccessAsync(action);

        // Assert
        await action.DidNotReceive().Invoke();

        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task OnSuccessAsync_Should_Await_Action()
    {
        // Arrange
        Result source = Result.Success();

        var actionCompleted = false;

        // Act
        Result result = await source.OnSuccessAsync(
            async () =>
            {
                await Task.Yield();

                actionCompleted = true;
            });

        // Assert
        actionCompleted.Should().BeTrue();
        result.Should().BeSameAs(source);
    }

    #endregion

    #region Result<T> Async

    [Fact]
    public async Task OnSuccessAsync_Generic_Should_Invoke_Action_When_Result_Is_Success()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Func<int, Task> action =
            Substitute.For<Func<int, Task>>();

        action(42)
            .Returns(Task.CompletedTask);

        // Act
        Result<int> result =
            await source.OnSuccessAsync(action);

        // Assert
        await action.Received(1)
            .Invoke(42);

        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task OnSuccessAsync_Generic_Should_Pass_Value_To_Action()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        var receivedValue = 0;

        // Act
        Result<int> result = await source.OnSuccessAsync(
            async value =>
            {
                await Task.Yield();

                receivedValue = value;
            });

        // Assert
        receivedValue.Should().Be(42);

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task OnSuccessAsync_Generic_Should_Not_Invoke_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, Task> action =
            Substitute.For<Func<int, Task>>();

        // Act
        Result<int> result =
            await source.OnSuccessAsync(action);

        // Assert
        await action.DidNotReceive()
            .Invoke(Arg.Any<int>());

        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task OnSuccessAsync_Generic_Should_Await_Action()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        var actionCompleted = false;

        // Act
        Result<int> result = await source.OnSuccessAsync(
            async value =>
            {
                await Task.Yield();

                actionCompleted = true;
            });

        // Assert
        actionCompleted.Should().BeTrue();

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    #endregion

    #region Fluent Chaining

    [Fact]
    public void OnSuccess_Should_Preserve_Type_For_Fluent_Chaining()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result = source
            .OnSuccess(_ => { })
            .OnSuccess(_ => { });

        // Assert
        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public void OnSuccess_Should_Chain_With_OnFailure_Without_Losing_Type()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result = source
            .OnSuccess(_ => { })
            .OnFailure(_ => { });

        // Assert
        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task OnSuccessAsync_Should_Preserve_Type_For_Further_Chaining()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> afterAsync =
            await source.OnSuccessAsync(
                _ => Task.CompletedTask);

        Result<int> result =
            afterAsync.OnSuccess(_ => { });

        // Assert
        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    #endregion

    #region Null Guards

    [Fact]
    public void OnSuccess_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Action act = () =>
            result.OnSuccess(() => { });

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void OnSuccess_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Result result = Result.Success();
        Action action = null!;

        // Act
        Action act = () =>
            result.OnSuccess(action);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public void OnSuccess_Generic_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Action act = () =>
            result.OnSuccess(_ => { });

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void OnSuccess_Generic_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Action<int> action = null!;

        // Act
        Action act = () =>
            result.OnSuccess(action);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public async Task OnSuccessAsync_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Func<Task> act = async () =>
            await result.OnSuccessAsync(
                () => Task.CompletedTask);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task OnSuccessAsync_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Result result = Result.Success();
        Func<Task> action = null!;

        // Act
        Func<Task> act = async () =>
            await result.OnSuccessAsync(action);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public async Task OnSuccessAsync_Generic_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Func<Task> act = async () =>
            await result.OnSuccessAsync(
                _ => Task.CompletedTask);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task OnSuccessAsync_Generic_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Func<int, Task> action = null!;

        // Act
        Func<Task> act = async () =>
            await result.OnSuccessAsync(action);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    #endregion
}
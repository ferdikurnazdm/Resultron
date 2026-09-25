using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Resultron.UnitTest;

public sealed class OnFailureTests
{
    private static readonly Error TestError =
        new("test.error", "Test error.");

    #region Result

    [Fact]
    public void OnFailure_Should_Invoke_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result source = Result.Failure(TestError);

        Action<Error> action =
            Substitute.For<Action<Error>>();

        // Act
        Result result = source.OnFailure(action);

        // Assert
        action.Received(1)
            .Invoke(TestError);

        result.Should().BeSameAs(source);
    }

    [Fact]
    public void OnFailure_Should_Not_Invoke_Action_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Action<Error> action =
            Substitute.For<Action<Error>>();

        // Act
        Result result = source.OnFailure(action);

        // Assert
        action.DidNotReceive()
            .Invoke(Arg.Any<Error>());

        result.Should().BeSameAs(source);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void OnFailure_Should_Pass_Primary_Error_To_Action()
    {
        // Arrange
        var firstError =
            new Error("first.error", "First error.");

        var secondError =
            new Error("second.error", "Second error.");

        Result source = Result.Failure(
        [
            firstError,
            secondError
        ]);

        Action<Error> action =
            Substitute.For<Action<Error>>();

        // Act
        _ = source.OnFailure(action);

        // Assert
        action.Received(1)
            .Invoke(firstError);
    }

    [Fact]
    public void OnFailure_Should_Preserve_Result_State()
    {
        // Arrange
        Result source = Result.Failure(TestError);

        // Act
        Result result = source.OnFailure(_ => { });

        // Assert
        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    #endregion

    #region Result<T>

    [Fact]
    public void OnFailure_Generic_Should_Invoke_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Action<Error> action =
            Substitute.For<Action<Error>>();

        // Act
        Result<int> result =
            source.OnFailure(action);

        // Assert
        action.Received(1)
            .Invoke(TestError);

        result.Should().BeSameAs(source);
    }

    [Fact]
    public void OnFailure_Generic_Should_Not_Invoke_Action_When_Result_Is_Success()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Action<Error> action =
            Substitute.For<Action<Error>>();

        // Act
        Result<int> result =
            source.OnFailure(action);

        // Assert
        action.DidNotReceive()
            .Invoke(Arg.Any<Error>());

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public void OnFailure_Generic_Should_Pass_Primary_Error_To_Action()
    {
        // Arrange
        var firstError =
            new Error("first.error", "First error.");

        var secondError =
            new Error("second.error", "Second error.");

        Result<int> source = Result<int>.Failure(
        [
            firstError,
            secondError
        ]);

        Action<Error> action =
            Substitute.For<Action<Error>>();

        // Act
        _ = source.OnFailure(action);

        // Assert
        action.Received(1)
            .Invoke(firstError);
    }

    [Fact]
    public void OnFailure_Generic_Should_Preserve_Concrete_Result_Type()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        // Act
        Result<int> result =
            source.OnFailure(_ => { });

        // Assert
        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
    }

    #endregion

    #region Result Async

    [Fact]
    public async Task OnFailureAsync_Should_Invoke_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result source = Result.Failure(TestError);

        Func<Error, Task> action =
            Substitute.For<Func<Error, Task>>();

        action(TestError)
            .Returns(Task.CompletedTask);

        // Act
        Result result =
            await source.OnFailureAsync(action);

        // Assert
        await action.Received(1)
            .Invoke(TestError);

        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task OnFailureAsync_Should_Not_Invoke_Action_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Func<Error, Task> action =
            Substitute.For<Func<Error, Task>>();

        // Act
        Result result =
            await source.OnFailureAsync(action);

        // Assert
        await action.DidNotReceive()
            .Invoke(Arg.Any<Error>());

        result.Should().BeSameAs(source);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task OnFailureAsync_Should_Await_Action()
    {
        // Arrange
        Result source = Result.Failure(TestError);

        var actionCompleted = false;

        // Act
        Result result = await source.OnFailureAsync(
            async error =>
            {
                await Task.Yield();

                actionCompleted = true;
            });

        // Assert
        actionCompleted.Should().BeTrue();
        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task OnFailureAsync_Should_Pass_Primary_Error_To_Action()
    {
        // Arrange
        var firstError =
            new Error("first.error", "First error.");

        var secondError =
            new Error("second.error", "Second error.");

        Result source = Result.Failure(
        [
            firstError,
            secondError
        ]);

        Func<Error, Task> action =
            Substitute.For<Func<Error, Task>>();

        action(firstError)
            .Returns(Task.CompletedTask);

        // Act
        _ = await source.OnFailureAsync(action);

        // Assert
        await action.Received(1)
            .Invoke(firstError);
    }

    #endregion

    #region Result<T> Async

    [Fact]
    public async Task OnFailureAsync_Generic_Should_Invoke_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<Error, Task> action =
            Substitute.For<Func<Error, Task>>();

        action(TestError)
            .Returns(Task.CompletedTask);

        // Act
        Result<int> result =
            await source.OnFailureAsync(action);

        // Assert
        await action.Received(1)
            .Invoke(TestError);

        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task OnFailureAsync_Generic_Should_Not_Invoke_Action_When_Result_Is_Success()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Func<Error, Task> action =
            Substitute.For<Func<Error, Task>>();

        // Act
        Result<int> result =
            await source.OnFailureAsync(action);

        // Assert
        await action.DidNotReceive()
            .Invoke(Arg.Any<Error>());

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task OnFailureAsync_Generic_Should_Await_Action()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        var actionCompleted = false;

        // Act
        Result<int> result = await source.OnFailureAsync(
            async error =>
            {
                await Task.Yield();

                actionCompleted = true;
            });

        // Assert
        actionCompleted.Should().BeTrue();
        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task OnFailureAsync_Generic_Should_Preserve_Concrete_Result_Type()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        // Act
        Result<int> result = await source.OnFailureAsync(
            _ => Task.CompletedTask);

        // Assert
        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    #endregion

    #region Fluent Chaining

    [Fact]
    public void OnFailure_Should_Preserve_Type_For_Fluent_Chaining()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        // Act
        Result<int> result = source
            .OnFailure(_ => { })
            .OnFailure(_ => { });

        // Assert
        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task OnFailureAsync_Should_Preserve_Type_For_Further_Chaining()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        // Act
        Result<int> afterAsync =
            await source.OnFailureAsync(
                _ => Task.CompletedTask);

        Result<int> result =
            afterAsync.OnFailure(_ => { });

        // Assert
        result.Should().BeSameAs(source);
    }

    #endregion

    #region Null Guards

    [Fact]
    public void OnFailure_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Action act = () =>
            result.OnFailure(_ => { });

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void OnFailure_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Result result =
            Result.Failure(TestError);

        Action<Error> action = null!;

        // Act
        Action act = () =>
            result.OnFailure(action);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public void OnFailure_Generic_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Action act = () =>
            result.OnFailure(_ => { });

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void OnFailure_Generic_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Failure(TestError);

        Action<Error> action = null!;

        // Act
        Action act = () =>
            result.OnFailure(action);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public async Task OnFailureAsync_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Func<Task> act = async () =>
            await result.OnFailureAsync(
                _ => Task.CompletedTask);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task OnFailureAsync_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Result result =
            Result.Failure(TestError);

        Func<Error, Task> action = null!;

        // Act
        Func<Task> act = async () =>
            await result.OnFailureAsync(action);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public async Task OnFailureAsync_Generic_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Func<Task> act = async () =>
            await result.OnFailureAsync(
                _ => Task.CompletedTask);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task OnFailureAsync_Generic_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Failure(TestError);

        Func<Error, Task> action = null!;

        // Act
        Func<Task> act = async () =>
            await result.OnFailureAsync(action);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    #endregion
}
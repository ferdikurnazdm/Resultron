using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Resultron.UnitTest;

public sealed class TapTests
{
    private static readonly Error TestError =
        new("test.error", "Test error.");

    #region Tap - Result

    [Fact]
    public void Tap_Should_Invoke_Action_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Action action =
            Substitute.For<Action>();

        // Act
        Result result = source.Tap(action);

        // Assert
        action.Received(1).Invoke();

        result.Should().BeSameAs(source);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Tap_Should_Not_Invoke_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result source =
            Result.Failure(TestError);

        Action action =
            Substitute.For<Action>();

        // Act
        Result result = source.Tap(action);

        // Assert
        action.DidNotReceive().Invoke();

        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Tap_Should_Return_Original_Result_Instance()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result result =
            source.Tap(() => { });

        // Assert
        result.Should().BeSameAs(source);
    }

    #endregion

    #region Tap - Result<T>

    [Fact]
    public void Tap_Generic_Should_Invoke_Action_When_Result_Is_Success()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Action<int> action =
            Substitute.For<Action<int>>();

        // Act
        Result<int> result =
            source.Tap(action);

        // Assert
        action.Received(1)
            .Invoke(42);

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Tap_Generic_Should_Pass_Value_To_Action()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        var receivedValue = 0;

        // Act
        Result<int> result = source.Tap(
            value => receivedValue = value);

        // Assert
        receivedValue.Should().Be(42);

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Tap_Generic_Should_Not_Invoke_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Action<int> action =
            Substitute.For<Action<int>>();

        // Act
        Result<int> result =
            source.Tap(action);

        // Assert
        action.DidNotReceive()
            .Invoke(Arg.Any<int>());

        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void Tap_Generic_Should_Preserve_Value()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result =
            source.Tap(_ => { });

        // Assert
        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    #endregion

    #region TapAsync - Result

    [Fact]
    public async Task TapAsync_Should_Invoke_Action_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Func<Task> action =
            Substitute.For<Func<Task>>();

        action()
            .Returns(Task.CompletedTask);

        // Act
        Result result =
            await source.TapAsync(action);

        // Assert
        await action.Received(1).Invoke();

        result.Should().BeSameAs(source);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task TapAsync_Should_Not_Invoke_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result source =
            Result.Failure(TestError);

        Func<Task> action =
            Substitute.For<Func<Task>>();

        // Act
        Result result =
            await source.TapAsync(action);

        // Assert
        await action.DidNotReceive().Invoke();

        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task TapAsync_Should_Await_Action()
    {
        // Arrange
        Result source = Result.Success();

        var completed = false;

        // Act
        Result result = await source.TapAsync(
            async () =>
            {
                await Task.Yield();

                completed = true;
            });

        // Assert
        completed.Should().BeTrue();
        result.Should().BeSameAs(source);
    }

    #endregion

    #region TapAsync - Result<T>

    [Fact]
    public async Task TapAsync_Generic_Should_Invoke_Action_When_Result_Is_Success()
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
            await source.TapAsync(action);

        // Assert
        await action.Received(1)
            .Invoke(42);

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task TapAsync_Generic_Should_Pass_Value_To_Action()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        var receivedValue = 0;

        // Act
        Result<int> result = await source.TapAsync(
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
    public async Task TapAsync_Generic_Should_Not_Invoke_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, Task> action =
            Substitute.For<Func<int, Task>>();

        // Act
        Result<int> result =
            await source.TapAsync(action);

        // Assert
        await action.DidNotReceive()
            .Invoke(Arg.Any<int>());

        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task TapAsync_Generic_Should_Await_Action()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        var completed = false;

        // Act
        Result<int> result = await source.TapAsync(
            async value =>
            {
                await Task.Yield();

                completed = true;
            });

        // Assert
        completed.Should().BeTrue();

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    #endregion

    #region TapAsync - Task<Result> With Sync Action

    [Fact]
    public async Task TapAsync_Task_Result_Should_Invoke_Sync_Action_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Task<Result> resultTask =
            Task.FromResult(source);

        Action action =
            Substitute.For<Action>();

        // Act
        Result result =
            await resultTask.TapAsync(action);

        // Assert
        action.Received(1).Invoke();

        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task TapAsync_Task_Result_Should_Not_Invoke_Sync_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result source =
            Result.Failure(TestError);

        Task<Result> resultTask =
            Task.FromResult(source);

        Action action =
            Substitute.For<Action>();

        // Act
        Result result =
            await resultTask.TapAsync(action);

        // Assert
        action.DidNotReceive().Invoke();

        result.Should().BeSameAs(source);
        result.Error.Should().Be(TestError);
    }

    #endregion

    #region TapAsync - Task<Result> With Async Action

    [Fact]
    public async Task TapAsync_Task_Result_Should_Invoke_Async_Action_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Task<Result> resultTask =
            Task.FromResult(source);

        Func<Task> action =
            Substitute.For<Func<Task>>();

        action()
            .Returns(Task.CompletedTask);

        // Act
        Result result =
            await resultTask.TapAsync(action);

        // Assert
        await action.Received(1).Invoke();

        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task TapAsync_Task_Result_Should_Not_Invoke_Async_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result source =
            Result.Failure(TestError);

        Task<Result> resultTask =
            Task.FromResult(source);

        Func<Task> action =
            Substitute.For<Func<Task>>();

        // Act
        Result result =
            await resultTask.TapAsync(action);

        // Assert
        await action.DidNotReceive().Invoke();

        result.Should().BeSameAs(source);
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task TapAsync_Task_Result_Should_Await_Async_Action()
    {
        // Arrange
        Result source = Result.Success();

        Task<Result> resultTask =
            Task.FromResult(source);

        var completed = false;

        // Act
        Result result = await resultTask.TapAsync(
            async () =>
            {
                await Task.Yield();

                completed = true;
            });

        // Assert
        completed.Should().BeTrue();
        result.Should().BeSameAs(source);
    }

    #endregion

    #region TapAsync - Task<Result<T>> With Sync Action

    [Fact]
    public async Task TapAsync_Task_Generic_Result_Should_Invoke_Sync_Action_With_Value()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Task<Result<int>> resultTask =
            Task.FromResult(source);

        Action<int> action =
            Substitute.For<Action<int>>();

        // Act
        Result<int> result =
            await resultTask.TapAsync(action);

        // Assert
        action.Received(1)
            .Invoke(42);

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task TapAsync_Task_Generic_Result_Should_Not_Invoke_Sync_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Task<Result<int>> resultTask =
            Task.FromResult(source);

        Action<int> action =
            Substitute.For<Action<int>>();

        // Act
        Result<int> result =
            await resultTask.TapAsync(action);

        // Assert
        action.DidNotReceive()
            .Invoke(Arg.Any<int>());

        result.Should().BeSameAs(source);
        result.Error.Should().Be(TestError);
    }

    #endregion

    #region TapAsync - Task<Result<T>> With Async Action

    [Fact]
    public async Task TapAsync_Task_Generic_Result_Should_Invoke_Async_Action_With_Value()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Task<Result<int>> resultTask =
            Task.FromResult(source);

        Func<int, Task> action =
            Substitute.For<Func<int, Task>>();

        action(42)
            .Returns(Task.CompletedTask);

        // Act
        Result<int> result =
            await resultTask.TapAsync(action);

        // Assert
        await action.Received(1)
            .Invoke(42);

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task TapAsync_Task_Generic_Result_Should_Not_Invoke_Async_Action_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Task<Result<int>> resultTask =
            Task.FromResult(source);

        Func<int, Task> action =
            Substitute.For<Func<int, Task>>();

        // Act
        Result<int> result =
            await resultTask.TapAsync(action);

        // Assert
        await action.DidNotReceive()
            .Invoke(Arg.Any<int>());

        result.Should().BeSameAs(source);
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task TapAsync_Task_Generic_Result_Should_Await_Async_Action()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Task<Result<int>> resultTask =
            Task.FromResult(source);

        var completed = false;
        var receivedValue = 0;

        // Act
        Result<int> result = await resultTask.TapAsync(
            async value =>
            {
                await Task.Yield();

                receivedValue = value;
                completed = true;
            });

        // Assert
        completed.Should().BeTrue();
        receivedValue.Should().Be(42);

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    #endregion

    #region Fluent Chaining

    [Fact]
    public void Tap_Should_Preserve_Generic_Type_For_Fluent_Chaining()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result = source
            .Tap(_ => { })
            .Tap(_ => { })
            .OnSuccess(_ => { });

        // Assert
        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task TapAsync_Should_Preserve_Generic_Type_For_Further_Chaining()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> afterTap =
            await source.TapAsync(
                _ => Task.CompletedTask);

        Result<int> result =
            afterTap.Tap(_ => { });

        // Assert
        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    #endregion

    #region Null Guards - Result

    [Fact]
    public void Tap_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Action act = () =>
            result.Tap(() => { });

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Tap_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Result result = Result.Success();

        Action action = null!;

        // Act
        Action act = () =>
            result.Tap(action);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public async Task TapAsync_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Func<Task> act = async () =>
            await result.TapAsync(
                () => Task.CompletedTask);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task TapAsync_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Result result = Result.Success();

        Func<Task> action = null!;

        // Act
        Func<Task> act = async () =>
            await result.TapAsync(action);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    #endregion

    #region Null Guards - Result<T>

    [Fact]
    public void Tap_Generic_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Action act = () =>
            result.Tap(_ => { });

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Tap_Generic_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Action<int> action = null!;

        // Act
        Action act = () =>
            result.Tap(action);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public async Task TapAsync_Generic_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Func<Task> act = async () =>
            await result.TapAsync(
                _ => Task.CompletedTask);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task TapAsync_Generic_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Func<int, Task> action = null!;

        // Act
        Func<Task> act = async () =>
            await result.TapAsync(action);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    #endregion

    #region Null Guards - Task<Result>

    [Fact]
    public async Task TapAsync_Task_Result_With_Sync_Action_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result> resultTask = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.TapAsync(
                () => { });

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    [Fact]
    public async Task TapAsync_Task_Result_With_Sync_Action_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        Action action = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.TapAsync(action);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public async Task TapAsync_Task_Result_With_Async_Action_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result> resultTask = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.TapAsync(
                () => Task.CompletedTask);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    [Fact]
    public async Task TapAsync_Task_Result_With_Async_Action_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Task<Result> resultTask =
            Task.FromResult(Result.Success());

        Func<Task> action = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.TapAsync(action);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    #endregion

    #region Null Guards - Task<Result<T>>

    [Fact]
    public async Task TapAsync_Task_Generic_Result_With_Sync_Action_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result<int>> resultTask = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.TapAsync(
                value => { });

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    [Fact]
    public async Task TapAsync_Task_Generic_Result_With_Sync_Action_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Task<Result<int>> resultTask =
            Task.FromResult(
                Result<int>.Success(42));

        Action<int> action = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.TapAsync(action);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public async Task TapAsync_Task_Generic_Result_With_Async_Action_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result<int>> resultTask = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.TapAsync(
                value => Task.CompletedTask);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    [Fact]
    public async Task TapAsync_Task_Generic_Result_With_Async_Action_Should_Throw_When_Action_Is_Null()
    {
        // Arrange
        Task<Result<int>> resultTask =
            Task.FromResult(
                Result<int>.Success(42));

        Func<int, Task> action = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.TapAsync(action);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    #endregion
}
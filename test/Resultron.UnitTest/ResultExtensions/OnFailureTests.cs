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
        Result source = Result.Failure(TestError);

        Action<Error> action =
            Substitute.For<Action<Error>>();

        Result result = source.OnFailure(action);

        action.Received(1)
            .Invoke(TestError);

        result.Should().BeSameAs(source);
    }

    [Fact]
    public void OnFailure_Should_Not_Invoke_Action_When_Result_Is_Success()
    {
        Result source = Result.Success();

        Action<Error> action =
            Substitute.For<Action<Error>>();

        Result result = source.OnFailure(action);

        action.DidNotReceive()
            .Invoke(Arg.Any<Error>());

        result.Should().BeSameAs(source);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void OnFailure_Should_Pass_Error_To_Action()
    {
        Result source =
            Result.Failure(TestError);

        Action<Error> action =
            Substitute.For<Action<Error>>();

        _ = source.OnFailure(action);

        action.Received(1)
            .Invoke(TestError);
    }

    [Fact]
    public void OnFailure_Should_Preserve_Result_State()
    {
        Result source =
            Result.Failure(TestError);

        Result result =
            source.OnFailure(_ => { });

        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    #endregion

    #region Result<T>

    [Fact]
    public void OnFailure_Generic_Should_Invoke_Action_When_Result_Is_Failure()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Action<Error> action =
            Substitute.For<Action<Error>>();

        Result<int> result =
            source.OnFailure(action);

        action.Received(1)
            .Invoke(TestError);

        result.Should().BeSameAs(source);
    }

    [Fact]
    public void OnFailure_Generic_Should_Not_Invoke_Action_When_Result_Is_Success()
    {
        Result<int> source =
            Result<int>.Success(42);

        Action<Error> action =
            Substitute.For<Action<Error>>();

        Result<int> result =
            source.OnFailure(action);

        action.DidNotReceive()
            .Invoke(Arg.Any<Error>());

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public void OnFailure_Generic_Should_Pass_Error_To_Action()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Action<Error> action =
            Substitute.For<Action<Error>>();

        _ = source.OnFailure(action);

        action.Received(1)
            .Invoke(TestError);
    }

    [Fact]
    public void OnFailure_Generic_Should_Preserve_Concrete_Result_Type()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Result<int> result =
            source.OnFailure(_ => { });

        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
    }

    #endregion

    #region Result Async

    [Fact]
    public async Task OnFailureAsync_Should_Invoke_Action_When_Result_Is_Failure()
    {
        Result source =
            Result.Failure(TestError);

        Func<Error, Task> action =
            Substitute.For<Func<Error, Task>>();

        action(TestError)
            .Returns(Task.CompletedTask);

        Result result =
            await source.OnFailureAsync(action);

        await action.Received(1)
            .Invoke(TestError);

        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task OnFailureAsync_Should_Not_Invoke_Action_When_Result_Is_Success()
    {
        Result source =
            Result.Success();

        Func<Error, Task> action =
            Substitute.For<Func<Error, Task>>();

        Result result =
            await source.OnFailureAsync(action);

        await action.DidNotReceive()
            .Invoke(Arg.Any<Error>());

        result.Should().BeSameAs(source);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task OnFailureAsync_Should_Await_Action()
    {
        Result source =
            Result.Failure(TestError);

        bool actionCompleted = false;

        Result result = await source.OnFailureAsync(
            async _ =>
            {
                await Task.Yield();
                actionCompleted = true;
            });

        actionCompleted.Should().BeTrue();
        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task OnFailureAsync_Should_Pass_Error_To_Action()
    {
        Result source =
            Result.Failure(TestError);

        Func<Error, Task> action =
            Substitute.For<Func<Error, Task>>();

        action(TestError)
            .Returns(Task.CompletedTask);

        _ = await source.OnFailureAsync(action);

        await action.Received(1)
            .Invoke(TestError);
    }

    #endregion

    #region Result<T> Async

    [Fact]
    public async Task OnFailureAsync_Generic_Should_Invoke_Action_When_Result_Is_Failure()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<Error, Task> action =
            Substitute.For<Func<Error, Task>>();

        action(TestError)
            .Returns(Task.CompletedTask);

        Result<int> result =
            await source.OnFailureAsync(action);

        await action.Received(1)
            .Invoke(TestError);

        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task OnFailureAsync_Generic_Should_Not_Invoke_Action_When_Result_Is_Success()
    {
        Result<int> source =
            Result<int>.Success(42);

        Func<Error, Task> action =
            Substitute.For<Func<Error, Task>>();

        Result<int> result =
            await source.OnFailureAsync(action);

        await action.DidNotReceive()
            .Invoke(Arg.Any<Error>());

        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task OnFailureAsync_Generic_Should_Await_Action()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        bool actionCompleted = false;

        Result<int> result = await source.OnFailureAsync(
            async _ =>
            {
                await Task.Yield();
                actionCompleted = true;
            });

        actionCompleted.Should().BeTrue();
        result.Should().BeSameAs(source);
    }

    [Fact]
    public async Task OnFailureAsync_Generic_Should_Preserve_Concrete_Result_Type()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Result<int> result = await source.OnFailureAsync(
            _ => Task.CompletedTask);

        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    #endregion

    #region Fluent Chaining

    [Fact]
    public void OnFailure_Should_Preserve_Type_For_Fluent_Chaining()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Result<int> result = source
            .OnFailure(_ => { })
            .OnFailure(_ => { });

        result.Should().BeSameAs(source);
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task OnFailureAsync_Should_Preserve_Type_For_Further_Chaining()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Result<int> afterAsync =
            await source.OnFailureAsync(
                _ => Task.CompletedTask);

        Result<int> result =
            afterAsync.OnFailure(_ => { });

        result.Should().BeSameAs(source);
    }

    #endregion

    #region Null Guards

    [Fact]
    public void OnFailure_Should_Throw_When_Result_Is_Null()
    {
        Result result = null!;

        Action act = () =>
            result.OnFailure(_ => { });

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void OnFailure_Should_Throw_When_Action_Is_Null()
    {
        Result result =
            Result.Failure(TestError);

        Action<Error> action = null!;

        Action act = () =>
            result.OnFailure(action);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public void OnFailure_Generic_Should_Throw_When_Result_Is_Null()
    {
        Result<int> result = null!;

        Action act = () =>
            result.OnFailure(_ => { });

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void OnFailure_Generic_Should_Throw_When_Action_Is_Null()
    {
        Result<int> result =
            Result<int>.Failure(TestError);

        Action<Error> action = null!;

        Action act = () =>
            result.OnFailure(action);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public async Task OnFailureAsync_Should_Throw_When_Result_Is_Null()
    {
        Result result = null!;

        Func<Task> act = async () =>
            await result.OnFailureAsync(
                _ => Task.CompletedTask);

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task OnFailureAsync_Should_Throw_When_Action_Is_Null()
    {
        Result result =
            Result.Failure(TestError);

        Func<Error, Task> action = null!;

        Func<Task> act = async () =>
            await result.OnFailureAsync(action);

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public async Task OnFailureAsync_Generic_Should_Throw_When_Result_Is_Null()
    {
        Result<int> result = null!;

        Func<Task> act = async () =>
            await result.OnFailureAsync(
                _ => Task.CompletedTask);

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task OnFailureAsync_Generic_Should_Throw_When_Action_Is_Null()
    {
        Result<int> result =
            Result<int>.Failure(TestError);

        Func<Error, Task> action = null!;

        Func<Task> act = async () =>
            await result.OnFailureAsync(action);

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("action");
    }

    #endregion
}
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Resultron.UnitTest;

public sealed class BindTests
{
    private static readonly Error TestError =
        new("test.error", "Test error.");

    #region Result -> Result

    [Fact]
    public void Bind_Result_Should_Invoke_Binder_When_Source_Is_Success()
    {
        Result source = Result.Success();

        Func<Result> binder =
            Substitute.For<Func<Result>>();

        binder()
            .Returns(Result.Success());

        Result result = source.Bind(binder);

        result.IsSuccess.Should().BeTrue();

        binder.Received(1)
            .Invoke();
    }

    [Fact]
    public void Bind_Result_Should_Not_Invoke_Binder_When_Source_Is_Failure()
    {
        Result source =
            Result.Failure(TestError);

        Func<Result> binder =
            Substitute.For<Func<Result>>();

        Result result = source.Bind(binder);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        binder.DidNotReceive()
            .Invoke();
    }

    [Fact]
    public void Bind_Result_Should_Return_Binder_Result()
    {
        Result source = Result.Success();

        var expectedError =
            new Error(
                "binder.failed",
                "Binder failed.");

        Func<Result> binder = () =>
            Result.Failure(expectedError);

        Result result = source.Bind(binder);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(expectedError);
    }

    #endregion

    #region Result -> Result<T>

    [Fact]
    public void Bind_Result_Should_Return_Generic_Result_When_Binder_Succeeds()
    {
        Result source = Result.Success();

        Result<int> result = source.Bind(
            () => Result<int>.Success(42));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Bind_Result_Should_Propagate_Error_When_Source_Is_Failure()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Result source =
            Result.Failure(error);

        Result<int> result = source.Bind(
            () => Result<int>.Success(42));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region Result<T> -> Result

    [Fact]
    public void Bind_Generic_Result_Should_Pass_Value_To_Binder()
    {
        Result<int> source =
            Result<int>.Success(42);

        var receivedValue = 0;

        Result result = source.Bind(
            value =>
            {
                receivedValue = value;

                return Result.Success();
            });

        result.IsSuccess.Should().BeTrue();
        receivedValue.Should().Be(42);
    }

    [Fact]
    public void Bind_Generic_Result_Should_Not_Invoke_Binder_When_Source_Is_Failure()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, Result> binder =
            Substitute.For<Func<int, Result>>();

        Result result = source.Bind(binder);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        binder.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    [Fact]
    public void Bind_Generic_Result_Should_Propagate_Error_To_Non_Generic_Result()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Result<int> source =
            Result<int>.Failure(error);

        Result result = source.Bind(
            _ => Result.Success());

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region Result<T> -> Result<TOut>

    [Fact]
    public void Bind_Generic_Result_Should_Map_To_New_Result_Type()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<string> result = source.Bind(
            value => Result<string>.Success(
                $"Value: {value}"));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void Bind_Generic_Result_Should_Propagate_Binder_Failure()
    {
        Result<int> source =
            Result<int>.Success(42);

        var error =
            new Error(
                "conversion.failed",
                "Conversion failed.");

        Result<string> result = source.Bind(
            _ => Result<string>.Failure(error));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Bind_Generic_Result_Should_Propagate_Error_When_Source_Fails()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Result<int> source =
            Result<int>.Failure(error);

        Func<int, Result<string>> binder =
            Substitute.For<Func<int, Result<string>>>();

        Result<string> result =
            source.Bind(binder);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);

        binder.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    #endregion

    #region Result -> async Result

    [Fact]
    public async Task BindAsync_Result_Should_Invoke_Async_Binder_When_Successful()
    {
        Result source = Result.Success();

        Func<Task<Result>> binder =
            Substitute.For<Func<Task<Result>>>();

        binder()
            .Returns(Task.FromResult(Result.Success()));

        Result result =
            await source.BindAsync(binder);

        result.IsSuccess.Should().BeTrue();

        await binder.Received(1)
            .Invoke();
    }

    [Fact]
    public async Task BindAsync_Result_Should_Not_Invoke_Binder_When_Source_Fails()
    {
        Result source =
            Result.Failure(TestError);

        Func<Task<Result>> binder =
            Substitute.For<Func<Task<Result>>>();

        Result result =
            await source.BindAsync(binder);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        await binder.DidNotReceive()
            .Invoke();
    }

    #endregion

    #region Result<T> -> async Result

    [Fact]
    public async Task BindAsync_Generic_Result_Should_Pass_Value_To_Non_Generic_Binder()
    {
        Result<int> source =
            Result<int>.Success(42);

        var receivedValue = 0;

        Result result = await source.BindAsync(
            async value =>
            {
                await Task.Yield();

                receivedValue = value;

                return Result.Success();
            });

        result.IsSuccess.Should().BeTrue();
        receivedValue.Should().Be(42);
    }

    [Fact]
    public async Task BindAsync_Generic_Result_Should_Propagate_Error_When_Source_Fails()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Result<int> source =
            Result<int>.Failure(error);

        Result result = await source.BindAsync(
            value => Task.FromResult(
                Result.Success()));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region Result<T> -> async Result<TOut>

    [Fact]
    public async Task BindAsync_Generic_Result_Should_Map_To_New_Result_Type()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<string> result = await source.BindAsync(
            async value =>
            {
                await Task.Yield();

                return Result<string>.Success(
                    $"Value: {value}");
            });

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public async Task BindAsync_Generic_Result_Should_Not_Invoke_Binder_When_Source_Fails()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, Task<Result<string>>> binder =
            Substitute.For<Func<int, Task<Result<string>>>>();

        Result<string> result =
            await source.BindAsync(binder);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        await binder.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    #endregion

    #region Task<Result>

    [Fact]
    public async Task BindAsync_Task_Result_Should_Bind_After_Source_Completes()
    {
        Task<Result> source =
            Task.FromResult(Result.Success());

        Result result = await source.BindAsync(
            () => Result.Success());

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task BindAsync_Task_Result_Should_Support_Async_Binder()
    {
        Task<Result> source =
            Task.FromResult(Result.Success());

        Result result = await source.BindAsync(
            async () =>
            {
                await Task.Yield();

                return Result.Success();
            });

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task BindAsync_Task_Result_Should_Bind_To_Generic_Result()
    {
        Task<Result> source =
            Task.FromResult(Result.Success());

        Result<int> result = await source.BindAsync(
            () => Result<int>.Success(42));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task BindAsync_Task_Result_Should_Bind_To_Generic_Result_Asynchronously()
    {
        Task<Result> source =
            Task.FromResult(Result.Success());

        Result<int> result = await source.BindAsync(
            async () =>
            {
                await Task.Yield();

                return Result<int>.Success(42);
            });

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    #endregion

    #region Task<Result<T>>

    [Fact]
    public async Task BindAsync_Task_Generic_Result_Should_Bind_To_New_Type()
    {
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        Result<string> result = await source.BindAsync(
            value => Result<string>.Success(
                value.ToString()));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_Task_Generic_Result_Should_Support_Async_Binder()
    {
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        Result<string> result = await source.BindAsync(
            async value =>
            {
                await Task.Yield();

                return Result<string>.Success(
                    value.ToString());
            });

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    #endregion

    #region Null Guards

    [Fact]
    public void Bind_Should_Throw_When_Result_Is_Null()
    {
        Result result = null!;

        Action act = () =>
            result.Bind(() => Result.Success());

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Bind_Should_Throw_When_Binder_Is_Null()
    {
        Result result = Result.Success();
        Func<Result> binder = null!;

        Action act = () =>
            result.Bind(binder);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("binder");
    }

    [Fact]
    public async Task BindAsync_Should_Throw_When_Result_Is_Null()
    {
        Result<int> result = null!;

        Func<Task> act = async () =>
            await result.BindAsync(
                value => Task.FromResult(
                    Result.Success()));

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task BindAsync_Should_Throw_When_ResultTask_Is_Null()
    {
        Task<Result> resultTask = null!;

        Func<Task> act = async () =>
            await resultTask.BindAsync(
                () => Result.Success());

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    #endregion
}
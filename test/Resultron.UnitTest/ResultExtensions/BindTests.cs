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
        // Arrange
        Result source = Result.Success();
        Func<Result> binder = Substitute.For<Func<Result>>();

        binder()
            .Returns(Result.Success());

        // Act
        Result result = source.Bind(binder);

        // Assert
        result.IsSuccess.Should().BeTrue();

        binder.Received(1).Invoke();
    }

    [Fact]
    public void Bind_Result_Should_Not_Invoke_Binder_When_Source_Is_Failure()
    {
        // Arrange
        Result source = Result.Failure(TestError);
        Func<Result> binder = Substitute.For<Func<Result>>();

        // Act
        Result result = source.Bind(binder);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        binder.DidNotReceive().Invoke();
    }

    [Fact]
    public void Bind_Result_Should_Return_Binder_Result()
    {
        // Arrange
        Result source = Result.Success();

        var expectedError =
            new Error("binder.failed", "Binder failed.");

        Func<Result> binder = () =>
            Result.Failure(expectedError);

        // Act
        Result result = source.Bind(binder);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(expectedError);
    }

    #endregion

    #region Result -> Result<T>

    [Fact]
    public void Bind_Result_Should_Return_Generic_Result_When_Binder_Succeeds()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result<int> result = source.Bind(
            () => Result<int>.Success(42));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Bind_Result_Should_Propagate_Reasons_When_Source_Is_Failure()
    {
        // Arrange
        var error =
            new Error("source.failed", "Source failed.");

        var successReason =
            new Success("Context information.");

        Result source = Result.Failure(
            new IReason[]
            {
                error,
                successReason
            });

        // Act
        Result<int> result = source.Bind(
            () => Result<int>.Success(42));

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should().HaveCount(2);
        result.Reasons.Should().Contain(error);
        result.Reasons.Should().Contain(successReason);
    }

    #endregion

    #region Result<T> -> Result

    [Fact]
    public void Bind_Generic_Result_Should_Pass_Value_To_Binder()
    {
        // Arrange
        Result<int> source = Result<int>.Success(42);

        var receivedValue = 0;

        // Act
        Result result = source.Bind(value =>
        {
            receivedValue = value;

            return Result.Success();
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        receivedValue.Should().Be(42);
    }

    [Fact]
    public void Bind_Generic_Result_Should_Not_Invoke_Binder_When_Source_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, Result> binder =
            Substitute.For<Func<int, Result>>();

        // Act
        Result result = source.Bind(binder);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        binder.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    [Fact]
    public void Bind_Generic_Result_Should_Propagate_Reasons_To_Non_Generic_Result()
    {
        // Arrange
        var error =
            new Error("source.failed", "Source failed.");

        var context =
            new Success("Context.");

        Result<int> source = Result<int>.Failure(
            new IReason[]
            {
                error,
                context
            });

        // Act
        Result result = source.Bind(
            _ => Result.Success());

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should().BeEquivalentTo(
            source.Reasons);
    }

    #endregion

    #region Result<T> -> Result<TOut>

    [Fact]
    public void Bind_Generic_Result_Should_Map_To_New_Result_Type()
    {
        // Arrange
        Result<int> source = Result<int>.Success(42);

        // Act
        Result<string> result = source.Bind(
            value => Result<string>.Success(
                $"Value: {value}"));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void Bind_Generic_Result_Should_Propagate_Binder_Failure()
    {
        // Arrange
        Result<int> source = Result<int>.Success(42);

        var error =
            new Error("conversion.failed", "Conversion failed.");

        // Act
        Result<string> result = source.Bind(
            _ => Result<string>.Failure(error));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Bind_Generic_Result_Should_Propagate_All_Reasons_When_Source_Fails()
    {
        // Arrange
        var error =
            new Error("source.failed", "Source failed.");

        var context =
            new Success("Context.");

        Result<int> source = Result<int>.Failure(
            new IReason[]
            {
                error,
                context
            });

        Func<int, Result<string>> binder =
            Substitute.For<Func<int, Result<string>>>();

        // Act
        Result<string> result = source.Bind(binder);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should().BeEquivalentTo(
            source.Reasons);

        binder.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    #endregion

    #region Result -> async Result

    [Fact]
    public async Task BindAsync_Result_Should_Invoke_Async_Binder_When_Successful()
    {
        // Arrange
        Result source = Result.Success();

        Func<Task<Result>> binder =
            Substitute.For<Func<Task<Result>>>();

        binder()
            .Returns(Task.FromResult(Result.Success()));

        // Act
        Result result = await source.BindAsync(binder);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await binder.Received(1).Invoke();
    }

    [Fact]
    public async Task BindAsync_Result_Should_Not_Invoke_Binder_When_Source_Fails()
    {
        // Arrange
        Result source = Result.Failure(TestError);

        Func<Task<Result>> binder =
            Substitute.For<Func<Task<Result>>>();

        // Act
        Result result = await source.BindAsync(binder);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        await binder.DidNotReceive().Invoke();
    }

    #endregion

    #region Result<T> -> async Result

    [Fact]
    public async Task BindAsync_Generic_Result_Should_Pass_Value_To_Non_Generic_Binder()
    {
        // Arrange
        Result<int> source = Result<int>.Success(42);

        var receivedValue = 0;

        // Act
        Result result = await source.BindAsync(
            async value =>
            {
                await Task.Yield();

                receivedValue = value;

                return Result.Success();
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        receivedValue.Should().Be(42);
    }

    [Fact]
    public async Task BindAsync_Generic_Result_Should_Propagate_Reasons_When_Source_Fails()
    {
        // Arrange
        var error =
            new Error("source.failed", "Source failed.");

        var context =
            new Success("Context.");

        Result<int> source = Result<int>.Failure(
            new IReason[]
            {
                error,
                context
            });

        // Act
        Result result = await source.BindAsync(
            value => Task.FromResult(Result.Success()));

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should().BeEquivalentTo(
            source.Reasons);
    }

    #endregion

    #region Result<T> -> async Result<TOut>

    [Fact]
    public async Task BindAsync_Generic_Result_Should_Map_To_New_Result_Type()
    {
        // Arrange
        Result<int> source = Result<int>.Success(42);

        // Act
        Result<string> result = await source.BindAsync(
            async value =>
            {
                await Task.Yield();

                return Result<string>.Success(
                    $"Value: {value}");
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public async Task BindAsync_Generic_Result_Should_Not_Invoke_Binder_When_Source_Fails()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, Task<Result<string>>> binder =
            Substitute.For<Func<int, Task<Result<string>>>>();

        // Act
        Result<string> result =
            await source.BindAsync(binder);

        // Assert
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
        // Arrange
        Task<Result> source =
            Task.FromResult(Result.Success());

        // Act
        Result result = await source.BindAsync(
            () => Result.Success());

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task BindAsync_Task_Result_Should_Support_Async_Binder()
    {
        // Arrange
        Task<Result> source =
            Task.FromResult(Result.Success());

        // Act
        Result result = await source.BindAsync(
            async () =>
            {
                await Task.Yield();

                return Result.Success();
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task BindAsync_Task_Result_Should_Bind_To_Generic_Result()
    {
        // Arrange
        Task<Result> source =
            Task.FromResult(Result.Success());

        // Act
        Result<int> result = await source.BindAsync(
            () => Result<int>.Success(42));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task BindAsync_Task_Result_Should_Bind_To_Generic_Result_Asynchronously()
    {
        // Arrange
        Task<Result> source =
            Task.FromResult(Result.Success());

        // Act
        Result<int> result = await source.BindAsync(
            async () =>
            {
                await Task.Yield();

                return Result<int>.Success(42);
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    #endregion

    #region Task<Result<T>>

    [Fact]
    public async Task BindAsync_Task_Generic_Result_Should_Bind_To_New_Type()
    {
        // Arrange
        Task<Result<int>> source =
            Task.FromResult(Result<int>.Success(42));

        // Act
        Result<string> result = await source.BindAsync(
            value => Result<string>.Success(
                value.ToString()));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public async Task BindAsync_Task_Generic_Result_Should_Support_Async_Binder()
    {
        // Arrange
        Task<Result<int>> source =
            Task.FromResult(Result<int>.Success(42));

        // Act
        Result<string> result = await source.BindAsync(
            async value =>
            {
                await Task.Yield();

                return Result<string>.Success(
                    value.ToString());
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    #endregion

    #region Null Guards

    [Fact]
    public void Bind_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Action act = () =>
            result.Bind(() => Result.Success());

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Bind_Should_Throw_When_Binder_Is_Null()
    {
        // Arrange
        Result result = Result.Success();
        Func<Result> binder = null!;

        // Act
        Action act = () =>
            result.Bind(binder);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("binder");
    }

    [Fact]
    public async Task BindAsync_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Func<Task> act = async () =>
            await result.BindAsync(
                value => Task.FromResult(Result.Success()));

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task BindAsync_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result> resultTask = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.BindAsync(
                () => Result.Success());

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    #endregion
}
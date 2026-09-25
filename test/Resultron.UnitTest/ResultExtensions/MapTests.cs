using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Resultron.UnitTest;

public sealed class MapTests
{
    private static readonly Error TestError =
        new("test.error", "Test error.");

    #region Result -> Result<TOut>

    [Fact]
    public void Map_Should_Map_Successful_Result_To_Value()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result<int> result = source.Map(
            () => 42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Map_Should_Invoke_Mapper_Once_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Func<int> mapper =
            Substitute.For<Func<int>>();

        mapper()
            .Returns(42);

        // Act
        Result<int> result = source.Map(mapper);

        // Assert
        result.Value.Should().Be(42);

        mapper.Received(1).Invoke();
    }

    [Fact]
    public void Map_Should_Not_Invoke_Mapper_When_Result_Is_Failure()
    {
        // Arrange
        Result source =
            Result.Failure(TestError);

        Func<int> mapper =
            Substitute.For<Func<int>>();

        // Act
        Result<int> result = source.Map(mapper);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        mapper.DidNotReceive().Invoke();
    }

    [Fact]
    public void Map_Should_Propagate_All_Reasons_When_Result_Is_Failure()
    {
        // Arrange
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        var context =
            new Success("Context.");

        Result source = Result.Failure(
            new IReason[]
            {
                error,
                context
            });

        // Act
        Result<int> result = source.Map(
            () => 42);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(source.Reasons);
    }

    #endregion

    #region Result<T> -> Result<TOut>

    [Fact]
    public void Map_Generic_Should_Map_Value_To_New_Type()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<string> result = source.Map(
            value => $"Value: {value}");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public void Map_Generic_Should_Pass_Value_To_Mapper()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Func<int, string> mapper =
            Substitute.For<Func<int, string>>();

        mapper(42)
            .Returns("mapped");

        // Act
        Result<string> result = source.Map(mapper);

        // Assert
        result.Value.Should().Be("mapped");

        mapper.Received(1)
            .Invoke(42);
    }

    [Fact]
    public void Map_Generic_Should_Not_Invoke_Mapper_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, string> mapper =
            Substitute.For<Func<int, string>>();

        // Act
        Result<string> result = source.Map(mapper);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        mapper.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    [Fact]
    public void Map_Generic_Should_Propagate_All_Reasons_When_Result_Is_Failure()
    {
        // Arrange
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        var context =
            new Success("Context.");

        Result<int> source = Result<int>.Failure(
            new IReason[]
            {
                error,
                context
            });

        // Act
        Result<string> result = source.Map(
            value => value.ToString());

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(source.Reasons);
    }

    [Fact]
    public void Map_Generic_Should_Allow_Multiple_Transformations()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(21);

        // Act
        Result<string> result = source
            .Map(value => value * 2)
            .Map(value => $"Value: {value}");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    #endregion

    #region Result -> Async Mapper

    [Fact]
    public async Task MapAsync_Should_Map_Successful_Result()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result<int> result = await source.MapAsync(
            async () =>
            {
                await Task.Yield();

                return 42;
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task MapAsync_Should_Invoke_Mapper_Once_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Func<Task<int>> mapper =
            Substitute.For<Func<Task<int>>>();

        mapper()
            .Returns(Task.FromResult(42));

        // Act
        Result<int> result =
            await source.MapAsync(mapper);

        // Assert
        result.Value.Should().Be(42);

        await mapper.Received(1).Invoke();
    }

    [Fact]
    public async Task MapAsync_Should_Not_Invoke_Mapper_When_Result_Is_Failure()
    {
        // Arrange
        Result source =
            Result.Failure(TestError);

        Func<Task<int>> mapper =
            Substitute.For<Func<Task<int>>>();

        // Act
        Result<int> result =
            await source.MapAsync(mapper);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        await mapper.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task MapAsync_Should_Propagate_Reasons_When_Result_Is_Failure()
    {
        // Arrange
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        var context =
            new Success("Context.");

        Result source = Result.Failure(
            new IReason[]
            {
                error,
                context
            });

        // Act
        Result<int> result = await source.MapAsync(
            () => Task.FromResult(42));

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(source.Reasons);
    }

    #endregion

    #region Result<T> -> Async Mapper

    [Fact]
    public async Task MapAsync_Generic_Should_Map_Value_To_New_Type()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<string> result = await source.MapAsync(
            async value =>
            {
                await Task.Yield();

                return $"Value: {value}";
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public async Task MapAsync_Generic_Should_Pass_Value_To_Mapper()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Func<int, Task<string>> mapper =
            Substitute.For<Func<int, Task<string>>>();

        mapper(42)
            .Returns(Task.FromResult("mapped"));

        // Act
        Result<string> result =
            await source.MapAsync(mapper);

        // Assert
        result.Value.Should().Be("mapped");

        await mapper.Received(1)
            .Invoke(42);
    }

    [Fact]
    public async Task MapAsync_Generic_Should_Not_Invoke_Mapper_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        Func<int, Task<string>> mapper =
            Substitute.For<Func<int, Task<string>>>();

        // Act
        Result<string> result =
            await source.MapAsync(mapper);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);

        await mapper.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    [Fact]
    public async Task MapAsync_Generic_Should_Propagate_All_Reasons_When_Result_Is_Failure()
    {
        // Arrange
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        var context =
            new Success("Context.");

        Result<int> source = Result<int>.Failure(
            new IReason[]
            {
                error,
                context
            });

        // Act
        Result<string> result = await source.MapAsync(
            value => Task.FromResult(
                value.ToString()));

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(source.Reasons);
    }

    #endregion

    #region Task<Result>

    [Fact]
    public async Task MapAsync_Task_Result_Should_Map_With_Synchronous_Mapper()
    {
        // Arrange
        Task<Result> source =
            Task.FromResult(Result.Success());

        // Act
        Result<int> result = await source.MapAsync(
            () => 42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task MapAsync_Task_Result_Should_Map_With_Asynchronous_Mapper()
    {
        // Arrange
        Task<Result> source =
            Task.FromResult(Result.Success());

        // Act
        Result<int> result = await source.MapAsync(
            async () =>
            {
                await Task.Yield();

                return 42;
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task MapAsync_Task_Result_Should_Not_Invoke_Synchronous_Mapper_When_Source_Fails()
    {
        // Arrange
        Task<Result> source =
            Task.FromResult(
                Result.Failure(TestError));

        Func<int> mapper =
            Substitute.For<Func<int>>();

        // Act
        Result<int> result =
            await source.MapAsync(mapper);

        // Assert
        result.IsFailure.Should().BeTrue();

        mapper.DidNotReceive().Invoke();
    }

    [Fact]
    public async Task MapAsync_Task_Result_Should_Not_Invoke_Asynchronous_Mapper_When_Source_Fails()
    {
        // Arrange
        Task<Result> source =
            Task.FromResult(
                Result.Failure(TestError));

        Func<Task<int>> mapper =
            Substitute.For<Func<Task<int>>>();

        // Act
        Result<int> result =
            await source.MapAsync(mapper);

        // Assert
        result.IsFailure.Should().BeTrue();

        await mapper.DidNotReceive().Invoke();
    }

    #endregion

    #region Task<Result<T>>

    [Fact]
    public async Task MapAsync_Task_Generic_Result_Should_Map_With_Synchronous_Mapper()
    {
        // Arrange
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        // Act
        Result<string> result = await source.MapAsync(
            value => $"Value: {value}");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public async Task MapAsync_Task_Generic_Result_Should_Map_With_Asynchronous_Mapper()
    {
        // Arrange
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        // Act
        Result<string> result = await source.MapAsync(
            async value =>
            {
                await Task.Yield();

                return $"Value: {value}";
            });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Value: 42");
    }

    [Fact]
    public async Task MapAsync_Task_Generic_Result_Should_Not_Invoke_Mapper_When_Source_Fails()
    {
        // Arrange
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Failure(TestError));

        Func<int, string> mapper =
            Substitute.For<Func<int, string>>();

        // Act
        Result<string> result =
            await source.MapAsync(mapper);

        // Assert
        result.IsFailure.Should().BeTrue();

        mapper.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    [Fact]
    public async Task MapAsync_Task_Generic_Result_Should_Propagate_All_Reasons()
    {
        // Arrange
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        var context =
            new Success("Context.");

        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Failure(
                    new IReason[]
                    {
                        error,
                        context
                    }));

        // Act
        Result<string> result = await source.MapAsync(
            value => value.ToString());

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should().Equal(
            error,
            context);
    }

    #endregion

    #region Null Guards

    [Fact]
    public void Map_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Action act = () =>
            result.Map(() => 42);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Map_Should_Throw_When_Mapper_Is_Null()
    {
        // Arrange
        Result result = Result.Success();
        Func<int> mapper = null!;

        // Act
        Action act = () =>
            result.Map(mapper);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("mapper");
    }

    [Fact]
    public void Map_Generic_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Action act = () =>
            result.Map(
                value => value.ToString());

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Map_Generic_Should_Throw_When_Mapper_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Func<int, string> mapper = null!;

        // Act
        Action act = () =>
            result.Map(mapper);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("mapper");
    }

    [Fact]
    public async Task MapAsync_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Func<Task> act = async () =>
            await result.MapAsync(
                () => Task.FromResult(42));

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task MapAsync_Should_Throw_When_Mapper_Is_Null()
    {
        // Arrange
        Result result = Result.Success();
        Func<Task<int>> mapper = null!;

        // Act
        Func<Task> act = async () =>
            await result.MapAsync(mapper);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("mapper");
    }

    [Fact]
    public async Task MapAsync_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result> resultTask = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.MapAsync(
                () => 42);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    [Fact]
    public async Task MapAsync_Generic_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result<int>> resultTask = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.MapAsync(
                value => value.ToString());

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    #endregion
}
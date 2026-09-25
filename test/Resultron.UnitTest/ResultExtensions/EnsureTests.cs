using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Resultron.UnitTest;

public sealed class EnsureTests
{
    private static readonly Error ValidationError =
        new(
            "validation.failed",
            "Validation failed.");

    #region Result

    [Fact]
    public void Ensure_Should_Return_Original_Result_When_Predicate_Is_True()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result result = source.Ensure(
            () => true,
            ValidationError);

        // Assert
        result.Should().BeSameAs(source);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Ensure_Should_Return_Failure_When_Predicate_Is_False()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result result = source.Ensure(
            () => false,
            ValidationError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ValidationError);
    }

    [Fact]
    public void Ensure_Should_Invoke_Predicate_Once_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Func<bool> predicate =
            Substitute.For<Func<bool>>();

        predicate()
            .Returns(true);

        // Act
        _ = source.Ensure(
            predicate,
            ValidationError);

        // Assert
        predicate.Received(1).Invoke();
    }

    [Fact]
    public void Ensure_Should_Not_Invoke_Predicate_When_Result_Is_Already_Failure()
    {
        // Arrange
        var sourceError =
            new Error(
                "source.failed",
                "Source failed.");

        Result source =
            Result.Failure(sourceError);

        Func<bool> predicate =
            Substitute.For<Func<bool>>();

        // Act
        Result result = source.Ensure(
            predicate,
            ValidationError);

        // Assert
        result.Should().BeSameAs(source);
        result.Error.Should().Be(sourceError);

        predicate.DidNotReceive().Invoke();
    }

    [Fact]
    public void Ensure_Should_Preserve_Existing_Reasons_When_Result_Is_Already_Failure()
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
        Result result = source.Ensure(
            () => true,
            ValidationError);

        // Assert
        result.Should().BeSameAs(source);

        result.Reasons.Should()
            .BeEquivalentTo(source.Reasons);
    }

    #endregion

    #region Result<T>

    [Fact]
    public void Ensure_Generic_Should_Return_Original_Result_When_Predicate_Is_True()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result = source.Ensure(
            value => value > 0,
            ValidationError);

        // Assert
        result.Should().BeSameAs(source);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Ensure_Generic_Should_Return_Failure_When_Predicate_Is_False()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result = source.Ensure(
            value => value < 0,
            ValidationError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ValidationError);
    }

    [Fact]
    public void Ensure_Generic_Should_Pass_Value_To_Predicate()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Func<int, bool> predicate =
            Substitute.For<Func<int, bool>>();

        predicate(42)
            .Returns(true);

        // Act
        Result<int> result = source.Ensure(
            predicate,
            ValidationError);

        // Assert
        result.IsSuccess.Should().BeTrue();

        predicate.Received(1)
            .Invoke(42);
    }

    [Fact]
    public void Ensure_Generic_Should_Not_Invoke_Predicate_When_Result_Is_Failure()
    {
        // Arrange
        var sourceError =
            new Error(
                "source.failed",
                "Source failed.");

        Result<int> source =
            Result<int>.Failure(sourceError);

        Func<int, bool> predicate =
            Substitute.For<Func<int, bool>>();

        // Act
        Result<int> result = source.Ensure(
            predicate,
            ValidationError);

        // Assert
        result.Should().BeSameAs(source);
        result.Error.Should().Be(sourceError);

        predicate.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    [Fact]
    public void Ensure_Generic_Should_Not_Expose_Value_When_Validation_Fails()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Result<int> result = source.Ensure(
            value => value < 0,
            ValidationError);

        // Act
        Action act = () =>
        {
            _ = result.Value;
        };

        // Assert
        result.IsFailure.Should().BeTrue();

        act.Should()
            .Throw<InvalidOperationException>();
    }

    #endregion

    #region Result Async

    [Fact]
    public async Task EnsureAsync_Should_Return_Original_Result_When_Predicate_Is_True()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result result = await source.EnsureAsync(
            () => Task.FromResult(true),
            ValidationError);

        // Assert
        result.Should().BeSameAs(source);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task EnsureAsync_Should_Return_Failure_When_Predicate_Is_False()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result result = await source.EnsureAsync(
            () => Task.FromResult(false),
            ValidationError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ValidationError);
    }

    [Fact]
    public async Task EnsureAsync_Should_Invoke_Predicate_Once_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        Func<Task<bool>> predicate =
            Substitute.For<Func<Task<bool>>>();

        predicate()
            .Returns(Task.FromResult(true));

        // Act
        _ = await source.EnsureAsync(
            predicate,
            ValidationError);

        // Assert
        await predicate.Received(1).Invoke();
    }

    [Fact]
    public async Task EnsureAsync_Should_Not_Invoke_Predicate_When_Result_Is_Failure()
    {
        // Arrange
        Result source =
            Result.Failure(
                new Error(
                    "source.failed",
                    "Source failed."));

        Func<Task<bool>> predicate =
            Substitute.For<Func<Task<bool>>>();

        // Act
        Result result = await source.EnsureAsync(
            predicate,
            ValidationError);

        // Assert
        result.Should().BeSameAs(source);

        await predicate.DidNotReceive().Invoke();
    }

    #endregion

    #region Result<T> Async

    [Fact]
    public async Task EnsureAsync_Generic_Should_Return_Original_Result_When_Predicate_Is_True()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result = await source.EnsureAsync(
            value => Task.FromResult(value > 0),
            ValidationError);

        // Assert
        result.Should().BeSameAs(source);
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task EnsureAsync_Generic_Should_Return_Failure_When_Predicate_Is_False()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<int> result = await source.EnsureAsync(
            value => Task.FromResult(value < 0),
            ValidationError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ValidationError);
    }

    [Fact]
    public async Task EnsureAsync_Generic_Should_Pass_Value_To_Predicate()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        Func<int, Task<bool>> predicate =
            Substitute.For<Func<int, Task<bool>>>();

        predicate(42)
            .Returns(Task.FromResult(true));

        // Act
        Result<int> result = await source.EnsureAsync(
            predicate,
            ValidationError);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await predicate.Received(1)
            .Invoke(42);
    }

    [Fact]
    public async Task EnsureAsync_Generic_Should_Not_Invoke_Predicate_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(
                new Error(
                    "source.failed",
                    "Source failed."));

        Func<int, Task<bool>> predicate =
            Substitute.For<Func<int, Task<bool>>>();

        // Act
        Result<int> result = await source.EnsureAsync(
            predicate,
            ValidationError);

        // Assert
        result.Should().BeSameAs(source);

        await predicate.DidNotReceive()
            .Invoke(Arg.Any<int>());
    }

    #endregion

    #region Task<Result>

    [Fact]
    public async Task EnsureAsync_Task_Result_Should_Evaluate_Synchronous_Predicate()
    {
        // Arrange
        Task<Result> source =
            Task.FromResult(Result.Success());

        // Act
        Result result = await source.EnsureAsync(
            () => true,
            ValidationError);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task EnsureAsync_Task_Result_Should_Evaluate_Asynchronous_Predicate()
    {
        // Arrange
        Task<Result> source =
            Task.FromResult(Result.Success());

        // Act
        Result result = await source.EnsureAsync(
            () => Task.FromResult(true),
            ValidationError);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task EnsureAsync_Task_Result_Should_Return_Failure_When_Predicate_Fails()
    {
        // Arrange
        Task<Result> source =
            Task.FromResult(Result.Success());

        // Act
        Result result = await source.EnsureAsync(
            () => false,
            ValidationError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ValidationError);
    }

    #endregion

    #region Task<Result<T>>

    [Fact]
    public async Task EnsureAsync_Task_Generic_Result_Should_Evaluate_Synchronous_Predicate()
    {
        // Arrange
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        // Act
        Result<int> result = await source.EnsureAsync(
            value => value > 0,
            ValidationError);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task EnsureAsync_Task_Generic_Result_Should_Evaluate_Asynchronous_Predicate()
    {
        // Arrange
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        // Act
        Result<int> result = await source.EnsureAsync(
            value => Task.FromResult(value > 0),
            ValidationError);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task EnsureAsync_Task_Generic_Result_Should_Return_Failure_When_Predicate_Fails()
    {
        // Arrange
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        // Act
        Result<int> result = await source.EnsureAsync(
            value => value < 0,
            ValidationError);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ValidationError);
    }

    #endregion

    #region Null Guards

    [Fact]
    public void Ensure_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Action act = () =>
            result.Ensure(
                () => true,
                ValidationError);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void Ensure_Should_Throw_When_Predicate_Is_Null()
    {
        // Arrange
        Result result = Result.Success();
        Func<bool> predicate = null!;

        // Act
        Action act = () =>
            result.Ensure(
                predicate,
                ValidationError);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("predicate");
    }

    [Fact]
    public void Ensure_Should_Throw_When_Error_Is_Null()
    {
        // Arrange
        Result result = Result.Success();
        Error error = null!;

        // Act
        Action act = () =>
            result.Ensure(
                () => true,
                error);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("error");
    }

    [Fact]
    public void Ensure_Generic_Should_Throw_When_Predicate_Is_Null()
    {
        // Arrange
        Result<int> result =
            Result<int>.Success(42);

        Func<int, bool> predicate = null!;

        // Act
        Action act = () =>
            result.Ensure(
                predicate,
                ValidationError);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("predicate");
    }

    [Fact]
    public async Task EnsureAsync_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result> resultTask = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.EnsureAsync(
                () => true,
                ValidationError);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    [Fact]
    public async Task EnsureAsync_Generic_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result<int>> resultTask = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.EnsureAsync(
                value => value > 0,
                ValidationError);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    #endregion
}
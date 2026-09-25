using FluentAssertions;
using Xunit;

namespace Resultron.UnitTest;

public sealed class ConversionTests
{
    private static readonly Error TestError =
        new("test.error", "Test error.");

    #region ToUnitResult - Result

    [Fact]
    public void ToUnitResult_Should_Return_Success_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result<Unit> result =
            source.ToUnitResult();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void ToUnitResult_Should_Return_Unit_Value_When_Result_Is_Success()
    {
        // Arrange
        Result source = Result.Success();

        // Act
        Result<Unit> result =
            source.ToUnitResult();

        // Assert
        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public void ToUnitResult_Should_Return_Failure_When_Result_Is_Failure()
    {
        // Arrange
        Result source =
            Result.Failure(TestError);

        // Act
        Result<Unit> result =
            source.ToUnitResult();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void ToUnitResult_Should_Propagate_All_Reasons_When_Result_Is_Failure()
    {
        // Arrange
        var context =
            new Success("Context.");

        Result source = Result.Failure(
            new IReason[]
            {
                TestError,
                context
            });

        // Act
        Result<Unit> result =
            source.ToUnitResult();

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(source.Reasons);
    }

    [Fact]
    public void ToUnitResult_Should_Preserve_Reason_Order_When_Result_Is_Failure()
    {
        // Arrange
        var firstError =
            new Error(
                "first.error",
                "First error.");

        var context =
            new Success("Context.");

        var secondError =
            new Error(
                "second.error",
                "Second error.");

        Result source = Result.Failure(
            new IReason[]
            {
                firstError,
                context,
                secondError
            });

        // Act
        Result<Unit> result =
            source.ToUnitResult();

        // Assert
        result.Reasons.Should().Equal(
            firstError,
            context,
            secondError);
    }

    #endregion

    #region ToUnitResult - Result<T>

    [Fact]
    public void ToUnitResult_Generic_Should_Return_Success_When_Result_Is_Success()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<Unit> result =
            source.ToUnitResult();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public void ToUnitResult_Generic_Should_Discard_Source_Value()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result<Unit> result =
            source.ToUnitResult();

        // Assert
        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public void ToUnitResult_Generic_Should_Return_Failure_When_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        // Act
        Result<Unit> result =
            source.ToUnitResult();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void ToUnitResult_Generic_Should_Propagate_All_Reasons_When_Result_Is_Failure()
    {
        // Arrange
        var context =
            new Success("Context.");

        Result<int> source =
            Result<int>.Failure(
                new IReason[]
                {
                    TestError,
                    context
                });

        // Act
        Result<Unit> result =
            source.ToUnitResult();

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(source.Reasons);
    }

    [Fact]
    public void ToUnitResult_Generic_Should_Preserve_Reason_Order_When_Result_Is_Failure()
    {
        // Arrange
        var firstError =
            new Error(
                "first.error",
                "First error.");

        var context =
            new Success("Context.");

        var secondError =
            new Error(
                "second.error",
                "Second error.");

        Result<int> source =
            Result<int>.Failure(
                new IReason[]
                {
                    firstError,
                    context,
                    secondError
                });

        // Act
        Result<Unit> result =
            source.ToUnitResult();

        // Assert
        result.Reasons.Should().Equal(
            firstError,
            context,
            secondError);
    }

    #endregion

    #region ToNonGenericResult

    [Fact]
    public void ToNonGenericResult_Should_Return_Success_When_Generic_Result_Is_Success()
    {
        // Arrange
        Result<int> source =
            Result<int>.Success(42);

        // Act
        Result result =
            source.ToNonGenericResult();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void ToNonGenericResult_Should_Return_Failure_When_Generic_Result_Is_Failure()
    {
        // Arrange
        Result<int> source =
            Result<int>.Failure(TestError);

        // Act
        Result result =
            source.ToNonGenericResult();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void ToNonGenericResult_Should_Propagate_All_Reasons_When_Result_Is_Failure()
    {
        // Arrange
        var context =
            new Success("Context.");

        Result<int> source =
            Result<int>.Failure(
                new IReason[]
                {
                    TestError,
                    context
                });

        // Act
        Result result =
            source.ToNonGenericResult();

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(source.Reasons);
    }

    [Fact]
    public void ToNonGenericResult_Should_Preserve_Reason_Order_When_Result_Is_Failure()
    {
        // Arrange
        var firstError =
            new Error(
                "first.error",
                "First error.");

        var context =
            new Success("Context.");

        var secondError =
            new Error(
                "second.error",
                "Second error.");

        Result<int> source =
            Result<int>.Failure(
                new IReason[]
                {
                    firstError,
                    context,
                    secondError
                });

        // Act
        Result result =
            source.ToNonGenericResult();

        // Assert
        result.Reasons.Should().Equal(
            firstError,
            context,
            secondError);
    }

    #endregion

    #region ToUnitResultAsync - Task<Result>

    [Fact]
    public async Task ToUnitResultAsync_Should_Return_Success_When_Result_Is_Success()
    {
        // Arrange
        Task<Result> source =
            Task.FromResult(Result.Success());

        // Act
        Result<Unit> result =
            await source.ToUnitResultAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public async Task ToUnitResultAsync_Should_Return_Failure_When_Result_Is_Failure()
    {
        // Arrange
        Task<Result> source =
            Task.FromResult(
                Result.Failure(TestError));

        // Act
        Result<Unit> result =
            await source.ToUnitResultAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task ToUnitResultAsync_Should_Propagate_All_Reasons_When_Result_Is_Failure()
    {
        // Arrange
        var context =
            new Success("Context.");

        Result sourceResult =
            Result.Failure(
                new IReason[]
                {
                    TestError,
                    context
                });

        Task<Result> source =
            Task.FromResult(sourceResult);

        // Act
        Result<Unit> result =
            await source.ToUnitResultAsync();

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(sourceResult.Reasons);
    }

    #endregion

    #region ToUnitResultAsync - Task<Result<T>>

    [Fact]
    public async Task ToUnitResultAsync_Generic_Should_Return_Success_When_Result_Is_Success()
    {
        // Arrange
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        // Act
        Result<Unit> result =
            await source.ToUnitResultAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public async Task ToUnitResultAsync_Generic_Should_Return_Failure_When_Result_Is_Failure()
    {
        // Arrange
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Failure(TestError));

        // Act
        Result<Unit> result =
            await source.ToUnitResultAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task ToUnitResultAsync_Generic_Should_Propagate_All_Reasons_When_Result_Is_Failure()
    {
        // Arrange
        var context =
            new Success("Context.");

        Result<int> sourceResult =
            Result<int>.Failure(
                new IReason[]
                {
                    TestError,
                    context
                });

        Task<Result<int>> source =
            Task.FromResult(sourceResult);

        // Act
        Result<Unit> result =
            await source.ToUnitResultAsync();

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(sourceResult.Reasons);
    }

    #endregion

    #region ToNonGenericResultAsync

    [Fact]
    public async Task ToNonGenericResultAsync_Should_Return_Success_When_Result_Is_Success()
    {
        // Arrange
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        // Act
        Result result =
            await source.ToNonGenericResultAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public async Task ToNonGenericResultAsync_Should_Return_Failure_When_Result_Is_Failure()
    {
        // Arrange
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Failure(TestError));

        // Act
        Result result =
            await source.ToNonGenericResultAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task ToNonGenericResultAsync_Should_Propagate_All_Reasons_When_Result_Is_Failure()
    {
        // Arrange
        var context =
            new Success("Context.");

        Result<int> sourceResult =
            Result<int>.Failure(
                new IReason[]
                {
                    TestError,
                    context
                });

        Task<Result<int>> source =
            Task.FromResult(sourceResult);

        // Act
        Result result =
            await source.ToNonGenericResultAsync();

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Reasons.Should()
            .BeEquivalentTo(sourceResult.Reasons);
    }

    #endregion

    #region Null Guards

    [Fact]
    public void ToUnitResult_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result result = null!;

        // Act
        Action act = () =>
            result.ToUnitResult();

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void ToUnitResult_Generic_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Action act = () =>
            result.ToUnitResult();

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void ToNonGenericResult_Should_Throw_When_Result_Is_Null()
    {
        // Arrange
        Result<int> result = null!;

        // Act
        Action act = () =>
            result.ToNonGenericResult();

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task ToUnitResultAsync_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result> resultTask = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.ToUnitResultAsync();

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    [Fact]
    public async Task ToUnitResultAsync_Generic_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result<int>> resultTask = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.ToUnitResultAsync();

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    [Fact]
    public async Task ToNonGenericResultAsync_Should_Throw_When_ResultTask_Is_Null()
    {
        // Arrange
        Task<Result<int>> resultTask = null!;

        // Act
        Func<Task> act = async () =>
            await resultTask.ToNonGenericResultAsync();

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    #endregion
}
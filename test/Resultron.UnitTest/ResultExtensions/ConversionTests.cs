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
        Result source = Result.Success();

        Result<Unit> result =
            source.ToUnitResult();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void ToUnitResult_Should_Return_Unit_Value_When_Result_Is_Success()
    {
        Result source = Result.Success();

        Result<Unit> result =
            source.ToUnitResult();

        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public void ToUnitResult_Should_Return_Failure_When_Result_Is_Failure()
    {
        Result source =
            Result.Failure(TestError);

        Result<Unit> result =
            source.ToUnitResult();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void ToUnitResult_Should_Propagate_Error_When_Result_Is_Failure()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Result source =
            Result.Failure(error);

        Result<Unit> result =
            source.ToUnitResult();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region ToUnitResult - Result<T>

    [Fact]
    public void ToUnitResult_Generic_Should_Return_Success_When_Result_Is_Success()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<Unit> result =
            source.ToUnitResult();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public void ToUnitResult_Generic_Should_Discard_Source_Value()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result<Unit> result =
            source.ToUnitResult();

        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public void ToUnitResult_Generic_Should_Return_Failure_When_Result_Is_Failure()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Result<Unit> result =
            source.ToUnitResult();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void ToUnitResult_Generic_Should_Propagate_Error_When_Result_Is_Failure()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Result<int> source =
            Result<int>.Failure(error);

        Result<Unit> result =
            source.ToUnitResult();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region ToNonGenericResult

    [Fact]
    public void ToNonGenericResult_Should_Return_Success_When_Generic_Result_Is_Success()
    {
        Result<int> source =
            Result<int>.Success(42);

        Result result =
            source.ToNonGenericResult();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void ToNonGenericResult_Should_Return_Failure_When_Generic_Result_Is_Failure()
    {
        Result<int> source =
            Result<int>.Failure(TestError);

        Result result =
            source.ToNonGenericResult();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public void ToNonGenericResult_Should_Propagate_Error_When_Result_Is_Failure()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Result<int> source =
            Result<int>.Failure(error);

        Result result =
            source.ToNonGenericResult();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region ToUnitResultAsync - Task<Result>

    [Fact]
    public async Task ToUnitResultAsync_Should_Return_Success_When_Result_Is_Success()
    {
        Task<Result> source =
            Task.FromResult(Result.Success());

        Result<Unit> result =
            await source.ToUnitResultAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public async Task ToUnitResultAsync_Should_Return_Failure_When_Result_Is_Failure()
    {
        Task<Result> source =
            Task.FromResult(
                Result.Failure(TestError));

        Result<Unit> result =
            await source.ToUnitResultAsync();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task ToUnitResultAsync_Should_Propagate_Error_When_Result_Is_Failure()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Task<Result> source =
            Task.FromResult(
                Result.Failure(error));

        Result<Unit> result =
            await source.ToUnitResultAsync();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region ToUnitResultAsync - Task<Result<T>>

    [Fact]
    public async Task ToUnitResultAsync_Generic_Should_Return_Success_When_Result_Is_Success()
    {
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        Result<Unit> result =
            await source.ToUnitResultAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
    }

    [Fact]
    public async Task ToUnitResultAsync_Generic_Should_Return_Failure_When_Result_Is_Failure()
    {
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Failure(TestError));

        Result<Unit> result =
            await source.ToUnitResultAsync();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task ToUnitResultAsync_Generic_Should_Propagate_Error_When_Result_Is_Failure()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Failure(error));

        Result<Unit> result =
            await source.ToUnitResultAsync();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region ToNonGenericResultAsync

    [Fact]
    public async Task ToNonGenericResultAsync_Should_Return_Success_When_Result_Is_Success()
    {
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Success(42));

        Result result =
            await source.ToNonGenericResultAsync();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public async Task ToNonGenericResultAsync_Should_Return_Failure_When_Result_Is_Failure()
    {
        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Failure(TestError));

        Result result =
            await source.ToNonGenericResultAsync();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TestError);
    }

    [Fact]
    public async Task ToNonGenericResultAsync_Should_Propagate_Error_When_Result_Is_Failure()
    {
        var error =
            new Error(
                "source.failed",
                "Source failed.");

        Task<Result<int>> source =
            Task.FromResult(
                Result<int>.Failure(error));

        Result result =
            await source.ToNonGenericResultAsync();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    #endregion

    #region Null Guards

    [Fact]
    public void ToUnitResult_Should_Throw_When_Result_Is_Null()
    {
        Result result = null!;

        Action act = () =>
            result.ToUnitResult();

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void ToUnitResult_Generic_Should_Throw_When_Result_Is_Null()
    {
        Result<int> result = null!;

        Action act = () =>
            result.ToUnitResult();

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public void ToNonGenericResult_Should_Throw_When_Result_Is_Null()
    {
        Result<int> result = null!;

        Action act = () =>
            result.ToNonGenericResult();

        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("result");
    }

    [Fact]
    public async Task ToUnitResultAsync_Should_Throw_When_ResultTask_Is_Null()
    {
        Task<Result> resultTask = null!;

        Func<Task> act = async () =>
            await resultTask.ToUnitResultAsync();

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    [Fact]
    public async Task ToUnitResultAsync_Generic_Should_Throw_When_ResultTask_Is_Null()
    {
        Task<Result<int>> resultTask = null!;

        Func<Task> act = async () =>
            await resultTask.ToUnitResultAsync();

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    [Fact]
    public async Task ToNonGenericResultAsync_Should_Throw_When_ResultTask_Is_Null()
    {
        Task<Result<int>> resultTask = null!;

        Func<Task> act = async () =>
            await resultTask.ToNonGenericResultAsync();

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTask");
    }

    #endregion
}
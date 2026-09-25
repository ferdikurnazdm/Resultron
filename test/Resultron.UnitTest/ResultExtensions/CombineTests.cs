using FluentAssertions;
using Xunit;

namespace Resultron.UnitTest;

public sealed class CombineTests
{
    private static readonly Error FirstError =
        new("first.error", "First error.");

    private static readonly Error SecondError =
        new("second.error", "Second error.");

    #region Result

    [Fact]
    public void Combine_Should_Return_Success_When_All_Results_Are_Successful()
    {
        // Arrange
        Result[] results =
        [
            Result.Success(),
            Result.Success(),
            Result.Success()
        ];

        // Act
        Result combined = results.Combine();

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.IsFailure.Should().BeFalse();
        combined.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Combine_Should_Return_Failure_When_Any_Result_Has_Failed()
    {
        // Arrange
        Result[] results =
        [
            Result.Success(),
            Result.Failure(FirstError),
            Result.Success()
        ];

        // Act
        Result combined = results.Combine();

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Errors.Should().ContainSingle();
        combined.Error.Should().Be(FirstError);
    }

    [Fact]
    public void Combine_Should_Accumulate_Errors_From_All_Failed_Results()
    {
        // Arrange
        Result[] results =
        [
            Result.Success(),
            Result.Failure(FirstError),
            Result.Failure(SecondError)
        ];

        // Act
        Result combined = results.Combine();

        // Assert
        combined.IsFailure.Should().BeTrue();

        combined.Errors.Should().Equal(
            FirstError,
            SecondError);
    }

    [Fact]
    public void Combine_Should_Accumulate_Multiple_Errors_From_A_Single_Result()
    {
        // Arrange
        Result[] results =
        [
            Result.Failure(
            [
                FirstError,
                SecondError
            ])
        ];

        // Act
        Result combined = results.Combine();

        // Assert
        combined.IsFailure.Should().BeTrue();

        combined.Errors.Should().Equal(
            FirstError,
            SecondError);
    }

    [Fact]
    public void Combine_Should_Return_Success_For_Empty_Result_Collection()
    {
        // Arrange
        Result[] results = [];

        // Act
        Result combined = results.Combine();

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Combine_Enumerable_Should_Combine_Results()
    {
        // Arrange
        IEnumerable<Result> results =
        [
            Result.Success(),
            Result.Success()
        ];

        // Act
        Result combined = results.Combine();

        // Assert
        combined.IsSuccess.Should().BeTrue();
    }

    #endregion

    #region Result<T>

    [Fact]
    public void Combine_Generic_Should_Return_All_Values_When_All_Results_Succeed()
    {
        // Arrange
        Result<int>[] results =
        [
            Result<int>.Success(10),
            Result<int>.Success(20),
            Result<int>.Success(30)
        ];

        // Act
        Result<IReadOnlyList<int>> combined =
            results.Combine();

        // Assert
        combined.IsSuccess.Should().BeTrue();

        combined.Value.Should().Equal(
            10,
            20,
            30);
    }

    [Fact]
    public void Combine_Generic_Should_Preserve_Value_Order()
    {
        // Arrange
        Result<string>[] results =
        [
            Result<string>.Success("first"),
            Result<string>.Success("second"),
            Result<string>.Success("third")
        ];

        // Act
        Result<IReadOnlyList<string>> combined =
            results.Combine();

        // Assert
        combined.Value.Should().Equal(
            "first",
            "second",
            "third");
    }

    [Fact]
    public void Combine_Generic_Should_Return_Failure_When_Any_Result_Fails()
    {
        // Arrange
        Result<int>[] results =
        [
            Result<int>.Success(10),
            Result<int>.Failure(FirstError),
            Result<int>.Success(30)
        ];

        // Act
        Result<IReadOnlyList<int>> combined =
            results.Combine();

        // Assert
        combined.IsFailure.Should().BeTrue();
        combined.Errors.Should().ContainSingle();
        combined.Error.Should().Be(FirstError);
    }

    [Fact]
    public void Combine_Generic_Should_Accumulate_All_Errors()
    {
        // Arrange
        Result<int>[] results =
        [
            Result<int>.Failure(FirstError),
            Result<int>.Success(20),
            Result<int>.Failure(SecondError)
        ];

        // Act
        Result<IReadOnlyList<int>> combined =
            results.Combine();

        // Assert
        combined.IsFailure.Should().BeTrue();

        combined.Errors.Should().Equal(
            FirstError,
            SecondError);
    }

    [Fact]
    public void Combine_Generic_Should_Return_Empty_Value_List_For_Empty_Collection()
    {
        // Arrange
        Result<int>[] results = [];

        // Act
        Result<IReadOnlyList<int>> combined =
            results.Combine();

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().BeEmpty();
    }

    [Fact]
    public void Combine_Generic_Enumerable_Should_Combine_Values()
    {
        // Arrange
        IEnumerable<Result<int>> results =
        [
            Result<int>.Success(10),
            Result<int>.Success(20)
        ];

        // Act
        Result<IReadOnlyList<int>> combined =
            results.Combine();

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Equal(10, 20);
    }

    #endregion

    #region Result Async

    [Fact]
    public async Task CombineAsync_Should_Return_Success_When_All_Tasks_Succeed()
    {
        // Arrange
        Task<Result>[] tasks =
        [
            Task.FromResult(Result.Success()),
            Task.FromResult(Result.Success()),
            Task.FromResult(Result.Success())
        ];

        // Act
        Result combined =
            await tasks.CombineAsync();

        // Assert
        combined.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CombineAsync_Should_Accumulate_Errors_From_Failed_Tasks()
    {
        // Arrange
        Task<Result>[] tasks =
        [
            Task.FromResult(Result.Success()),
            Task.FromResult(Result.Failure(FirstError)),
            Task.FromResult(Result.Failure(SecondError))
        ];

        // Act
        Result combined =
            await tasks.CombineAsync();

        // Assert
        combined.IsFailure.Should().BeTrue();

        combined.Errors.Should().Equal(
            FirstError,
            SecondError);
    }

    [Fact]
    public async Task CombineAsync_Enumerable_Should_Combine_Results()
    {
        // Arrange
        IEnumerable<Task<Result>> tasks =
        [
            Task.FromResult(Result.Success()),
            Task.FromResult(Result.Success())
        ];

        // Act
        Result combined =
            await tasks.CombineAsync();

        // Assert
        combined.IsSuccess.Should().BeTrue();
    }

    #endregion

    #region Result<T> Async

    [Fact]
    public async Task CombineAsync_Generic_Should_Return_All_Values_When_All_Tasks_Succeed()
    {
        // Arrange
        Task<Result<int>>[] tasks =
        [
            Task.FromResult(Result<int>.Success(10)),
            Task.FromResult(Result<int>.Success(20)),
            Task.FromResult(Result<int>.Success(30))
        ];

        // Act
        Result<IReadOnlyList<int>> combined =
            await tasks.CombineAsync();

        // Assert
        combined.IsSuccess.Should().BeTrue();

        combined.Value.Should().Equal(
            10,
            20,
            30);
    }

    [Fact]
    public async Task CombineAsync_Generic_Should_Accumulate_All_Errors()
    {
        // Arrange
        Task<Result<int>>[] tasks =
        [
            Task.FromResult(
                Result<int>.Failure(FirstError)),

            Task.FromResult(
                Result<int>.Success(20)),

            Task.FromResult(
                Result<int>.Failure(SecondError))
        ];

        // Act
        Result<IReadOnlyList<int>> combined =
            await tasks.CombineAsync();

        // Assert
        combined.IsFailure.Should().BeTrue();

        combined.Errors.Should().Equal(
            FirstError,
            SecondError);
    }

    [Fact]
    public async Task CombineAsync_Generic_Should_Preserve_Value_Order()
    {
        // Arrange
        Task<Result<int>>[] tasks =
        [
            Task.FromResult(Result<int>.Success(3)),
            Task.FromResult(Result<int>.Success(1)),
            Task.FromResult(Result<int>.Success(2))
        ];

        // Act
        Result<IReadOnlyList<int>> combined =
            await tasks.CombineAsync();

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Equal(3, 1, 2);
    }

    [Fact]
    public async Task CombineAsync_Generic_Enumerable_Should_Combine_Values()
    {
        // Arrange
        IEnumerable<Task<Result<int>>> tasks =
        [
            Task.FromResult(Result<int>.Success(10)),
            Task.FromResult(Result<int>.Success(20))
        ];

        // Act
        Result<IReadOnlyList<int>> combined =
            await tasks.CombineAsync();

        // Assert
        combined.IsSuccess.Should().BeTrue();
        combined.Value.Should().Equal(10, 20);
    }

    #endregion

    #region Null Guards

    [Fact]
    public void Combine_Should_Throw_When_Result_Array_Is_Null()
    {
        // Arrange
        Result[] results = null!;

        // Act
        Action act = () => results.Combine();

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("results");
    }

    [Fact]
    public void Combine_Should_Throw_When_Result_Enumerable_Is_Null()
    {
        // Arrange
        IEnumerable<Result> results = null!;

        // Act
        Action act = () => results.Combine();

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("results");
    }

    [Fact]
    public void Combine_Generic_Should_Throw_When_Result_Array_Is_Null()
    {
        // Arrange
        Result<int>[] results = null!;

        // Act
        Action act = () => results.Combine();

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("results");
    }

    [Fact]
    public void Combine_Generic_Should_Throw_When_Result_Enumerable_Is_Null()
    {
        // Arrange
        IEnumerable<Result<int>> results = null!;

        // Act
        Action act = () => results.Combine();

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("results");
    }

    [Fact]
    public async Task CombineAsync_Should_Throw_When_Task_Array_Is_Null()
    {
        // Arrange
        Task<Result>[] tasks = null!;

        // Act
        Func<Task> act = async () =>
            await tasks.CombineAsync();

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTasks");
    }

    [Fact]
    public async Task CombineAsync_Should_Throw_When_Task_Enumerable_Is_Null()
    {
        // Arrange
        IEnumerable<Task<Result>> tasks = null!;

        // Act
        Func<Task> act = async () =>
            await tasks.CombineAsync();

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTasks");
    }

    [Fact]
    public async Task CombineAsync_Generic_Should_Throw_When_Task_Array_Is_Null()
    {
        // Arrange
        Task<Result<int>>[] tasks = null!;

        // Act
        Func<Task> act = async () =>
            await tasks.CombineAsync();

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTasks");
    }

    [Fact]
    public async Task CombineAsync_Generic_Should_Throw_When_Task_Enumerable_Is_Null()
    {
        // Arrange
        IEnumerable<Task<Result<int>>> tasks = null!;

        // Act
        Func<Task> act = async () =>
            await tasks.CombineAsync();

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithParameterName("resultTasks");
    }

    #endregion
}